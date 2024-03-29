using Adsboard.Services.Adverts.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Adsboard.Services.Adverts.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opt) 
            : base (opt) { }
        
        public ApplicationContext() { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdvertMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}