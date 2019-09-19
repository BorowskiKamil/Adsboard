using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adsboard.Common.MySql
{
    public static class Extensions
    {
        public static void AddMySql(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<MySqlOptions>(configuration.GetSection("mysql"));
            }
        }

        public static void AddMySqlContext<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            MySqlOptions options = null;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                options = configuration.GetOptions<MySqlOptions>("mysql");
            }

            services.AddDbContext<TContext>(o => {
                o.UseMySql(options.ConnectionString);
            });
        }

        public static void InitializeMigrations<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<TContext>().Database.Migrate();
            }
        }
    }
}