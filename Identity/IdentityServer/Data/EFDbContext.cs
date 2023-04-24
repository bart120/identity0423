using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class EFDbContext : DbContext
    {
        protected readonly IConfiguration configuration;

        public EFDbContext(IConfiguration configuration)
        {
            this.configuration= configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("IdentityConnection"));
        }
    }
}
