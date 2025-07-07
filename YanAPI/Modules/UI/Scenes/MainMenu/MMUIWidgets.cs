using UnityEngine;

#nullable enable
namespace YanAPI.Modules.UI.Scenes.MainMenu;
internal static class MMUIWidgets {
    public static UIWidget? NewGame, Content, Mission, Sponsors, Settings, Credits, Extras, Quit;

    internal static void CacheFrom(Transform menuRoot) {
        foreach (Transform child in menuRoot) {
            var widget = child.GetComponent<UIWidget>();
            if (widget == null) continue;

            switch (child.name.Substring(0, 1)) {
                case "1": NewGame = widget; break;
                case "2": Content = widget; break;
                case "3": Mission = widget; break;
                case "4": Sponsors = widget; break;
                case "5": Settings = widget; break;
                case "6": Credits = widget; break;
                case "7": Extras = widget; break;
                case "8": Quit = widget; break;
            }
        }
    }
}