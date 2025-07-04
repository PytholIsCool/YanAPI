using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YanAPI.Logging;
using YanAPI.UI.Base.Assets;
using YanAPI.UI.MainMenu.Controls;
using YanAPI.Utils;
using YanAPI.Wrappers;

namespace YanAPI.UI.MainMenu; 
public static class MainMenuUIBase {
#nullable enable
    public static Transform? MMObj { get; private set; }
    public static YanPage? MainMenuPage { get; private set; }
    public static UIPanel? MMUIPanel { get; private set; }
    public static UIWidget? NewGameWgt, ContentWgt, MissionWgt, SponsorsWgt, SettingsWgt, CreditsWgt, ExtrasWgt, QuitWgt;
#nullable disable

    public static bool isMainMenuUIReady { get; private set; } = false;
    public static event Action OnMainMenuUIIsReady;

    internal static void Init() {
        GameSceneWrapper.OnMainMenuSceneLoaded += OnMainMenuSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        CLogs.LogInfo("MainMenuUIBase initialized!");
    }

    private static void OnMainMenuSceneLoaded() {
        CLogs.LogDebug("Main Menu Scene loaded, initializing UI...");

        MMUIPanel = (MMObj = GameObject.Find("MainMenuPanel")?.transform)?.GetComponent<UIPanel>();
        MainMenuPage = MMObj.gameObject.AddComponent<YanPage>();
        MainMenuPage.UIPanelCompnt = MMUIPanel;
        MainMenuPage.isOpen = true;

        foreach (Transform child in MMObj.Find("TitleMenu")) {
            CLogs.LogDebug($"Found child: \"{child.name}\"");
            var childWidget = child.GetComponent<UIWidget>();
            if (childWidget == null)
                continue;
            
            switch (int.Parse(child.name.Substring(0, 1))) {
                case 1: NewGameWgt = childWidget;
                    break;
                case 2: ContentWgt = childWidget;
                    break;
                case 3: MissionWgt = childWidget;
                    break;
                case 4: SponsorsWgt = childWidget;
                    break;
                case 5: SettingsWgt = childWidget; 
                    break;
                case 6: CreditsWgt = childWidget;
                    break;
                case 7: ExtrasWgt = childWidget;
                    break;
                case 8: QuitWgt = childWidget;
                    break; 
                default:
                    continue;
            }

            CLogs.LogDebug($"Widget reference set: {nameof(childWidget)}");
        }

        if (Fonts.FuturaCondensed == null)
            Fonts.CacheFuturaCondensedFromReference((UILabel)SettingsWgt);

        isMainMenuUIReady = true;
        OnMainMenuUIIsReady?.Invoke();
    }

    private static void OnSceneUnloaded(Scene scene) {
        if (scene.name == "NewTitleScene")
            isMainMenuUIReady = false;
    }

}
