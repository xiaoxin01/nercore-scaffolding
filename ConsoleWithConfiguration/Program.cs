using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ConsoleWithConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            var environmentName = null == Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ? "Development" : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine($"Environment: {environmentName}");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .Build();

            var theKey = config.GetSection("TheKey").Value;

            Console.WriteLine($"TheKey: {theKey}");
        }
    }
}
