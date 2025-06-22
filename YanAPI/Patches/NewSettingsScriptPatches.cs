using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using YanAPI.Logging;
using YanAPI.Wrappers;

namespace YanAPI.Patches {
    public static class NewSettingsScriptPatches {
        internal static void Apply() {
            var harmony = new Harmony("YanAPI.NewSettingsScriptPatches");

            harmony.Patch(AccessTools.Method(typeof(NewSettingsScript), "Update"), prefix: new(typeof(NewSettingsScriptPatches), nameof(Update)));
            CLogs.LogInfo("NewSettingsScript.Update patch applied successfully.");
        }

        #region Update

        // 0 = Main settings menu
        // 1 = Graphics (Post-processing)
        // 2 = Quality (environment settings)
        // 3 = Controls / Accessibility
        // 4 = Censorship
        // 5 = Eighties Effects
        // 6 = Display settings (Resolution, Fullscreen, Minimalist HUD)

        private static bool Update(NewSettingsScript __instance) {
            if (Input.GetButtonDown(InputNames.Xbox_B)) {
                __instance.PromptBar.Show = false;
                __instance.PromptBar.ClearButtons();
                __instance.PromptBar.UpdateButtons();

                __instance.Menu = 0;
                __instance.Selection = 1;
                __instance.Transition = false;

                if (__instance.SchoolScene) {
                    if (__instance.PauseScreen != null && __instance.PauseScreen.MainMenu != null) {
                        // Closing the settings menu in the phone
                        __instance.PauseScreen.MainMenu.SetActive(true);
                        __instance.gameObject.SetActive(false);

                        // This forcefully opens the phone
                        var pauseMan = GameWrapper.GetPauseManager();
                        pauseMan.Show = true;
                        pauseMan.Panel.enabled = true;
                    }
                } else {
                    if (__instance.NewTitleScreen != null) {
                        __instance.NewTitleScreen.Phase = 2;
                        __instance.NewTitleScreen.Speed = 0f;
                    }
                }

                __instance.enabled = false;
            }

            return false;
        }

        #endregion

    }
}
