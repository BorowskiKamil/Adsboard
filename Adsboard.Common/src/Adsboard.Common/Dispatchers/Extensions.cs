using System;
using System.Reflection;
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

            var callingAssembly = Assembly.GetCallingAssembly().GetName().Name.ToString();

            services.Scan(scan => scan
                .FromAssemblies(Assembly.Load(callingAssembly))
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }
    }
}
