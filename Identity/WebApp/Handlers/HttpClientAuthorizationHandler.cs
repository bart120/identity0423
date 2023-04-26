using Microsoft.AspNetCore.Authentication;

namespace WebApp.Handlers
{
    public class HttpClientAuthorizationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpClientAuthorizationHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessor.HttpContext.GetTokenAsync("access_token");
            var refre = await _accessor.HttpContext.GetTokenAsync("refresh_token");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            //request.Headers.Add("Authorization", $"Bearer {accessToken}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
