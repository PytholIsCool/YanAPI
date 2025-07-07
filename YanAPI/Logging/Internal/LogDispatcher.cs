using BepInEx.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace YanAPI.Logging.Internal; 
internal static class LogDispatcher {

    internal static readonly char ESC = (char)27;
    private static readonly Regex AnsiRegex = new($@"{ESC}\[[0-9;]*m", RegexOptions.Compiled);

    private static DiskLogListener _fileLogger = Logger.Listeners.OfType<DiskLogListener>().FirstOrDefault();
    private static DiskLogListener FileLogger => _fileLogger ??= Logger.Listeners.OfType<DiskLogListener>().FirstOrDefault();
    private static readonly Stream StdOut = Console.OpenStandardOutput();

    internal static void AssertNoAnsi(string message, string methodName) {
        if (AnsiRegex.IsMatch(message))
            throw new ArgumentException($"ANSI escape sequences are not allowed in '{methodName}'. Use Log() for custom styling.");
    }

    internal static void DispatchLog(object data, Assembly caller, LogLevel logLevel = LogLevel.Message, string rawTag = null) {
        if (rawTag == null)
            SafeLog(data?.ToString() ?? "", logLevel, caller.FullName);
        else
            SafeLog($"{rawTag} {data?.ToString() ?? ""}", logLevel, caller.FullName);
    }

    private static void SafeLog(string message, LogLevel logLevel, string assemblyName) {
        lock (LogProperties._logLock) {
            if (LogProperties.isInitialized == false)
                LogInitializer.Init();

            if (LogSettings.IsDiskLogLevelEnabled(logLevel))
                FileLogger?.LogEvent(assemblyName, new LogEventArgs(AnsiRegex.Replace(message, ""), logLevel, LogProperties.LogSource));

            if (LogProperties.isEnabled == false)
                return;

            var bytes = Encoding.UTF8.GetBytes(message + Environment.NewLine);
            StdOut.Write(bytes, 0, bytes.Length);
            StdOut.Flush();
        }
    }
}