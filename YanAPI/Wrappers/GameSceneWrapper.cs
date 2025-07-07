using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using YanAPI.Logging;

namespace YanAPI.Wrappers {
    public static class GameSceneWrapper {
        private static readonly Dictionary<string, Action> SceneEvents = new() {
            { "ResolutionScene", () => OnResolutionSceneLoaded?.Invoke() },
            { "DisclaimerScene", () => OnDisclaimerSceneLoaded?.Invoke() },
            { "WelcomeScene", () => OnWelcomeSceneLoaded?.Invoke() },
            { "SponsorScene", () => OnSponsorSceneLoaded?.Invoke() },
            { "NewTitleScene", () => OnMainMenuSceneLoaded?.Invoke() },
            { "CalendarScene", () => OnCalendarSceneLoaded?.Invoke() },
            { "HomeScene", () => OnHomeSceneLoaded?.Invoke() },
            { "StreetScene", () => OnTownSceneLoaded?.Invoke() },
            { "LoadingScene", () => OnLoadingSceneLoaded?.Invoke() },
            { "SchoolScene", () => OnSchoolSceneLoaded?.Invoke() },
            { "SempaiScene", () => OnSempaiSceneLoaded?.Invoke() },
            { "NewIntroScene", () => OnAyanoMonologueSceneLoaded?.Invoke() },
            { "KokonaTutorialScene", () => OnKokonaTutorialSceneLoaded?.Invoke() },
            { "PhoneScene", () => OnPhoneSceneLoaded?.Invoke() }
        };

        public static event Action OnResolutionSceneLoaded;     // ResolutionScene
        public static event Action OnDisclaimerSceneLoaded;     // DisclaimerScene
        public static event Action OnWelcomeSceneLoaded;        // WelcomeScene
        public static event Action OnSponsorSceneLoaded;        // SponsorScene
        public static event Action OnMainMenuSceneLoaded;       // NewTitleScene
        public static event Action OnCalendarSceneLoaded;       // CalendarScene
        public static event Action OnHomeSceneLoaded;           // HomeScene
        public static event Action OnTownSceneLoaded;           // StreetScene
        public static event Action OnLoadingSceneLoaded;        // LoadingScene
        public static event Action OnSchoolSceneLoaded;         // SchoolScene
        public static event Action OnSempaiSceneLoaded;         // SempaiScene
        public static event Action OnAyanoMonologueSceneLoaded; // NewIntroScene
        public static event Action OnKokonaTutorialSceneLoaded; // KokonaTutorialScene
        public static event Action OnPhoneSceneLoaded;          // PhoneScene

        internal static void Init() {
            SceneManager.sceneLoaded += (scene, _) => {
                if (SceneEvents.TryGetValue(scene.name, out var action)) {
                    action.Invoke();
                } else {
                    CLogs.LogWarning($"SceneWrapper: Unhandled scene loaded: {scene.name}");
                }
            };

            CLogs.LogInfo("SceneWrapper initialized! Listening for scene changes...");
        }
    }
}