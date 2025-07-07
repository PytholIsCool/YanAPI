using UnityEngine;
using UnityEngine.SceneManagement;
using YanAPI.Modules.UI.Scenes.MainMenu;
using YanAPI.Wrappers;

namespace YanAPI.Modules.UI.Scenes; 
internal static class UniUIBase {
    internal static void Init() {
        GameSceneWrapper.OnMainMenuSceneLoaded += () => MMUIBase.Init();

        GameSceneWrapper.OnHomeSceneLoaded += () => BedroomUIBase.Init();

        GameSceneWrapper.OnSchoolSceneLoaded += () => SchoolUIBase.Init();
    }
}
