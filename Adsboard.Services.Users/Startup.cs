using Adsboard.Common.Dispatchers;
using Adsboard.Common.Mvc;
using Adsboard.Common.MySql;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Swagger;
using Adsboard.Common.AutoMapper;
using Adsboard.Services.Users.Data;
using Adsboard.Services.Users.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Adsboard.Services.Users.Messages.Events;

namespace Adsboard.Services.Users
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
            services.AddDispatchers();
            services.AddSwaggerDocs();
            services.AddRabbitMq();
            services.AddAutoMapperSetup(typeof(DomainToDtoMappingProfile));
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
            app.UseErrorHandler();
            app.UseSwaggerDocs();
            app.UseMvc();
            app.UseRabbitMq()
                .SubscribeEvent<IdentityCreated>();

            app.InitializeMigrations<ApplicationContext>();
        }
    }
}
