using Duende.IdentityServer.Models;

namespace IdentityServer.InMemory
{
    public static class ConfigInMemory
    {
        public static IEnumerable<ApiScope> GetScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api_demo", "API de demo"),
                new ApiScope("api_demo2", "API de demo 2")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client {
                    ClientId= "client_console",
                    AllowedGrantTypes = { GrantType.ClientCredentials },
                    ClientSecrets =
                    {
                        new Secret ("secret_console".Sha256())
                    },
                    AllowedScopes = { "api_demo", "api_demo2" }
                }
            };
        }
    }
}
