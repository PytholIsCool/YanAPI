using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using YanAPI;
using YanAPI.Logging;
using YanAPI.Patches;
using YanAPI.UI.MainMenu;
using YanAPI.Utils;
using YanAPI.Wrappers;

[BepInPlugin("com.Pythol.YanAPI", "YanAPI", "1.0.0")]
public class YanAPICore : BaseUnityPlugin {
    public static YanAPICore Instance { get; private set; }
    private void Awake() {
        CLogs.LogInfo("YanAPI loaded. Checking for dupes...");

        if (Instance != null) {
            CLogs.LogError("An instance of YanAPI already exists! This should not happen. The most likely cause is that there's multiple YanAPI plugins in your plugins folder.");
            Destroy(this);
            return;
        }

        CLogs.LogInfo("No dupes found! Initializing YanAPI.Core...");

        CLogs.LogInfo("Applying patches...");
        YanAPIPatchManager.PatchAll();

        Instance = this;
        DontDestroyOnLoad(this);


        CLogs.LogInfo("YanAPI.Core has been initialized successfully!");
        CLogs.LogInfo("Initializing modules...");

        GameSceneWrapper.Init();
        MainMenuUIBase.Init();

        CLogs.LogInfo("All modules initialized!");
        CLogs.LogInfo("Finalizing YanAPI.Core setup...");

        SceneManager.sceneLoaded += (scene, loadSceneMode) => CLogs.LogDebug($"Scene loaded: {scene.name} in mode {loadSceneMode}.");
        SceneManager.sceneUnloaded += (scene) => CLogs.LogDebug($"Scene unloaded: {scene.name}.");

        CLogs.LogInfo("YanAPI.Core initialized successfully!");

        // APIExamples.Load();
    }
    
    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
            Logger.LogInfo("YanAPI has been destroyed successfully!");
        }
    }
}
