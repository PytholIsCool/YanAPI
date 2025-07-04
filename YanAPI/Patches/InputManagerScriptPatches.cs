using HarmonyLib;
using UnityEngine;
using YanAPI.Logging;

namespace YanAPI.Patches; 
internal static class InputManagerScriptPatches {
    internal static void Apply() {
        var harmony = new Harmony("YanAPI.InputManagerScriptPatches");

        harmony.Patch(AccessTools.Method(typeof(InputManagerScript), "Update"), prefix: new(typeof(InputManagerScriptPatches), nameof(Update)));
        CLogs.LogInfo("InputManagerScript.Update patch applied successfully.");
    }

    private static bool Update(InputManagerScript __instance) {
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

        if (__instance.TappedUp) Wrappers.InputWrapper.RaiseUp();
        if (__instance.TappedDown) Wrappers.InputWrapper.RaiseDown();
        if (__instance.TappedLeft) Wrappers.InputWrapper.RaiseLeft();
        if (__instance.TappedRight) Wrappers.InputWrapper.RaiseRight();

        if (Input.GetButtonDown(InputNames.Xbox_A) || Input.GetKeyDown(KeyCode.Return))
            Wrappers.InputWrapper.RaiseSubmit();

        if (Input.GetButtonDown(InputNames.Xbox_B) || Input.GetKeyDown(KeyCode.Escape))
            Wrappers.InputWrapper.RaiseCancel();

        if (Input.GetButtonDown(InputNames.Xbox_Start) || Input.GetKeyDown(KeyCode.Pause))
            Wrappers.InputWrapper.RaiseStart();

        return false;
    }

    private static void NoStick(InputManagerScript inst) {
        inst.StickUp = false;
        inst.StickDown = false;
        inst.StickLeft = false;
        inst.StickRight = false;
    }
}
