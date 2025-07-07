using YanAPI.Modules.UI.Scenes.MainMenu;
using YanAPI.Modules.UI.Controls;

namespace YanAPI; 
public class APIExamples {
    public static void Load() {
        MMUIBase.OnMainMenuUIIsReady += () => {
            YanPage page = new("TestPage", MMUIBase.MMObj);
            page.transform.position = new(0f, 1.235f, -2.75f);
            page.transform.rotation = MMUIBase.MMObj.rotation;

            for (int i = 0; i < 8; i++) {
                new YanLabel(page, $"TestButton{i}", () => {
                    // Do something when the button is clicked
                });
            }
        };
    }
}
