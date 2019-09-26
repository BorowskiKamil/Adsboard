using Adsboard.Common.Dispatchers;
using Adsboard.Common.Mvc;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Redis;
using Adsboard.Common.Swagger;
using Adsboard.Services.Operations.Handlers;
using Adsboard.Services.Operations.Infrastructure;
using Adsboard.Services.Operations.Services;
using Chronicle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adsboard.Services.Operations
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
            services.AddWebApi();
            services.AddSwaggerDocs();
            services.AddRedis();
            services.AddChronicle();

            services.AddRabbitMq();
            services.AddDispatchers();

            services.RegisterHandlers();

            services.AddScoped<IOperationsStorage, OperationsStorage>();
            services.AddScoped<IOperationPublisher, OperationPublisher>();
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

            app.UseMvc();

            app.UseSwaggerDocs();
            app.UseErrorHandler();
            app.UseMvc();
            app.UseRabbitMq()
                .SubscribeAllMessages();
        }
    }
}
