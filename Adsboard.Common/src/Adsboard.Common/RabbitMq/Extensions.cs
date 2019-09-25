using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit;
using RawRabbit.Instantiation;
using RawRabbit.Configuration;
using RawRabbit.Common;
using RawRabbit.Enrichers.MessageContext;
using Microsoft.AspNetCore.Builder;
using Adsboard.Common.Handlers;

namespace Adsboard.Common.RabbitMq
{
    public static class Extensions
    {

        public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
            => new BusSubscriber(app);

        public static void AddRabbitMq(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            RabbitMqOptions rabbitmqOptions;
            RawRabbitConfiguration rawRabbitConfiguration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<RabbitMqOptions>(configuration.GetSection("rabbitmq"));
                services.Configure<RawRabbitConfiguration>(configuration.GetSection("rabbitmq"));
                rawRabbitConfiguration = configuration.GetOptions<RawRabbitConfiguration>("rabbitmq");
                rabbitmqOptions = configuration.GetOptions<RabbitMqOptions>("rabbitmq");
            }

            var namingConventions = new CustomNamingConventions(rabbitmqOptions.Namespace);

            services.Scan(scan => scan
                .FromEntryAssembly()
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(classes => classes
                    .AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );
            
            services.AddTransient<IHandler, Handler>();
            services.AddTransient<IBusPublisher, BusPublisher>();

            services.AddRawRabbit(new RawRabbitOptions {
                ClientConfiguration = rawRabbitConfiguration,
                DependencyInjection = ioc => ioc
                    .AddSingleton<INamingConventions>(namingConventions),
                Plugins = p => p
                    .UseAttributeRouting()
                    .UseRetryLater()
                    .UpdateRetryInfo()
                    .UseMessageContext<CorrelationContext>()
                    .UseContextForwarding()
            });
        }

        private static IClientBuilder UpdateRetryInfo(this IClientBuilder clientBuilder)
        {
            clientBuilder.Register(c => c.Use<RetryStagedMiddleware>());

            return clientBuilder;
        }
    }
}