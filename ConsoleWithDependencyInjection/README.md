# 

1. Add reference packages

    dotnet add package Microsoft.Extensions.DependencyInjection
    dotnet add package Microsoft.Extensions.Logging
    dotnet add package Microsoft.Extensions.Logging.Console

2. Add log reference in constructor

```c#
public class DemoClass
{
    private readonly ILogger<DemoClass> _logger;

    public DemoClass(ILogger<DemoClass> logger)
    {
        _logger = logger;
    }

    public void TestLog(string log)
    {
        this._logger.LogInformation(log);
    }
}
```

3. Build services collection

```c#
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var demoClass = serviceProvider.GetService<DemoClass>();
            demoClass.TestLog("Hello");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<DemoClass>();

            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);
        }
```

4. Control logger level

You can set logger level by

```c#
services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);
```

or get level from configuration, see lastest file:

    Program.cs

# Reference

* [net-core-console-logging](https://www.blinkingcaret.com/2018/02/14/net-core-console-logging/)
