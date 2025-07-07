using BepInEx.Logging;
using BepInEx;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace YanAPI.Logging.Internal; 
internal static class LogInitializer {
    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    internal static void Init() {
        if (LogProperties.isInitialized == true) // Double checking in case 2 threads try to initialize at the same time
            return;

        lock (LogProperties._logLock) {
            if (LogProperties.isInitialized == true)
                return;

            Logger.Sources.Add(LogProperties.LogSource);

            bool inCLSection = false;
            var configPath = Path.Combine(Paths.ConfigPath, "BepInEx.cfg");

            if (!File.Exists(configPath))
                throw new FileNotFoundException("BepInEx configuration file not found.", configPath);

            foreach (var line in File.ReadLines(configPath)) {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                    inCLSection = trimmed == "[Logging.Console]";

                if (inCLSection && (trimmed == "Enabled=true" || trimmed == "Enabled = true")) {
                    LogProperties.isEnabled = true;
                    break;
                }
            }

            if (LogProperties.isEnabled && (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.OSVersion.Version.Major >= 10))
                EnableANSI(); // Enable only on Windows 10+, required for ANSI escape code support

            LogProperties.isInitialized = true;
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
}