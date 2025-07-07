using BepInEx.Logging;

namespace YanAPI.Logging.Internal; 
internal static class LogSettings {
    /// <summary>
    /// Checks if the specified log level is enabled for console output.
    /// </summary>
    /// <param name="level">The log level being checked.</param>
    /// <returns>True if enabled. False if not.</returns>
    public static bool IsConsoleLogLevelEnabled(LogLevel level) => LogLevelParser.IsConsoleLogLevelEnabled_Internal(level);
    /// <summary>
    /// Checks if the specified log level is enabled for disk output.
    /// </summary>
    /// <param name="level">The log level being checked.</param>
    /// <returns>True if enabled. False if not.</returns>
    public static bool IsDiskLogLevelEnabled(LogLevel level) => LogLevelParser.IsDiskLogLevelEnabled_Internal(level);
}
