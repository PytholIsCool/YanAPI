using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using YanAPI.Utils.Extensions;

namespace YanAPI.Logging; 
public static class CLogs {

    #region Native Console Setup

    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    #endregion

    #region Static Properties

    private static bool isInitialized = false;
    private static bool isEnabled = false;

    /// <summary>
    /// ASCII escape character (0x1B), used to detect ANSI escape sequences.
    /// </summary>
    public static readonly char ESC = (char)27;
    private static readonly Regex AnsiRegex = new($@"{ESC}\[[0-9;]*m", RegexOptions.Compiled);

#pragma warning disable IDE0044 // Shouldn't be readonly, as BepInEx might not be fully initialized when this is called thus, a lazy eval should be used instead
    private static DiskLogListener _fileLogger = Logger.Listeners.OfType<DiskLogListener>().FirstOrDefault();
#pragma warning restore IDE0044
    private static DiskLogListener FileLogger => _fileLogger ??= Logger.Listeners.OfType<DiskLogListener>().FirstOrDefault();

    private static readonly Stream StdOut = Console.OpenStandardOutput();
    private static readonly ManualLogSource LogSource = new("YanAPI");
    private static readonly object _logLock = new();

    #endregion

    #region Dynamic Properties

    #region Colors

    private static readonly ConcurrentDictionary<Assembly, LogColorSet> AssemblyColorOverrides = [];

    private class LogColorSet {
        public (int r, int g, int b)? None;
        public (int r, int g, int b)? Message;
        public (int r, int g, int b)? Info;
        public (int r, int g, int b)? Debug;
        public (int r, int g, int b)? Warning;
        public (int r, int g, int b)? Error;
        public (int r, int g, int b)? Fatal;
    }

    private static readonly (int r, int g, int b) YanNoneColor = (255, 255, 255);
    private static readonly (int r, int g, int b) YanMessageColor = (245, 160, 255);
    private static readonly (int r, int g, int b) YanInfoColor = (255, 130, 255);
    private static readonly (int r, int g, int b) YanDebugColor = (255, 255, 85);
    private static readonly (int r, int g, int b) YanWarningColor = (255, 170, 60);
    private static readonly (int r, int g, int b) YanErrorColor = (200, 10, 15);
    private static readonly (int r, int g, int b) YanFatalColor = (150, 0, 0);

    /// <summary>
    /// Sets custom ANSI RGB color overrides for the calling assembly's log levels.
    /// </summary>
    /// <param name="none">Color for LogLevel.None entries.</param>
    /// <param name="message">Color for LogLevel.Message entries.</param>
    /// <param name="info">Color for LogLevel.Info entries.</param>
    /// <param name="debug">Color for LogLevel.Debug entries.</param>
    /// <param name="warning">Color for LogLevel.Warning entries.</param>
    /// <param name="error">Color for LogLevel.Error entries.</param>
    /// <param name="fatal">Color for LogLevel.Fatal entries.</param>
    public static void SetColors(
    (int r, int g, int b)? none = null,
    (int r, int g, int b)? message = null,
    (int r, int g, int b)? info = null,
    (int r, int g, int b)? debug = null,
    (int r, int g, int b)? warning = null,
    (int r, int g, int b)? error = null,
    (int r, int g, int b)? fatal = null) {
        var assembly = Assembly.GetCallingAssembly();

        if (!AssemblyColorOverrides.TryGetValue(assembly, out var colors))
            colors = new LogColorSet();

        colors.None = none ?? colors.None;
        colors.Message = message ?? colors.Message;
        colors.Info = info ?? colors.Info;
        colors.Debug = debug ?? colors.Debug;
        colors.Warning = warning ?? colors.Warning;
        colors.Error = error ?? colors.Error;
        colors.Fatal = fatal ?? colors.Fatal;

        AssemblyColorOverrides[assembly] = colors;
    }

    /// <summary>
    /// Retrieves the RGB color tuple associated with a specific log level,
    /// using the calling assembly's override if set.
    /// </summary>
    /// <param name="level">The log level to retrieve a color for.</param>
    /// <returns>A tuple representing the RGB color.</returns>
    /// <exception cref="ArgumentException">Thrown if LogLevel.All is passed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the log level is unrecognized.</exception>
    public static (int r, int g, int b) GetColor(LogLevel level) {
        if (level == LogLevel.All)
            throw new ArgumentException("LogLevel.All cannot be used in this context.", nameof(level));

        if (AssemblyColorOverrides.TryGetValue(Assembly.GetCallingAssembly(), out var colors)) {
            return level switch {
                LogLevel.None => colors.None ?? YanNoneColor,
                LogLevel.Message => colors.Message ?? YanMessageColor,
                LogLevel.Info => colors.Info ?? YanInfoColor,
                LogLevel.Debug => colors.Debug ?? YanDebugColor,
                LogLevel.Warning => colors.Warning ?? YanWarningColor,
                LogLevel.Error => colors.Error ?? YanErrorColor,
                LogLevel.Fatal => colors.Fatal ?? YanFatalColor,
                _ => throw new ArgumentOutOfRangeException(nameof(level), $"Unhandled log level: {level}")
            };
        }
        return level switch {
            LogLevel.None => YanNoneColor,
            LogLevel.Message => YanMessageColor,
            LogLevel.Info => YanInfoColor,
            LogLevel.Debug => YanDebugColor,
            LogLevel.Warning => YanWarningColor,
            LogLevel.Error => YanErrorColor,
            LogLevel.Fatal => YanFatalColor,
            _ => throw new ArgumentOutOfRangeException(nameof(level), $"Unhandled log level: {level}")
        };
    }

    #endregion

    #region Other

    private static readonly ConcurrentDictionary<Assembly, bool> AssemblyDebugOverrides = [];

    /// <summary>
    /// Enables or disables debug-level logging for the calling assembly.
    /// </summary>
    /// <param name="enabled">True to enable debug logs; false to disable.</param>
    public static void SetDebug(bool enabled) => AssemblyDebugOverrides[Assembly.GetCallingAssembly()] = enabled;

    /// <summary>
    /// Checks whether debug-level logging is enabled for the calling assembly.
    /// </summary>
    /// <returns>True if debug logging is enabled; otherwise, false.</returns>
    public static bool IsDebugEnabled() => AssemblyDebugOverrides.TryGetValue(Assembly.GetCallingAssembly(), out var enabled) && enabled;

    #endregion

    #endregion

    #region Log Base

    private static void AssertNoAnsi(string message, string methodName) {
        if (AnsiRegex.IsMatch(message))
            throw new ArgumentException($"ANSI escape sequences are not allowed in '{methodName}'. Use Log() for custom styling.");
    }

    private static void DispatchLog(object data, Assembly caller, LogLevel logLevel = LogLevel.Message, string rawTag = null) {        
        if (rawTag == null)
            SafeLog(data?.ToString() ?? "", logLevel, caller.FullName);
        else
            SafeLog($"{rawTag} {data?.ToString() ?? ""}", logLevel, caller.FullName);
    }

    private static void SafeLog(string message, LogLevel logLevel, string assemblyName) {
        lock (_logLock) {
            if (isInitialized == false)
                Init();

            FileLogger?.LogEvent(assemblyName, new LogEventArgs(AnsiRegex.Replace(message, ""), logLevel, LogSource));

            if (isEnabled == false)
                return;

            var bytes = Encoding.UTF8.GetBytes(message + Environment.NewLine);
            StdOut.Write(bytes, 0, bytes.Length);
            StdOut.Flush();
        }
    }

    #endregion

    #region Usage

    /// <summary>
    /// Logs a raw message without applying ANSI styling or tagging.
    /// </summary>
    /// <param name="data">The object to log; will be converted to a string.</param>
    public static void Log(object data) => DispatchLog(data, Assembly.GetCallingAssembly());

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
        AssertNoAnsi(data?.ToString() ?? "", nameof(LogRGB));
        DispatchLog(data.ConvertToANSI(r, g, b), Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a message using a hex color code (e.g., "#FFAABB") with ANSI styling.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <param name="hexColor">A hex string representing the RGB color.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogHex(object data, string hexColor) {
        AssertNoAnsi(data?.ToString() ?? "", nameof(LogHex));
        DispatchLog(data.ConvertToANSI(hexColor), Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Logs a general-purpose message using the default message color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogMessage(object data) {
        AssertNoAnsi(data?.ToString() ?? "", nameof(LogMessage));
        var (r, g, b) = GetColor(LogLevel.Message);
        DispatchLog(data.ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Message);
    }

    /// <summary>
    /// Logs an informational message with the "[INFO]" tag and default info color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogInfo(object data) {
        AssertNoAnsi(data?.ToString() ?? "", nameof(LogInfo));
        var (r, g, b) = GetColor(LogLevel.Info);
        DispatchLog(data.ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Info, "[INFO]".ConvertToANSI(r, g, b));
    }

    /// <summary>
    /// Logs a debug message with the "[DEBUG]" tag, containing the source location and default debug color, if debug logging is enabled for the calling assembly.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogDebug(object data) => LogDebug_Internal(data);

    /// <summary>
    /// Logs a warning message with the "[WARNING]" tag, containing the source location and default warning color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogWarning(object data) {
        AssertNoAnsi(data?.ToString() ?? "", nameof(LogWarning));
        var (r, g, b) = GetColor(LogLevel.Warning);
        DispatchLog(data.ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Warning, "[WARNING]".ConvertToANSI(r, g, b));
    }

    /// <summary>
    /// Logs an error message with the "[ERROR]" tag, containing the source location and default error color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogError(object data) => LogError_Internal(data);

    /// <summary>
    /// Logs an exception's full message and stack trace as an error log.
    /// ANSI codes in the exception message are not allowed.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    /// <exception cref="ArgumentException">Thrown if the exception contains ANSI sequences.</exception>
    public static void LogError(Exception ex) => LogError_Internal(ex.ToString());

    /// <summary>
    /// Logs a fatal error message with the "[FATAL]" tag, containing the source location and default fatal color.
    /// ANSI codes in the input message are not allowed.
    /// </summary>
    /// <param name="data">The object to log.</param>
    /// <exception cref="ArgumentException">Thrown if the message contains ANSI sequences.</exception>
    public static void LogFatal(object data) => LogFatal_Internal(data);

    /// <summary>
    /// Logs an exception's full message and stack trace as a fatal error log.
    /// ANSI codes in the exception message are not allowed.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    /// /// <exception cref="ArgumentException">Thrown if the exception contains ANSI sequences.</exception>
    public static void LogFatal(Exception ex) => LogFatal_Internal(ex.ToString());

    private static void LogDebug_Internal(object data, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) {
        if (!IsDebugEnabled())
            return; // Skip debug logs if debug is not enabled for this assembly
        AssertNoAnsi(data?.ToString() ?? "", nameof(LogDebug_Internal));
        var (r, g, b) = GetColor(LogLevel.Debug);
        string info = $"[{Path.GetFileNameWithoutExtension(file)}.{member}:{line}]";
        DispatchLog($"{info} {data}".ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Debug, "[DEBUG]".ConvertToANSI(r, g, b));
    }

    private static void LogError_Internal(object data, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) {
        AssertNoAnsi(data?.ToString() ?? "", nameof(LogError));
        var (r, g, b) = GetColor(LogLevel.Error);
        string info = $"[{Path.GetFileNameWithoutExtension(file)}.{member}:{line}]";
        DispatchLog($"{info} {data}".ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Error, "[ERROR]".ConvertToANSI(r, g, b));
    }

    private static void LogFatal_Internal(object data, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) {
        AssertNoAnsi(data?.ToString() ?? "", nameof(LogFatal));
        var (r, g, b) = GetColor(LogLevel.Fatal);
        string info = $"[{Path.GetFileNameWithoutExtension(file)}.{member}:{line}]";
        DispatchLog($"{info} {data}".ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Fatal, "[FATAL]".ConvertToANSI(r, g, b));
    }

    #endregion

    #region Init

    private static void Init() {
        if (isInitialized == true) // Double checking incase 2 threads try to initialize at the same time
            return;

        lock (_logLock) {
            if (isInitialized == true)
                return;

            Logger.Sources.Add(LogSource);

            bool inCLSection = false;
            var configPath = Path.Combine(Paths.ConfigPath, "BepInEx.cfg");

            if (!File.Exists(configPath))
                throw new FileNotFoundException("BepInEx configuration file not found.", configPath);

            foreach (var line in File.ReadLines(configPath)) {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                    inCLSection = trimmed == "[Logging.Console]";

                if (inCLSection && (trimmed == "Enabled=true" || trimmed == "Enabled = true")) {
                    isEnabled = true;
                    break;
                }
            }

            if (isEnabled && (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.OSVersion.Version.Major >= 10))
                EnableANSI(); // Enable only on Windows 10+, required for ANSI escape code support

            isInitialized = true;
        }
    }

    private static void EnableANSI() {
        var handle = GetStdHandle(STD_OUTPUT_HANDLE);
        if (!GetConsoleMode(handle, out uint mode)) {
            Console.WriteLine("Failed to get console mode");
            return;
        }

        mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;

        if (!SetConsoleMode(handle, mode)) {
            Console.WriteLine("Failed to set console mode");
        }
    }

    #endregion

}
