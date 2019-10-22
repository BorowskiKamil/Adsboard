using Adsboard.Services.Categories.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Adsboard.Services.Categories.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opt) 
            : base (opt) { }
        
        public ApplicationContext() { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}