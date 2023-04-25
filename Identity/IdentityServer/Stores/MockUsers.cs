using Duende.IdentityServer.Test;
using IdentityModel;

namespace IdentityServer.Stores
{
    public class MockUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "bob@gmail.com",
                        Password = "bob",
                        Claims =
                        {
                            new System.Security.Claims.Claim(JwtClaimTypes.Name, "Bob"),
                            new System.Security.Claims.Claim(JwtClaimTypes.GivenName, "Bob"),
                            new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, "Bob"),
                            new System.Security.Claims.Claim(JwtClaimTypes.Email, "bob@gmail.com")
                        }
                    }
                };
            }
        }
    }
}
