using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using YanAPI.Logging;

namespace YanAPI.Wrappers {
    public class GameSceneWrapper {
        #region Events

        public static event Action OnResolutionSceneLoaded;
        public static event Action OnDisclaimerSceneLoaded;
        public static event Action OnWelcomeSceneLoaded;
        public static event Action OnSponsorSceneLoaded;
        public static event Action OnMainMenuSceneLoaded; // NewTitleScene
        public static event Action OnCalendarSceneLoaded;
        public static event Action OnHomeSceneLoaded;
        public static event Action OnTownSceneLoaded; // StreetScene
        public static event Action OnLoadingSceneLoaded;
        public static event Action OnSchoolSceneLoaded;
        public static event Action OnSempaiSceneLoaded;
        public static event Action OnAyanoMonologueSceneLoaded; // NewIntroScene
        public static event Action OnKokonaTutorialSceneLoaded;
        public static event Action OnPhoneSceneLoaded;

        #endregion
        internal static void Init() {
            SceneManager.sceneLoaded += (scene, _) => {
                switch (scene.name) {
                    case "ResolutionScene":
                        OnResolutionSceneLoaded?.Invoke();
                        break;
                    case "DisclaimerScene":
                        OnDisclaimerSceneLoaded?.Invoke();
                        break;
                    case "WelcomeScene":
                        OnWelcomeSceneLoaded?.Invoke();
                        break;
                    case "SponsorScene":
                        OnSponsorSceneLoaded?.Invoke();
                        break;
                    case "NewTitleScene":
                        OnMainMenuSceneLoaded?.Invoke();
                        break;
                    case "CalendarScene":
                        OnCalendarSceneLoaded?.Invoke();
                        break;
                    case "HomeScene":
                        OnHomeSceneLoaded?.Invoke();
                        break;
                    case "StreetScene":
                        OnTownSceneLoaded?.Invoke();
                        break;
                    case "LoadingScene":
                        OnLoadingSceneLoaded?.Invoke();
                        break;
                    case "SchoolScene":
                        OnSchoolSceneLoaded?.Invoke();
                        break;
                    case "SempaiScene":
                        OnSempaiSceneLoaded?.Invoke();
                        break;
                    case "NewIntroScene":
                        OnAyanoMonologueSceneLoaded?.Invoke();
                        break;
                    case "KokonaTutorialScene":
                        OnKokonaTutorialSceneLoaded?.Invoke();
                        break;
                    case "PhoneScene":
                        OnPhoneSceneLoaded?.Invoke();
                        break;
                    default:
                        CLogs.LogWarning($"SceneWrapper: Unhandled scene loaded: {scene.name}");
                        break;
                };
            };

            CLogs.LogInfo("SceneWrapper initialized! Listening for scene changes...");
        }

    }
}
