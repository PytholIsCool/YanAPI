using BepInEx.Logging;
using System;
using System.Reflection;
using YanAPI.Logging.Internal;
using YanAPI.Modules.Utils.Extensions;

namespace YanAPI.Logging; 
public static class CLogs {

    /// <summary>
    /// Logs a raw message without applying ANSI styling or tagging.
    /// </summary>
    /// <param name="data">The object to log; will be converted to a string.</param>
    public static void Log(object data) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.None))
            return;
        LogDispatcher.DispatchLog(data, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a message with the specified RGB color using ANSI escape codes.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <param name="r">Red component (0–255).</param>
    /// <param name="g">Green component (0–255).</param>
    /// <param name="b">Blue component (0–255).</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogRGB(object data, int r, int g, int b) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.None))
            return;
        LogDispatcher.AssertNoAnsi(data?.ToString() ?? "", nameof(LogRGB));
        LogDispatcher.DispatchLog(data.ConvertToANSI(r, g, b), Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a message using a hex color code (e.g., "#FFAABB") with ANSI styling.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <param name="hexColor">A hex string representing the RGB color.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogHex(object data, string hexColor) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.None))
            return;
        LogDispatcher.AssertNoAnsi(data?.ToString() ?? "", nameof(LogHex));
        LogDispatcher.DispatchLog(data.ConvertToANSI(hexColor), Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a general-purpose message using the default message color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogMessage(object data) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.Message))
            return;
        LogDispatcher.AssertNoAnsi(data?.ToString() ?? "", nameof(LogMessage));
        var (r, g, b) = LogColorManager.GetColor(LogLevel.Message);
        LogDispatcher.DispatchLog(data.ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Message);
    }

    /// <summary>
    /// Logs an informational message with the "[INFO]" tag and default info color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogInfo(object data) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.Info))
            return;
        LogDispatcher.AssertNoAnsi(data?.ToString() ?? "", nameof(LogInfo));
        var (r, g, b) = LogColorManager.GetColor(LogLevel.Info);
        LogDispatcher.DispatchLog(data.ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Info, "[INFO]".ConvertToANSI(r, g, b));
    }

    /// <summary>
    /// Logs a debug message with the "[DEBUG]" tag, containing the source location and default debug color, if debug logging is enabled for the calling assembly.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogDebug(object data) => LogFormatter.LogDebug_Internal(data);

    /// <summary>
    /// Logs a warning message with the "[WARNING]" tag, containing the source location and default warning color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogWarning(object data) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.Warning))
            return;
        LogDispatcher.AssertNoAnsi(data?.ToString() ?? "", nameof(LogWarning));
        var (r, g, b) = LogColorManager.GetColor(LogLevel.Warning);
        LogDispatcher.DispatchLog(data.ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Warning, "[WARNING]".ConvertToANSI(r, g, b));
    }

    /// <summary>
    /// Logs an error message with the "[ERROR]" tag, containing the source location and default error color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogError(object data) => LogFormatter.LogError_Internal(data);

    /// <summary>
    /// Logs an exception's full message and stack trace as an error log.
    /// ANSI codes in the exception message are not allowed.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    /// <exception cref="ArgumentException">Thrown if the exception contains ANSI sequences.</exception>
    public static void LogError(Exception ex) => LogFormatter.LogError_Internal(ex.ToString());

    /// <summary>
    /// Logs a fatal error message with the "[FATAL]" tag, containing the source location and default fatal color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogFatal(object data) => LogFormatter.LogFatal_Internal(data);

    /// <summary>
    /// Logs an exception's full message and stack trace as a fatal error log.
    /// ANSI codes in the exception message are not allowed.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    /// /// <exception cref="ArgumentException">Thrown if the exception contains ANSI sequences.</exception>
    public static void LogFatal(Exception ex) => LogFormatter.LogFatal_Internal(ex.ToString());

}
