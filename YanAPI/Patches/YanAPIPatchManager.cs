using YanAPI.Logging;

namespace YanAPI.Patches; 
internal static class YanAPIPatchManager {
    public static void PatchAll() {
        NewTitleScreenScriptPatches.Apply();
        NewSettingsScriptPatches.Apply();
        CLogs.LogInfo("All patches applied successfully.");
    }
}
