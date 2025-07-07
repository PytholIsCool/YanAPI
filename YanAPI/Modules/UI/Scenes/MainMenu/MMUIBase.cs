using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YanAPI.Logging;
using YanAPI.Modules.UI.Controls;
using YanAPI.Wrappers;

#nullable enable
namespace YanAPI.Modules.UI.Scenes.MainMenu; 
internal static class MMUIBase {
    public static Transform? MMObj { get; private set; }
    public static YanPage? MainMenuPage { get; private set; }
    public static UIPanel? MMUIPanel { get; private set; }

    public static bool isMainMenuUIReady { get; private set; } = false;
    public static event Action? OnMainMenuUIIsReady;

    internal static void Init() {
        GameSceneWrapper.OnMainMenuSceneLoaded += OnMainMenuSceneLoaded;
        SceneManager.sceneUnloaded += (scene) => isMainMenuUIReady = scene.name == "NewTitleScene";

        CLogs.LogInfo("MainMenuUIBase initialized!");
    }

    private static void OnMainMenuSceneLoaded() {
        MMUIPanel = (MMObj = GameObject.Find("MainMenuPanel")?.transform)?.GetComponent<UIPanel>();
        if (MMObj == null || MMUIPanel == null) {
            CLogs.LogError("MainMenuPanel or its UIPanel component not found. The scene may have loaded incorrectly or you may be using an outdated version of YanAPI.");
            return;
        }

        MMUIWidgets.CacheFrom(MMObj.Find("TitleMenu"));

        isMainMenuUIReady = true;
        OnMainMenuUIIsReady?.Invoke();
    }
}
