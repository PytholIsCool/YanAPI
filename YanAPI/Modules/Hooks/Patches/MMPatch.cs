using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using YanAPI.Logging;
using YanAPI.Modules.UI.Scenes.MainMenu;
using YanAPI.Wrappers;

#nullable enable
namespace YanAPI.Modules.Hooks.Patches;
internal static class MMPatch {

    internal static void Apply() {
        var harmony = new Harmony("YanAPI.NewTitleScreenScriptPatches");

        harmony.Patch(AccessTools.Method(typeof(NewTitleScreenScript), "Start"), postfix: new(typeof(MMPatch), nameof(Start)));
        harmony.Patch(AccessTools.Method(typeof(NewTitleScreenScript), "Update"), prefix: new(typeof(MMPatch), nameof(Update)));
        CLogs.LogInfo("NewTitleScreenScript.Update patch applied successfully.");
    }

    #region Start

    private static void Start(NewTitleScreenScript __instance) {
        MMUIBase.OnMainMenuUIIsReady += () => {
            // TODO: Remake the in-game UI on the main menu panel
        };
    }

    #endregion

    #region Update

    // Mainly doing it this way so i don't have to scroll though all that bullshit to change any altered names
    const string ShadowTestSceneName = "ShadowTestScene";
    const string CustomModeSceneName = "CustomModeScene";
    const string WeekSelectSceneName = "WeekSelectScene";
    const string CalenderSceneName = "CalendarScene";
    const string EightiesCutsceneSceneName = "EightiesCutsceneScene";
    const string SenpaiSceneName = "SenpaiScene";
    const string LoadingSceneName = "LoadingScene";
    const string HomeSceneName = "HomeScene";
    const string ConfessionSceneName = "ConfessionScene";
    const string MissionModeSceneName = "MissionModeScene";
    const string CreditsSceneName = "CreditsScene";

    private static readonly MethodInfo updateCursor = AccessTools.Method(typeof(NewTitleScreenScript), "UpdateCursor");
    private static readonly MethodInfo updateBloom = AccessTools.Method(typeof(NewTitleScreenScript), "UpdateBloom");
    private static readonly MethodInfo updateDOF = AccessTools.Method(typeof(NewTitleScreenScript), "UpdateDOF");
    private static readonly MethodInfo disableEightiesEffects = AccessTools.Method(typeof(NewTitleScreenScript), "DisableEightiesEffects");
    private static readonly MethodInfo enableEightiesEffects = AccessTools.Method(typeof(NewTitleScreenScript), "EnableEightiesEffects");
    private static readonly MethodInfo setEightiesVariables = AccessTools.Method(typeof(NewTitleScreenScript), "SetEightiesVariables");

    private static bool Update(NewTitleScreenScript __instance) {
        if (__instance.Frame == 1) {
            if (__instance.Eighties)
                enableEightiesEffects?.Invoke(__instance, null);
            else
                disableEightiesEffects?.Invoke(__instance, null);

            GameGlobals.Debug = false;
        }

        __instance.Frame++;

        HandleInputs(__instance);
        HandleCursor(__instance);

        if (!__instance.FadeOut) 
            UpdatePhase(__instance);
        else
            PerformFadeOut(__instance);

        __instance.transform.LookAt(__instance.LookAtTarget);

        if (__instance.Fun)
            UpdateFunMode(__instance);

        //if (Input.GetKeyDown("z")) {
        //    __instance.Zs++;
        //    if (__instance.Zs >= 10)
        //        SceneManager.LoadScene(ShadowTestSceneName); // This is a dev-only thing
        //}

        return false;
    }

    private static void HandleInputs(NewTitleScreenScript i) {
        if (Input.GetKey(KeyCode.Escape)) {
            // NOTE: MAKE A POPUP HERE
            i.Selection = 8;
            i.FadeOut = true;
        }

        // Enabling debug log info
        if (i.Log == 0 && Input.GetKeyDown("l")) i.Log++;
        else if (i.Log == 1 && Input.GetKeyDown("o")) i.Log++;
        else if (i.Log == 2 && Input.GetKeyDown("g")) {
            CLogs.LogInfo("DebugLog object activited. If you didn't mean to do this, type \"log\" to disable it.");
            i.DebugLog.gameObject.SetActive(!i.DebugLog.gameObject.activeSelf); // Usually just set to true. I changed this so that you can enable and disable it whenever
            i.Log++;
        }

        if (i.Log == 3) {
            i.DebugLog.text = $"GameGlobals.Debug is: {GameGlobals.Debug} and QuickStart is: {i.QuickStart}\nType \"Log\" to disable this message";
            i.Log = 0; // This wasnt here before. I added it so that you can activate the log code again which will set the object's activity to the opposite of itself
        }

        // Muting the music
        if (Input.GetKeyDown("m"))
            i.CurrentJukebox.volume = i.CurrentJukebox.volume == 0 ? 0.5f : 0f;
    }

    private static void HandleCursor(NewTitleScreenScript i) {
        i.Cursor.transform.parent.Rotate(Vector3.right * (Time.deltaTime * 100f), Space.Self);

        if (i.Phase != 2)
            i.Cursor.alpha = Mathf.MoveTowards(i.Cursor.alpha, 0f, Time.deltaTime);

        var newPos = new Vector3(
            i.PositionX,
            300f - 75f * i.Selection, // Could be of use in the future
            i.Cursor.transform.parent.localPosition.z
        );

        i.Cursor.transform.parent.localPosition = Vector3.Lerp(
            i.Cursor.transform.parent.localPosition,
            newPos,
            Time.deltaTime * 10f
        );
    }

    private static void UpdatePhase(NewTitleScreenScript i) {
        i.PressStart.text = i.InputDevice.Type == InputDeviceType.Gamepad ? "PRESS START" : "PRESS ENTER";

        if (i.Phase < 2) {
            i.transform.position = Vector3.Lerp(i.transform.position, new Vector3(0f, 0.5f, -17f), Time.deltaTime * 0.5f * i.SpeedUpFactor);
            i.EightiesLogo.alpha = Mathf.MoveTowards(i.EightiesLogo.alpha, 1f, Time.deltaTime * 0.2f);
            i.BloomIntensity = Mathf.Lerp(i.BloomIntensity, 1f, Time.deltaTime * i.Speed);
            i.BloomRadius = Mathf.Lerp(i.BloomRadius, 4f, Time.deltaTime * i.Speed);
            updateBloom?.Invoke(i, [i.BloomIntensity, i.BloomRadius]);
            updateDOF?.Invoke(i, [2f]);
        }

        switch (i.Phase) {
            case 0: TitleScreenPhaseHandler.Phase0(i); break;
            case 1: TitleScreenPhaseHandler.Phase1(i); break;
            case 2: TitleScreenPhaseHandler.Phase2(i); break;
            case 3: TitleScreenPhaseHandler.Phase3(i); break;
            case 4: TitleScreenPhaseHandler.Phase4(i); break;
            case 5: TitleScreenPhaseHandler.Phase5(i); break;
            case 6: TitleScreenPhaseHandler.Phase6(i); break;
            case 7: TitleScreenPhaseHandler.Phase7(i); break;
            case 8: TitleScreenPhaseHandler.Phase8(i); break;
        }
    }

    private static void PerformFadeOut(NewTitleScreenScript i) {
        i.PromptBar.Show = false;
        i.Darkness.alpha = Mathf.MoveTowards(i.Darkness.alpha, 1f, Time.deltaTime * 0.5f);
        i.CurrentJukebox.volume = Mathf.MoveTowards(i.CurrentJukebox.volume, 0f, Time.deltaTime * 0.5f);
        i.FountainSFX[1].volume = Mathf.MoveTowards(i.FountainSFX[1].volume, 0f, Time.deltaTime * 0.5f);
        i.FountainSFX[2].volume = Mathf.MoveTowards(i.FountainSFX[2].volume, 0f, Time.deltaTime * 0.5f);

        if (i.Darkness.alpha == 1f)
            TitleScreenPhaseHandler.FadeOutTransition(i);
    }

    private static void UpdateFunMode(NewTitleScreenScript i) { // I kinda made this fucntion blindly. I still don't have a very good understanding of what this is supposed to do
        i.FunTimer += Time.unscaledDeltaTime;

        if (i.FunPhase == 1 && i.FunTimer >= 10f) {
            var pos = new Vector3(UnityEngine.Random.Range(0f, 17.5f), 0f, UnityEngine.Random.Range(0f, 5f));
            pos.y = UnityEngine.Random.Range(0.6f, 0.6f + (pos.x + pos.z) * 0.4f);
            i.FunGirl.transform.position = pos;
            i.FunGirl.SetActive(true);
            i.FunPhase = 2;
            i.FunTimer = 0f;
        } else if (i.FunPhase == 2 && i.FunTimer >= 1f) {
            i.FunGirl.SetActive(false);
            i.FunPhase = 1;
            i.FunTimer = 0f;
        }
    }

    public static class TitleScreenPhaseHandler {
        public static void Phase0(NewTitleScreenScript i) {
            if (Input.anyKeyDown)
                i.Speed += 1f;

            if (i.BloomIntensity < 1.1f) {
                if (i.CongratulationsWindow.activeInHierarchy) {
                    if (!i.PromptBar.Show) {
                        i.PromptBar.ClearButtons();
                        i.PromptBar.Label[0].text = "Awesome! Let's go!";
                        i.PromptBar.UpdateButtons();
                        i.PromptBar.Show = true;
                    }

                    if (Input.GetButtonDown(InputNames.Xbox_A)) {
                        i.CongratulationsWindow.SetActive(false);
                        i.PromptBar.Show = false;
                    }
                } else {
                    i.PressStart.alpha = Mathf.MoveTowards(i.PressStart.alpha, 1f, Time.deltaTime);
                    if (Input.GetButtonDown(InputNames.Xbox_Start)) {
                        i.PromptBar.ClearButtons();
                        i.PromptBar.Label[0].text = "Confirm";
                        i.PromptBar.Label[5].text = "Change Selection";
                        i.PromptBar.UpdateButtons();
                        i.PromptBar.Show = true;
                        i.Speed = 0f;
                        i.Phase++;
                    }
                }
            }
        }

        public static void Phase1(NewTitleScreenScript i) {
            i.LookAtTarget.position = Vector3.Lerp(i.LookAtTarget.position, new Vector3(0f, 5.055138f, 0f), Time.deltaTime);
            i.ModeSelection.alpha = Mathf.MoveTowards(i.ModeSelection.alpha, 1f, Time.deltaTime);
            i.PressStart.alpha = Mathf.MoveTowards(i.PressStart.alpha, 0f, Time.deltaTime);

            if (i.Eighties) {
                i.EightiesWindow.alpha = Mathf.MoveTowards(i.EightiesWindow.alpha, 1f, Time.deltaTime * 10f);
                i.DemoWindow.alpha = Mathf.MoveTowards(i.DemoWindow.alpha, 0.25f, Time.deltaTime * 10f);
            } else {
                i.EightiesWindow.alpha = Mathf.MoveTowards(i.EightiesWindow.alpha, 0.25f, Time.deltaTime * 10f);
                i.DemoWindow.alpha = Mathf.MoveTowards(i.DemoWindow.alpha, 1f, Time.deltaTime * 10f);
            }

            if (i.ModeSelection.alpha == 1f && i.LookAtTarget.position.y > 3f) {
                if (i.InputManager.TappedLeft || i.InputManager.TappedRight) {
                    i.Eighties = !i.Eighties;
                    GameGlobals.Eighties = i.Eighties;
                    if (i.Eighties)
                        enableEightiesEffects?.Invoke(i, null);
                    else
                        disableEightiesEffects?.Invoke(i, null);
                }

                if (Input.GetButtonDown(InputNames.Xbox_A)) {
                    i.PromptBar.Label[0].text = "Make Selection";
                    i.PromptBar.Label[1].text = "Back";
                    i.PromptBar.Label[4].text = "Change Selection";
                    i.PromptBar.Label[5].text = "";
                    i.PromptBar.UpdateButtons();

                    i.MyAudio.clip = i.Whoosh;
                    i.MyAudio.pitch = 0.5f;
                    i.MyAudio.volume = 1f;
                    i.MyAudio.Play();

                    i.Speed = 0f;
                    i.Phase = 2;
                }
            }
        }

        public static void Phase2(NewTitleScreenScript i) {
            i.Speed += Time.deltaTime * 2f;
            i.transform.position = Vector3.Lerp(i.transform.position, new Vector3(-0.666666f, 1.195f, -2.9f), Time.deltaTime * i.Speed);
            i.LookAtTarget.position = Vector3.Lerp(i.LookAtTarget.position, new Vector3(0f, 1.195f, -2.263333f), Time.deltaTime * i.Speed);
            i.DepthFocus = Mathf.Lerp(i.DepthFocus, 1f, Time.deltaTime * i.Speed);
            updateDOF?.Invoke(i, [i.DepthFocus]);

            i.Settings.alpha = Mathf.MoveTowards(i.Settings.alpha, 0f, Time.deltaTime);
            i.Sponsors.alpha = Mathf.MoveTowards(i.Sponsors.alpha, 0f, Time.deltaTime);
            i.SaveFiles.alpha = Mathf.MoveTowards(i.SaveFiles.alpha, 0f, Time.deltaTime);
            i.ExtrasMenu.alpha = Mathf.MoveTowards(i.ExtrasMenu.alpha, 0f, Time.deltaTime);
            i.DemoChecklist.alpha = Mathf.MoveTowards(i.DemoChecklist.alpha, 0f, Time.deltaTime);

            if (i.Speed > 3f) {
                i.Cursor.alpha = Mathf.MoveTowards(i.Cursor.alpha, 1f, Time.deltaTime);

                if (i.Cursor.alpha == 1f) {
                    if (!i.PromptBar.Show && !i.ForVideo) {
                        i.PromptBar.ClearButtons();
                        i.PromptBar.Label[0].text = "Make Selection";
                        i.PromptBar.Label[1].text = "Back";
                        i.PromptBar.Label[4].text = "Change Selection";
                        i.PromptBar.UpdateButtons();
                        i.PromptBar.Show = true;
                    }

                    if (i.InputManager.TappedDown) { i.Selection++; updateCursor?.Invoke(i, null); }
                    if (i.InputManager.TappedUp) { i.Selection--; updateCursor?.Invoke(i, null); }

                    if (Input.GetButtonDown(InputNames.Xbox_A)) HandleSelection(i);
                    else if (Input.GetButtonDown(InputNames.Xbox_B)) {
                        i.PromptBar.Label[1].text = "";
                        i.PromptBar.Label[4].text = "";
                        i.PromptBar.Label[5].text = "Change Selection";
                        i.PromptBar.UpdateButtons();
                        i.SpeedUpFactor = 10f;
                        i.Speed = 0f;
                        i.Phase = 1;
                    }
                }
            }
        }

        public static void Phase3(NewTitleScreenScript i) {
            i.Speed += Time.deltaTime * 2f;
            i.transform.position = Vector3.Lerp(i.transform.position, new Vector3(0.125f, 0.875f, -2.66666f), Time.deltaTime * i.Speed);
            i.LookAtTarget.position = Vector3.Lerp(i.LookAtTarget.position, new Vector3(0.1f, 0.875f, 0f), Time.deltaTime * i.Speed);
            i.SaveFiles.alpha = Mathf.MoveTowards(i.SaveFiles.alpha, 1f, Time.deltaTime);
            i.DepthFocus = Mathf.Lerp(i.DepthFocus, 0.225f, Time.deltaTime * i.Speed);
            updateDOF?.Invoke(i, [i.DepthFocus]);
        }

        public static void Phase4(NewTitleScreenScript i) {
            i.Speed += Time.deltaTime * 2f;
            i.transform.position = Vector3.Lerp(i.transform.position, new Vector3(0f, 29.5f, 0f), Time.deltaTime * i.Speed);
            i.LookAtTarget.position = Vector3.Lerp(i.LookAtTarget.position, new Vector3(0f, 0f, 0.1f), Time.deltaTime * i.Speed);
            i.DemoChecklist.alpha = Mathf.MoveTowards(i.DemoChecklist.alpha, 1f, Time.deltaTime);
            i.DepthFocus = Mathf.Lerp(i.DepthFocus, 2f, Time.deltaTime * i.Speed);
            updateDOF?.Invoke(i, [i.DepthFocus]);
        }

        public static void Phase5(NewTitleScreenScript i) {
            i.Speed += Time.deltaTime * 2f;
            i.transform.position = Vector3.Lerp(i.transform.position, new Vector3(0f, 0.66f, -1.6f), Time.deltaTime * i.Speed);
            i.LookAtTarget.position = Vector3.Lerp(i.LookAtTarget.position, new Vector3(0f, 0.66f, 0f), Time.deltaTime * i.Speed);
            i.Sponsors.alpha = Mathf.MoveTowards(i.Sponsors.alpha, 1f, Time.deltaTime);
            i.DepthFocus = Mathf.Lerp(i.DepthFocus, 1f, Time.deltaTime * i.Speed);
            updateDOF?.Invoke(i, [i.DepthFocus]);
        }

        public static void Phase6(NewTitleScreenScript i) {
            i.Speed += Time.deltaTime * 2f;
            i.transform.position = Vector3.Lerp(i.transform.position, new(0f, 0.85f, -4f), Time.deltaTime * i.Speed);
            i.LookAtTarget.position = Vector3.Lerp(i.LookAtTarget.position, new(0f, 0.85f, 0f), Time.deltaTime * i.Speed);
            i.Settings.alpha = Mathf.MoveTowards(i.Settings.alpha, 1f, Time.deltaTime); // In charge of the fade-in animation for the settings menu
            i.DepthFocus = Mathf.Lerp(i.DepthFocus, 2f, Time.deltaTime * i.Speed);
            updateDOF?.Invoke(i, [i.DepthFocus]);
        }

        public static void Phase7(NewTitleScreenScript i) {
            i.Speed += Time.deltaTime * 2f;
            i.transform.position = Vector3.Lerp(i.transform.position, new(0f, 2.372f, -0.5f), Time.deltaTime * i.Speed);
            i.LookAtTarget.position = Vector3.Lerp(i.LookAtTarget.position, new(0f, 2.372f, 0f), Time.deltaTime * i.Speed);
            i.DepthFocus = Mathf.Lerp(i.DepthFocus, 0.5f, Time.deltaTime * i.Speed);
            updateDOF?.Invoke(i, [i.DepthFocus]);
            i.ExtrasMenu.alpha = Mathf.MoveTowards(i.ExtrasMenu.alpha, 1f, Time.deltaTime);

            if (i.Speed > 3f && !i.PromptBar.Show) {
                i.PromptBar.ClearButtons();
                i.PromptBar.Label[0].text = "Make Selection";
                i.PromptBar.Label[1].text = "Back";
                i.PromptBar.Label[4].text = "Change Selection";
                i.PromptBar.UpdateButtons();
                i.PromptBar.Show = true;
                i.TitleExtras.Show = true;
            }
        }

        public static void Phase8(NewTitleScreenScript i) {
            if (i.TitleScreenPanel.alpha > 0f) {
                i.TitleScreenPanel.alpha = Mathf.MoveTowards(i.TitleScreenPanel.alpha, 0f, Time.deltaTime * 2f);
            } else if (!i.FadeQuestion) {
                i.Questions[i.CurrentQuestion].alpha = Mathf.MoveTowards(i.Questions[i.CurrentQuestion].alpha, 1f, Time.deltaTime * 2f);
                if (Input.GetButtonDown(InputNames.Xbox_A))
                    i.FadeQuestion = true;
            } else {
                i.Questions[i.CurrentQuestion].alpha = Mathf.MoveTowards(i.Questions[i.CurrentQuestion].alpha, 0f, Time.deltaTime * 2f);
                if (i.Questions[i.CurrentQuestion].alpha == 0f) {
                    i.FadeQuestion = false;
                    i.CurrentQuestion++;
                }
            }
        }

        private static void HandleSelection(NewTitleScreenScript i) {
            if (i.ForVideo) {
                i.Phase = 7;
                return;
            }

            i.PromptBar.Show = false;
            i.MyAudio.clip = i.MakeSelection;
            i.MyAudio.volume = 0.5f;
            i.MyAudio.pitch = 1f;
            i.MyAudio.Play();

            switch (i.Selection) {
                case 1:
                    GameWrapper.OnNewGameSelectedInvoke();
                    i.TitleSaveFiles.enabled = true;
                    i.Speed = 0f;
                    i.Phase = 3;
                    break;
                case 2:
                    GameWrapper.OnContentCLSelectedInvoke();
                    i.TitleDemoChecklist.enabled = true;
                    i.Speed = 0f;
                    i.Phase = 4;
                    break;
                case 3:
                    GameWrapper.OnMissionModeSelectedInvoke();
                    i.FadeOut = true;
                    break;
                case 4:
                    GameWrapper.OnSponsorsSelectedInvoke();
                    i.TitleSponsor.enabled = true;
                    i.Speed = 0f;
                    i.Phase = 5;
                    break;
                case 5:
                    GameWrapper.OnSettingsSelectedInvoke();
                    i.NewSettings.enabled = true;
                    i.NewSettings.Cursor.alpha = 0f;
                    i.NewSettings.Selection = 1;
                    i.Speed = 0f;
                    i.Phase = 6;
                    break;

                case 7:
                    GameWrapper.OnExtrasSelectedInvoke();
                    if (i.ExtrasLabel.alpha == 1f) {
                        i.Speed = 0f;
                        i.Phase = 7;
                    }
                    else
                        i.PromptBar.Show = true;
                    break;
                case 6:
                    GameWrapper.OnCreditsSelectedInvoke();
                    i.FadeOut = true;
                    break;
                case 8:
                    i.FadeOut = true;
                    break;
            }
        }

        internal static void FadeOutTransition(NewTitleScreenScript i) {
            Time.timeScale = 1f;

            if (i.Selection == 1) {
                if (PlayerPrefs.GetInt("ProfileCreated_" + GameGlobals.Profile.ToString()) == 0) {
                    PlayerPrefs.SetInt("ProfileCreated_" + GameGlobals.Profile.ToString(), 1);
                    GameGlobals.MostRecentSlot = 0;
                    PlayerGlobals.Money = 10f;
                    DateGlobals.Weekday = DayOfWeek.Sunday;
                    DateGlobals.PassDays = 1;

                    // Set up initial state
                    if (i.CustomMode) {
                        setEightiesVariables?.Invoke(i, null);
                        GameGlobals.EightiesTutorial = false;
                        GameGlobals.CustomMode = true;
                        SceneManager.LoadScene(CustomModeSceneName);
                    } else if (i.WeekSelect) {
                        if (GameGlobals.Eighties)
                            setEightiesVariables?.Invoke(i, null);

                        GameGlobals.EightiesTutorial = false;
                        SceneManager.LoadScene(WeekSelectSceneName);
                    } else if (i.QuickStart) {
                        GameGlobals.CameFromTitleScreen = true;
                        SceneManager.LoadScene(CalenderSceneName);
                    } else if (i.Eighties) {
                        setEightiesVariables?.Invoke(i, null);
                        SceneManager.LoadScene(EightiesCutsceneSceneName);
                    } else {
                        StudentGlobals.FemaleUniform = 1;
                        StudentGlobals.MaleUniform = 1;
                        GameGlobals.LastInputType = (int)i.InputDevice.Type;
                        SceneManager.LoadScene(SenpaiSceneName);
                    }
                } else if (GameGlobals.KokonaTutorial) {
                    SceneManager.LoadScene(LoadingSceneName);
                } else if (GameGlobals.EightiesTutorial) {
                    SceneManager.LoadScene(EightiesCutsceneSceneName);
                } else if (GameGlobals.Eighties && GameGlobals.InCutscene) {
                    SceneManager.LoadScene(EightiesCutsceneSceneName);
                } else if (DateGlobals.Week < 11) {
                    SceneManager.LoadScene(HomeGlobals.Night ? HomeSceneName : CalenderSceneName);
                } else if (GameGlobals.CustomMode || GameGlobals.AlternateTimeline) {
                    SceneManager.LoadScene(ConfessionSceneName);
                } else {
                    SceneManager.LoadScene(LoadingSceneName);
                }
            } else if (i.Selection == 3) {
                SceneManager.LoadScene(MissionModeSceneName);
            } else if (i.Selection == 6) {
                GameGlobals.CameFromTitleScreen = true;
                SceneManager.LoadScene(CreditsSceneName);
            } else if (i.Selection == 8) {
                Application.Quit();
            }
        }

    }

    #endregion

}
