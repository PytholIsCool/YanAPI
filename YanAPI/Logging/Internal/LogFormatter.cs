using BepInEx.Logging;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using YanAPI.Modules.Utils.Extensions;

namespace YanAPI.Logging.Internal; 
internal static class LogFormatter {
    internal static void LogDebug_Internal(object data, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.Debug))
            return;
        LogDispatcher.AssertNoAnsi(data?.ToString() ?? "", nameof(CLogs.LogDebug));
        var (r, g, b) = LogColorManager.GetColor(LogLevel.Debug);
        string info = $"[{Path.GetFileNameWithoutExtension(file)}.{member}:{line}]";
        LogDispatcher.DispatchLog($"{info} {data}".ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Debug, "[DEBUG]".ConvertToANSI(r, g, b));
    }

    internal static void LogError_Internal(object data, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.Error))
            return;
        LogDispatcher.AssertNoAnsi(data?.ToString() ?? "", nameof(CLogs.LogError));
        var (r, g, b) = LogColorManager.GetColor(LogLevel.Error);
        string info = $"[{Path.GetFileNameWithoutExtension(file)}.{member}:{line}]";
        LogDispatcher.DispatchLog($"{info} {data}".ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Error, "[ERROR]".ConvertToANSI(r, g, b));
    }

    internal static void LogFatal_Internal(object data, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) {
        if (!LogSettings.IsConsoleLogLevelEnabled(LogLevel.Fatal))
            return;
        LogDispatcher.AssertNoAnsi(data?.ToString() ?? "", nameof(CLogs.LogFatal));
        var (r, g, b) = LogColorManager.GetColor(LogLevel.Fatal);
        string info = $"[{Path.GetFileNameWithoutExtension(file)}.{member}:{line}]";
        LogDispatcher.DispatchLog($"{info} {data}".ConvertToANSI(r, g, b), Assembly.GetCallingAssembly(), LogLevel.Fatal, "[FATAL]".ConvertToANSI(r, g, b));
    }
}