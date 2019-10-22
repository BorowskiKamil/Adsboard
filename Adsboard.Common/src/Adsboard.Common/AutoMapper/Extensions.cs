using System;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Adsboard.Common.AutoMapper
{
    public static class Extensions
    {
        public static void AddAutoMapperSetup(this IServiceCollection services, params Type[] profiles)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddAutoMapper(Assembly.GetCallingAssembly());
			RegisterMappings();
		}

		public static MapperConfiguration RegisterMappings(params Profile[] profiles)
		{
			return new MapperConfiguration(cfg =>
            {
				cfg.AddProfiles(profiles);
            });
		}
    }
}