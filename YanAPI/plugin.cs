using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        CLogs.LogInfo("No dupes found! Initializing YanAPI...");

        CLogs.LogInfo("Applying patches...");
        YanAPIPatchManager.PatchAll();

        Instance = this;
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        CLogs.LogInfo("YanAPI has been initialized successfully!");

        GameWrapper.OnNewGameSelected += () => {
            CLogs.LogInfo("New game/Load game selected.");
        };

        GameWrapper.OnSettingsSelected += () => {
            CLogs.LogInfo("Settings selected.");
        };
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        CLogs.Log($"[YanAPI] Scene loaded: \"{scene.name}\" in mode \"{loadSceneMode}\"".ConvertToANSI(185, 160, 255));
        if (GameWrapper.IsInMainMenu()) {
            CLogs.LogInfo("Main menu scene detected. Initializing MMUI module...");
            MMUIModuleBase.Init();
        }
    }

    private void OnSceneUnloaded(Scene scene) {
        CLogs.Log($"[YanAPI] Scene unloaded: \"{scene.name}\"".ConvertToANSI(185, 160, 255));
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
            Logger.LogInfo("YanAPI has been destroyed successfully!");
        }
    }
}
