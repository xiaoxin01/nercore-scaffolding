using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConsoleWithDependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            // set configuration
            var environmentName = null == Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ? "Development" : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine($"Environment: {environmentName}");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, config);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var demoClass = serviceProvider.GetService<DemoClass>();
            demoClass.LogTrace("LogTrace");
            demoClass.LogDebug("LogDebug");
            demoClass.LogInformation("LogInformation");
            demoClass.LogWarning("LogWarning");
            demoClass.LogError("LogError");
            demoClass.LogCritical("LogCritical");
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddLogging(configure =>
            {
                configure.AddConfiguration(config.GetSection("Logging"));
                configure.AddConsole();
            })
                .AddTransient<DemoClass>();

            //services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);
        }
    }
}
