using Serilog;
using System.IO;

namespace Business.Logging
{
    public static class SeriLoggers
    {
        private enum FileSize
        {
            _128MB = 134217728,
            _256MB = 268435456,
            _512MB = 536870912,
            _1GB = 1073741824,
        }

        public static ILogger InfoLogger { get; } = CreateInfoLogger();
        public static ILogger ErrorLogger { get; } = CreateErrorLogger();

        private static ILogger CreateInfoLogger()
        {
            string filePath = Path.Combine("log", "information", ".txt");

            return new LoggerConfiguration().WriteTo.Async(a =>
            {
                a.File(filePath, fileSizeLimitBytes: (int)FileSize._256MB, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: 100);
            }).CreateLogger();
        }

        private static ILogger CreateErrorLogger()
        {
            string filePath = Path.Combine("log", "error", ".txt");

            return new LoggerConfiguration().WriteTo.Async(a =>
            {
                a.File(filePath, fileSizeLimitBytes: (int)FileSize._256MB, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: 100);
            }).CreateLogger();
        }
    }
}