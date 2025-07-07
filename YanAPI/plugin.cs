using BepInEx;
using UnityEngine;
using YanAPI;
using YanAPI.Core;
using YanAPI.Logging;

[BepInPlugin("com.Pythol.YanAPI", "YanAPI", "1.0.0")]
public class YanAPIEntry : BaseUnityPlugin {
    public static YanAPIEntry Instance { get; private set; }
    private void Awake() {
        CLogs.LogInfo("YanAPI loaded. Checking for dupes...");

        if (Instance != null) {
            CLogs.LogError("An instance of YanAPI already exists! This should not happen. The most likely cause is that there's multiple YanAPI plugins in your plugins folder.");
            Destroy(this);
            return;
        }

        Instance = this;

        CLogs.LogInfo("No dupes found! Initializing YanAPI.Core...");
        GameObject core = new("YanAPICore");
        DontDestroyOnLoad(core);
        core.AddComponent<YanAPICore>();

        APIExamples.Load();
    }
    
    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
            Logger.LogInfo("YanAPI has been destroyed successfully!");
        }
    }
}
