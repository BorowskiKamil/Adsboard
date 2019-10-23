using Adsboard.Api.Authentication;
using Adsboard.Api.Services.RestEase;
using Adsboard.Common.Dispatchers;
using Adsboard.Common.Mvc;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Swagger;
using Adsboard.Common.Validation;
using Adsboard.Common.RestEase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adsboard.Api
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
            services.AddWebApi().AddValidation();
            services.AddSwaggerDocs();
            services.ConfigureAuthentication();

            services.RegisterServiceForwarder<IAdvertsService>("adverts-service");
            services.RegisterServiceForwarder<IOperationsService>("operations-service");
            services.RegisterServiceForwarder<IUsersService>("users-service");
            services.RegisterServiceForwarder<ICategoriesService>("categories-service");

            services.AddRabbitMq();
            services.AddDispatchers();            
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
            app.UseMvc();
            app.UseRabbitMq();
        }
    }
}