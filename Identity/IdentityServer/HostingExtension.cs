using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public static class HostingExtension
    {


        public static WebApplication ConfigureServices(this WebApplicationBuilder app)
        {


            app.Services.AddIdentityServer(conf =>
            {
                //conf.Endpoints.EnableIntrospectionEndpoint = false;
                //conf.Endpoints.EnableUserInfoEndpoint = false;
            });
                // IN MEMORY
                //.AddInMemoryApiScopes(InMemory.ConfigInMemory.GetScopes())
                //.AddInMemoryClients(InMemory.ConfigInMemory.GetClients());
           

            return app.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app) 
        {
            app.UseIdentityServer();

            return app;
        }
    }
}
