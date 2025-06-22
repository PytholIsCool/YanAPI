using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using YanAPI.Logging;
using Object = UnityEngine.Object;

namespace YanAPI.Wrappers;
public class GameWrapper {

    #region Core Runtime

    private static readonly object _runtimeLock = new();

    /// <summary>
    /// Forcefully closes the game process.
    /// </summary>
    public static void ForceClose() {
        lock (_runtimeLock) {
            try {
                Process.GetCurrentProcess().Kill();
            } catch (Exception ex) {
                CLogs.LogError($"Error while trying to force close the game: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Safely closes the game, checking for active scenes and preventing any closes if the player is at risk of losing progress.
    /// </summary>
    public static void SafeClose() {
        lock (_runtimeLock) {
            try {
                string currentScene = SceneManager.GetActiveScene().name;

                switch (currentScene) {
                    case "NewTitleScene": {
                            var svm = GetSaveFilesManager();

                            if (svm.DifficultyWindow.activeSelf == true || svm.WouldYouLikeToWindow.activeSelf == true) {
                                CLogs.LogWarning("Prevented game close due to active save config window."); // Temporary safety measure until I can get popups working
                                return;
                            }

                            Application.Quit();
                            break;
                        }
                    case "SempaiScene": {
                            CLogs.LogWarning("Prevented game close due to active save config window."); // Temporary safety measure until I can get popups working

                            break;
                        }
                    case "SchoolScene": {
                            CLogs.LogWarning("Prevented game close due to being in an active game."); // Temporary safety measure until I can get popups working

                            break;
                        }
                    default: {
                            Application.Quit();
                            break;
                        }
                }
            } catch (Exception ex) {
                CLogs.LogError($"Error while trying to close the game: {ex.Message}");
            }
        }
    }

    #endregion

    #region Core Objects

    // Main Menu

    #region Events

    public static event Action OnNewGameSelected;
    internal static void OnNewGameSelectedInvoke() => OnNewGameSelected?.Invoke();
    public static event Action OnContentCLSelected;
    internal static void OnContentCLSelectedInvoke() => OnContentCLSelected?.Invoke();
    public static event Action OnMissionModeSelected;
    internal static void OnMissionModeSelectedInvoke() => OnMissionModeSelected?.Invoke();
    public static event Action OnSponsorsSelected;
    internal static void OnSponsorsSelectedInvoke() => OnSponsorsSelected?.Invoke();
    public static event Action OnSettingsSelected;
    internal static void OnSettingsSelectedInvoke() => OnSettingsSelected?.Invoke();
    public static event Action OnCreditsSelected;
    internal static void OnCreditsSelectedInvoke() => OnCreditsSelected?.Invoke();
    public static event Action OnExtrasSelected;
    internal static void OnExtrasSelectedInvoke() => OnExtrasSelected?.Invoke();

    #endregion
    public static bool IsInMainMenu() => SceneManager.GetActiveScene().name == "NewTitleScene";

    public static NewTitleScreenScript GetMenuManager() => IsInMainMenu() ? Object.FindFirstObjectByType<NewTitleScreenScript>() : null;
    
    public static TitleSaveFilesScript GetSaveFilesManager() => IsInMainMenu() ? Object.FindFirstObjectByType<TitleSaveFilesScript>() : null;

    // In-Game

    public static bool IsInGame() => SceneManager.GetActiveScene().name == "SchoolScene";

    public static PauseScreenScript GetPauseManager() => IsInGame() ? Object.FindFirstObjectByType<PauseScreenScript>() : null;

    public static YandereScript GetLocalPlayerManager() => IsInGame() ? Object.FindFirstObjectByType<YandereScript>() : null;

    public static Transform GetLocalPlayerTransform() => IsInGame() ? GetLocalPlayerManager().transform : null;

    // Bedroom



    #endregion

}
