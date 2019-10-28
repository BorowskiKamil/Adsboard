using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Adsboard.Common.Mvc;
using Adsboard.Common.MySql;
using Adsboard.Common.Redis;
using Adsboard.Common.Swagger;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.AutoMapper;
using Adsboard.Services.Identity.Data;
using Adsboard.Common.Dispatchers;
using Adsboard.Services.Identity.Infrastructure;
using Adsboard.Services.Identity.Services;

namespace Adsboard.Services.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMySql();
            services.AddMySqlContext<ApplicationContext>();
            
            services.AddWebApi();
            services.AddSwaggerDocs();
            services.AddRedis();
            services.AddJwt();
            services.AddRabbitMq();
            services.AddDispatchers();

            services.AddTransient<IPasswordHasher<Domain.Identity>, PasswordHasher<Domain.Identity>>();
            services.AddTransient<IClaimsProvider, ClaimsProvider>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();

            services.AddAutoMapperSetup();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "docker")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors();
            app.UseAllForwardedHeaders();
            app.UseSwaggerDocs();
            app.UseErrorHandler();
            app.UseAuthentication();
            app.UseAccessTokenValidator();
            app.UseMvc();
            app.UseRabbitMq();

            app.InitializeMigrations<ApplicationContext>();
        }
    }
}
