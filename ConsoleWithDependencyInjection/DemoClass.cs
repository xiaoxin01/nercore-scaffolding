using Microsoft.Extensions.Logging;

namespace ConsoleWithDependencyInjection
{
    public class DemoClass
    {
        private readonly ILogger<DemoClass> _logger;

        public DemoClass(ILogger<DemoClass> logger)
        {
            _logger = logger;
        }

        public void LogTrace(string log)
        {
            this._logger.LogTrace(log);
        }
        public void LogDebug(string log)
        {
            this._logger.LogDebug(log);
        }
        public void LogInformation(string log)
        {
            this._logger.LogInformation(log);
        }
        public void LogWarning(string log)
        {
            this._logger.LogWarning(log);
        }
        public void LogError(string log)
        {
            this._logger.LogError(log);
        }
        public void LogCritical(string log)
        {
            this._logger.LogCritical(log);
        }
    }
}
