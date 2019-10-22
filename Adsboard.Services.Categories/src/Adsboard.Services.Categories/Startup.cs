using Adsboard.Common.Dispatchers;
using Adsboard.Common.Mvc;
using Adsboard.Common.MySql;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Redis;
using Adsboard.Common.Swagger;
using Adsboard.Common.AutoMapper;
using Adsboard.Services.Categories.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Adsboard.Services.Categories.Mappings;

namespace Adsboard.Services.Categories
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
            services.AddRabbitMq();
            services.AddDispatchers();

            services.AddAutoMapperSetup(typeof(DomainToDtoMappingProfile));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
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
            app.UseMvc();
            app.UseRabbitMq()
                .SubscribeCommand<CreateCategory>()
                .SubscribeCommand<UpdateCategory>()
                .SubscribeCommand<RemoveCategory>();

            app.InitializeMigrations<ApplicationContext>();

        }
    }
}
