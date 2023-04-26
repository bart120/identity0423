using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net.Http.Headers;
using System.Net.Mime;
using WebApp.Handlers;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")/*, options =>
    {
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async cookieCtx =>
            {
                var now = DateTimeOffset.UtcNow;
                var expiresAt = cookieCtx.Properties.GetTokenValue("expires_at");
                var accessTokenExpiration = DateTimeOffset.Parse(expiresAt);
                var timeRemaining = accessTokenExpiration.Subtract(now);

                var refreshThresholdMinutes = 5;
                var refreshThreshold = TimeSpan.FromMinutes(refreshThresholdMinutes);

                if (timeRemaining < refreshThreshold)
                {
                    var refreshToken = cookieCtx.Properties.GetTokenValue("refresh_token");

                    var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
                    {
                        Address = "https://localhost:7151/connect/token",
                        ClientId = "client_mvc",
                        ClientSecret = "secret_mvc",
                        RefreshToken = refreshToken
                    });

                    if (!response.IsError)
                    {
                        var expiresInSeconds = response.ExpiresIn;
                        var updatedExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds);
                        cookieCtx.Properties.UpdateTokenValue("expires_at", updatedExpiresAt.ToString());
                        cookieCtx.Properties.UpdateTokenValue("access_token", response.AccessToken);
                        cookieCtx.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);


                        cookieCtx.ShouldRenew = true;
                    }
                    else
                    {
                        cookieCtx.RejectPrincipal();
                        await cookieCtx.HttpContext.SignOutAsync();
                    }
                }
            }
        };
    })*/
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7151";

        options.ClientId = "client_mvc";
        options.ClientSecret = "secret_mvc";
        options.ResponseType = "code";
        options.UsePkce = true;
        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("api_demo_scope");
        options.Scope.Add("offline_access");

        options.GetClaimsFromUserInfoEndpoint= true;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType= "role"
        };
    });

builder.Services.AddHttpContextAccessor();
//IUserAccessTokenManagementService
builder.Services.AddAccessTokenManagement();
/*builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<HttpClientAuthorizationHandler>();*/
builder.Services.AddRefitClient<IWeatherService>().ConfigureHttpClient(c =>
{
    var url = "https://localhost:7252";
    c.BaseAddress = new Uri(url);
    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
}).AddUserAccessTokenHandler();
    //.AddHttpMessageHandler<HttpClientAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//.RequireAuthorization();

app.Run();
