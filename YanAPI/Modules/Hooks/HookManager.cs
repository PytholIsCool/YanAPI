using YanAPI.Logging;
using YanAPI.Modules.Hooks.Patches;

namespace YanAPI.Modules.Hooks;
internal static class HookManager {
    internal static void PatchAll() {
        CLogs.LogInfo("Applying YanAPI patches...");

        InputPatch.Apply();
        // NewSettingsScriptPatches.Apply();
        MMPatch.Apply();
        CLogs.LogInfo("All patches applied successfully.");
    }
}
