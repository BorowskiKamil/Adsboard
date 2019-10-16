using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Adsboard.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace Adsboard.Api.Authentication
{
    public static class Extensions
    {
        private static readonly string SectionName = "jwt";

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }
            
            var section = configuration.GetSection(SectionName);
            var options = configuration.GetOptions<AuthenticationOptions>(SectionName);

            services.Configure<AuthenticationOptions>(section);
            services.AddSingleton(options);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SecretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidateAudience = options.ValidateAudience,
                ValidAudience = options.ValidAudience,
                ValidateLifetime = options.ValidateLifetime,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication()
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}