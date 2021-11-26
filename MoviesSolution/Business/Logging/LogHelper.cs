using Serilog;
using System;

namespace Business.Logging
{
    public static class LogHelper
    {
        private static readonly ILogger _errorLogger = SeriLoggers.ErrorLogger;
        private static readonly ILogger _infoLogger = SeriLoggers.InfoLogger;

        public static void LogExceptionMessage(Exception exception)
        {
            _errorLogger.Error(exception.Message);
        }
        
        public static void LogExceptionStackTrace(Exception exception)
        {
            _errorLogger.Error(exception.StackTrace);
        }

        public static void LogInfo(string messageFormat)
        {
            _infoLogger.Information(messageFormat);
        }

        public static void LogConfigError(string methodName, string configName)
        {
            _infoLogger.Information($"{methodName} | Bad configuration: '{configName}' not found.");
        }
    }
}
