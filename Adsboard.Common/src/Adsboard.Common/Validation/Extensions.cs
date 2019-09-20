using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Adsboard.Common.Validation
{
    public static class Extensions
    {
        public static IMvcBuilder AddValidation(this IMvcBuilder builder)
        {
            builder.AddFluentValidation();

            var validatorTypes = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => typeof(IValidator).IsAssignableFrom(t))
                .ToArray();

            foreach (var validatorType in validatorTypes)
            {
                var genericType = validatorType.BaseType?.GenericTypeArguments;
                if (genericType == null) continue;

                builder.Services.AddTransient(typeof(IValidator<>).MakeGenericType(genericType), validatorType);
            }

            return builder;
        }
    } 
}