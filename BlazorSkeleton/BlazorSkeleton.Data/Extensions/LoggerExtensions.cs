using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace BlazorSkeleton.Data.Extensions
{
    /// <summary>Method entry/exit tracing. Logged at Trace level, so it is silent unless that level is enabled.</summary>
    public static class LoggerExtensions
    {
        public static void LogEntry(this ILogger logger, [CallerMemberName] string methodName = "")
            => logger.LogTrace("Entering {Method}", methodName);

        public static void LogExit(this ILogger logger, [CallerMemberName] string methodName = "")
            => logger.LogTrace("Exiting {Method}", methodName);
    }
}
