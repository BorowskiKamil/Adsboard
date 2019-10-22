using System.IO;
using Adsboard.Services.Categories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Adsboard.Services.Categories.IntegrationTests.Config
{
    public static class AppConfig
    {
        public static WebApplicationFactory<Startup> AddConfiguration(WebApplicationFactory<Startup> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            var _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context,conf) =>
                {
                    conf.AddJsonFile(configPath);
                });
            });
            return _factory;
        } 
    }
}