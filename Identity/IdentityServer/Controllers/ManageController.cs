using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class ManageController : Controller
    {
        private ConfigurationDbContext _conf;

        public ManageController(ConfigurationDbContext conf)
        {
            _conf = conf;
        }

        public async Task<IActionResult> AddIdentityScope()
        {
            var oidc = new IdentityResources.OpenId();
            var profil = new IdentityResources.Profile();
            _conf.IdentityResources.Add(oidc.ToEntity());
            _conf.IdentityResources.Add(profil.ToEntity());
            await _conf.SaveChangesAsync();
            return Ok();
        }

            public async Task<IActionResult> AddApiScope()
        {
            ApiScope scope = new ApiScope();
            scope.Name = "api_demo_scope";
            scope.Enabled= true;
            _conf.ApiScopes.Add(scope.ToEntity());
            await _conf.SaveChangesAsync();
            return Ok();

        }


        public async Task<IActionResult> AddClient()
        {
            /*Client client = new Client();
            client.ClientId = "client_console";
            client.ClientName = "Client en console";
            client.Enabled = true;
            client.RequirePkce = false; // pas d'auth user
            client.AllowedGrantTypes = GrantTypes.ClientCredentials;
            client.ClientSecrets = new List<Secret> { new Secret("secret_console".Sha256()) };
            //client.AllowedScopes = new List<string> { "api_demo_scope" };*/

            Client client = new Client();
            client.ClientId = "client_mvc";
            client.ClientName = "Client en web app";
            client.Enabled = true;
            client.RequirePkce = true;
            client.AllowedGrantTypes = GrantTypes.Code;
            client.ClientSecrets = new List<Secret> { new Secret("secret_mvc".Sha256()) };
            client.AllowedScopes = new List<string> {"openid", "api_demo_scope" };
            client.AllowOfflineAccess= true;

            _conf.Clients.Add(client.ToEntity());
            await _conf.SaveChangesAsync();

            return Ok();
        }


        public async Task<IActionResult> AddApi()
        {
            ApiResource api = new ApiResource();
            api.Name = "api_demo";
            api.Enabled = true;

            _conf.ApiResources.Add(api.ToEntity());
            await _conf.SaveChangesAsync();

            return Ok();
        }
    }
}
