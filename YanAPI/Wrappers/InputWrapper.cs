using System;

namespace YanAPI.Wrappers {
    internal static class InputWrapper {
        // Used by InputManagerScriptPatches.Update

        public static event Action OnUp;
        public static event Action OnDown;
        public static event Action OnLeft;
        public static event Action OnRight;

        public static event Action OnSubmit;
        public static event Action OnCancel;
        public static event Action OnStart;

        internal static void RaiseUp() => OnUp?.Invoke();
        internal static void RaiseDown() => OnDown?.Invoke();
        internal static void RaiseLeft() => OnLeft?.Invoke();
        internal static void RaiseRight() => OnRight?.Invoke();

        internal static void RaiseSubmit() => OnSubmit?.Invoke();
        internal static void RaiseCancel() => OnCancel?.Invoke();
        internal static void RaiseStart() => OnStart?.Invoke();
    }
}
