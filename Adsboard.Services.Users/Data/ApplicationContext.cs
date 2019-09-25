using Adsboard.Services.Users.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Adsboard.Services.Users.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opt)
            : base(opt) { }

        public ApplicationContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}