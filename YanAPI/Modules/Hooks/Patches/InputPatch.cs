using HarmonyLib;
using UnityEngine;
using YanAPI.Logging;
using YanAPI.Modules.Inputs;

namespace YanAPI.Modules.Hooks.Patches; 
internal static class InputPatch {
    internal static void Apply() {
        var harmony = new Harmony("YanAPI.InputManagerScriptPatches");

        harmony.Patch(AccessTools.Method(typeof(InputManagerScript), "Update"), prefix: new(typeof(InputPatch), nameof(Update)));
        CLogs.LogInfo("InputManagerScript.Update patch applied successfully.");
    }

    private static bool Update(InputManagerScript __instance) { // May seem like a log to run on an update loop but this handles all inputs for the game (including custom ones) and it's all centralized
        __instance.TappedUp = false;
        __instance.TappedDown = false;
        __instance.TappedRight = false;
        __instance.TappedLeft = false;

        float dpadY = Input.GetAxisRaw(InputNames.Xbox_DpadY);

        if (dpadY > 0.5f) {
            __instance.TappedUp = !__instance.DPadUp;
            __instance.DPadUp = true;
        } else if (dpadY < -0.5f) {
            __instance.TappedDown = !__instance.DPadDown;
            __instance.DPadDown = true;
        } else {
            __instance.DPadUp = false;
            __instance.DPadDown = false;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
            float vertical = Input.GetAxisRaw("Vertical");

            if (vertical > 0.5f) {
                __instance.TappedUp = !__instance.StickUp;
                __instance.StickUp = true;
            } else if (vertical < -0.5f) {
                __instance.TappedDown = !__instance.StickDown;
                __instance.StickDown = true;
            } else {
                __instance.StickUp = false;
                __instance.StickDown = false;
            }
        }

        float dpadX = Input.GetAxisRaw(InputNames.Xbox_DpadX);

        if (dpadX > 0.5f) {
            __instance.TappedRight = !__instance.DPadRight;
            __instance.DPadRight = true;
        } else if (dpadX < -0.5f) {
            __instance.TappedLeft = !__instance.DPadLeft;
            __instance.DPadLeft = true;
        } else {
            __instance.DPadRight = false;
            __instance.DPadLeft = false;
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            float horizontal = Input.GetAxisRaw("Horizontal");

            if (horizontal > 0.5f) {
                __instance.TappedRight = !__instance.StickRight;
                __instance.StickRight = true;
            } else if (horizontal < -0.5f) {
                __instance.TappedLeft = !__instance.StickLeft;
                __instance.StickLeft = true;
            } else {
                __instance.StickRight = false;
                __instance.StickLeft = false;
            }
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.5f && Mathf.Abs(dpadX) < 0.5f) {
            __instance.TappedRight = false;
            __instance.TappedLeft = false;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.5f && Mathf.Abs(dpadY) < 0.5f) {
            __instance.TappedUp = false;
            __instance.TappedDown = false;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            __instance.TappedUp = true;
            NoStick(__instance);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            __instance.TappedDown = true;
            NoStick(__instance);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            __instance.TappedLeft = true;
            NoStick(__instance);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            __instance.TappedRight = true;
            NoStick(__instance);
        }

        // Handle input wrapper events
        if (InputWrapper.IsUpRaised = __instance.TappedUp)
            InputWrapper.RaiseUp();
        if (InputWrapper.IsDownRaised = __instance.TappedDown)
            InputWrapper.RaiseDown();
        if (InputWrapper.IsLeftRaised = __instance.TappedLeft)
            InputWrapper.RaiseLeft();
        if (InputWrapper.IsRightRaised = __instance.TappedRight)
            InputWrapper.RaiseRight();

        if (InputWrapper.IsSubmitRaised = (Input.GetButtonDown(InputNames.Xbox_A) || Input.GetKeyDown(KeyCode.Return)))
            InputWrapper.RaiseSubmit();

        if (InputWrapper.IsCancelRaised = (Input.GetButtonDown(InputNames.Xbox_B) || Input.GetKeyDown(KeyCode.Escape)))
            InputWrapper.RaiseCancel();

        if (InputWrapper.IsStartRaised = (Input.GetButtonDown(InputNames.Xbox_Start) || Input.GetKeyDown(KeyCode.Pause)))
            InputWrapper.RaiseStart();

        // Handle custom input bindings
        InputBinder.UpdateInput();

        return false;
    }

    private static void NoStick(InputManagerScript inst) {
        inst.StickUp = false;
        inst.StickDown = false;
        inst.StickLeft = false;
        inst.StickRight = false;
    }
}
