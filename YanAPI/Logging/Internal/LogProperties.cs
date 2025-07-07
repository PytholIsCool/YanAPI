using BepInEx.Logging;

namespace YanAPI.Logging.Internal {
    internal static class LogProperties {
        internal static bool isInitialized = false;
        internal static bool isEnabled = false;

        internal static readonly ManualLogSource LogSource = new("YanAPI");

        internal static readonly object _logLock = new();
    }
}
