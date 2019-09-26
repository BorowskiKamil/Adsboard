using System;
using System.Linq;
using System.Reflection;
using Adsboard.Common.Handlers;
using Adsboard.Common.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Adsboard.Services.Operations.Handlers
{
    internal static class Extensions
    {
        public static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            Type[] events = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IEvent).IsAssignableFrom(t))
                .ToArray();

            Type[] commands = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t))
                .ToArray();

            foreach (var e in events)
                services.AddTransient(typeof(IEventHandler<>).MakeGenericType(e), typeof(GenericEventHandler<>).MakeGenericType(e));

            foreach (var command in commands)
                services.AddTransient(typeof(ICommandHandler<>).MakeGenericType(command), typeof(GenericCommandHandler<>).MakeGenericType(command));

            return services;
        }
    }
}