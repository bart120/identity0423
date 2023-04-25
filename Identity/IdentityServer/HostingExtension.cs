using IdentityServer.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public static class HostingExtension
    {


        public static WebApplication ConfigureServices(this WebApplicationBuilder app)
        {
            var assemblyName = typeof(Program).Assembly.GetName().Name;
            
            //Duende.IdentityServer.EntityFramework.DbContexts.PersistedGrantDbContext
            app.Services.AddIdentityServer(conf =>
            {
                //conf.Endpoints.EnableIntrospectionEndpoint = false;
                //conf.Endpoints.EnableUserInfoEndpoint = false;
                conf.UserInteraction.LoginUrl = "/login";
                conf.UserInteraction.LogoutUrl = "/logout";
            }).AddConfigurationStore(store =>
            {
                store.DefaultSchema = "configuation";
                store.ConfigureDbContext = db => db.UseNpgsql(app.Configuration.GetConnectionString("IdentityConnection"), 
                    sql => sql.MigrationsAssembly(assemblyName));
            }).AddOperationalStore(store =>
            {
                store.DefaultSchema = "operational";
                store.ConfigureDbContext = db => db.UseNpgsql(app.Configuration.GetConnectionString("IdentityConnection"),
                    sql => sql.MigrationsAssembly(assemblyName));
            }).AddTestUsers(MockUsers.Users);
                // IN MEMORY
                //.AddInMemoryApiScopes(InMemory.ConfigInMemory.GetScopes())
                //.AddInMemoryClients(InMemory.ConfigInMemory.GetClients());
           

            return app.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app) 
        {
            app.UseDeveloperExceptionPage();
            app.UseIdentityServer();

            return app;
        }
    }
}
