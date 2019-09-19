using Microsoft.EntityFrameworkCore;
using Adsboard.Identity.Data.Mappings;

namespace Adsboard.Identity.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opt) 
            : base (opt) { }
        
        public ApplicationContext() { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IdentityMap());
            modelBuilder.ApplyConfiguration(new RefreshTokenMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}