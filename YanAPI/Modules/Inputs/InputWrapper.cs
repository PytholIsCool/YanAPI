using System;

namespace YanAPI.Modules.Inputs; 
internal static class InputWrapper {
    public static event Action OnUp;
    public static bool IsUpRaised { get; internal set; } = false;
    public static event Action OnDown;
    public static bool IsDownRaised { get; internal set; } = false;
    public static event Action OnLeft;
    public static bool IsLeftRaised { get; internal set; } = false;
    public static event Action OnRight;
    public static bool IsRightRaised { get; internal set; } = false;

    public static event Action OnSubmit;
    public static bool IsSubmitRaised { get; internal set; } = false;
    public static event Action OnCancel;
    public static bool IsCancelRaised { get; internal set; } = false;
    public static event Action OnStart;
    public static bool IsStartRaised { get; internal set; } = false;

    internal static void RaiseUp() => OnUp?.Invoke();
    internal static void RaiseDown() => OnDown?.Invoke();
    internal static void RaiseLeft() => OnLeft?.Invoke();
    internal static void RaiseRight() => OnRight?.Invoke();

    internal static void RaiseSubmit() => OnSubmit?.Invoke();
    internal static void RaiseCancel() => OnCancel?.Invoke();
    internal static void RaiseStart() => OnStart?.Invoke();
}