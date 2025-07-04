using YanAPI.Logging;

namespace YanAPI.Patches; 
internal static class YanAPIPatchManager {
    internal static void PatchAll() {
        NewTitleScreenScriptPatches.Apply();
        NewSettingsScriptPatches.Apply();
        InputManagerScriptPatches.Apply();
        CLogs.LogInfo("All patches applied successfully.");
    }
}
