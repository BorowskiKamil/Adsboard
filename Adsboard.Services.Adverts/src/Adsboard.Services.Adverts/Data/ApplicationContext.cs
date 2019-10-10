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

            base.OnModelCreating(modelBuilder);
        }
    }
}