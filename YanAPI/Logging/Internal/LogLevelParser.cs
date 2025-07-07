using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;

#nullable enable
namespace YanAPI.Logging.Internal;

internal static class LogLevelParser {
    private static bool isConfigParsed = false;

    private static readonly Dictionary<LogLevel, (bool Console, bool Disk)> logLevelStatus = [];

    internal static bool IsConsoleLogLevelEnabled_Internal(LogLevel level) {
        if (!isConfigParsed)
            ParseConfig();

        return logLevelStatus.TryGetValue(level, out var status) && status.Console;
    }

    internal static bool IsDiskLogLevelEnabled_Internal(LogLevel level) {
        if (!isConfigParsed)
            ParseConfig();

        return logLevelStatus.TryGetValue(level, out var status) && status.Disk;
    }

    private static void ParseConfig() {
        string configPath = Path.Combine(Paths.ConfigPath, "BepInEx.cfg");
        if (!File.Exists(configPath))
            throw new FileNotFoundException("BepInEx configuration file not found.", configPath);

        LogLevel[] allLevels = (LogLevel[])Enum.GetValues(typeof(LogLevel));
        string? currentSection = null;

        foreach (var line in File.ReadLines(configPath)) {
            var trimmed = line.Trim();

            if (trimmed.StartsWith("[") && trimmed.EndsWith("]")) {
                currentSection = trimmed;
                continue;
            }

            if (!trimmed.StartsWith("LogLevels", StringComparison.OrdinalIgnoreCase) || currentSection == null)
                continue;

            bool isConsole = currentSection.Equals("[Logging.Console]", StringComparison.OrdinalIgnoreCase);
            bool isDisk = currentSection.Equals("[Logging.Disk]", StringComparison.OrdinalIgnoreCase);
            if (!isConsole && !isDisk)
                continue;

            string rawLevels = trimmed.Substring("LogLevels".Length).Trim().TrimStart('=').Trim();
            string[] splitLevels = rawLevels.Split(',');

            HashSet<string> levelsSet = new(splitLevels, StringComparer.OrdinalIgnoreCase);

            if (levelsSet.Contains("All")) {
                foreach (var level in allLevels) {
                    if (isConsole)
                        SetLevel(level, console: true);
                    if (isDisk)
                        SetLevel(level, disk: true);
                }
                break;
            }

            if (levelsSet.Contains("None")) {
                foreach (var level in allLevels) {
                    SetLevel(level, console: false, disk: false);
                }
                if (isConsole)
                    SetLevel(LogLevel.None, console: true);
                if (isDisk)
                    SetLevel(LogLevel.None, disk: true);
                break;
            }

            foreach (var level in allLevels) {
                if (levelsSet.Contains(level.ToString())) {
                    if (isConsole)
                        SetLevel(level, console: true);
                    if (isDisk)
                        SetLevel(level, disk: true);
                }
            }
        }

        isConfigParsed = true;
    }

    private static void SetLevel(LogLevel level, bool? console = null, bool? disk = null) {
        if (!logLevelStatus.TryGetValue(level, out var current))
            current = (false, false);

        logLevelStatus[level] = (
            console ?? current.Console,
            disk ?? current.Disk
        );
    }
}