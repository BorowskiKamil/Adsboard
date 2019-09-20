using Adsboard.Common.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Adsboard.Common.Dispatchers
{
    public static class Extensions
    {
        public static void AddDispatchers(this IServiceCollection services)
        {
            services.AddTransient<ICommandDispatcher, CommandDispatcher>();
            services.AddTransient<IDispatcher, Dispatcher>();
            services.AddTransient<IQueryDispatcher, QueryDispatcher>();

            services.Scan(scan => scan
                .FromEntryAssembly()
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }
    }
}
