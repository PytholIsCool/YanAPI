using UnityEngine;
using YanAPI.UI.MainMenu;
using YanAPI.UI.MainMenu.Controls;
using YanAPI.Wrappers;

namespace YanAPI; 
public class APIExamples {
    public static void Load() {
        MainMenuUIBase.OnMainMenuUIIsReady += () => {
            new YanLabel(MainMenuUIBase.MainMenuPage, "TestButton1", () => {
                MainMenuUIBase.MainMenuPage.OpenPage();
            });
        };
    }
}
