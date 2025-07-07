using BepInEx.Logging;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace YanAPI.Logging.Internal; 
internal static class LogColorManager {
    private static readonly ConcurrentDictionary<Assembly, LogColorSet> AssemblyColorOverrides = [];

    private struct LogColorSet {
        public (int r, int g, int b)? None, Message, Info, Debug, Warning, Error, Fatal;
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
    (int r, int g, int b)? none = null, (int r, int g, int b)? message = null, (int r, int g, int b)? info = null, (int r, int g, int b)? debug = null, (int r, int g, int b)? warning = null, (int r, int g, int b)? error = null, (int r, int g, int b)? fatal = null) {
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

    /// <summary>
    /// Resets the color overrides for the calling assembly's log levels to the default colors set by YanAPI.
    /// </summary>
    public static void ResetColors() => AssemblyColorOverrides.TryRemove(Assembly.GetCallingAssembly(), out _);
}
