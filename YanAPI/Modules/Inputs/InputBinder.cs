using System;
using System.Collections.Generic;

#nullable enable
namespace YanAPI.Modules.Inputs;

internal static class InputBinder {
    private static readonly Dictionary<string, InputAction> _actions = [];
    private static readonly HashSet<string> _lastFrameTriggered = [];

    public static event Action<string>? OnInputTriggered;

    /// <summary>
    /// Registers a new logical input with optional keyboard and gamepad bindings.
    /// </summary>
    public static void Register(string actionName, InputBinding? keyboard = null, InputBinding? gamepad = null) {
        if (string.IsNullOrWhiteSpace(actionName))
            throw new ArgumentException("Action name cannot be null or empty.", nameof(actionName));

        var action = new InputAction {
            Name = actionName,
            KeyboardBinding = keyboard,
            GamepadBinding = gamepad
        };

        _actions[actionName] = action;
    }

    /// <summary>
    /// Checks whether the given action is currently triggered.
    /// </summary>
    public static bool Get(string actionName) {
        return _actions.TryGetValue(actionName, out var action) && action.IsTriggered();
    }

    internal static void UpdateInput() {
        foreach (var pair in _actions) {
            var name = pair.Key;
            var action = pair.Value;

            bool triggered = action.IsTriggered();
            if (triggered && !_lastFrameTriggered.Contains(name)) {
                _lastFrameTriggered.Add(name);
                OnInputTriggered?.Invoke(name);
            } else if (!triggered) {
                _lastFrameTriggered.Remove(name);
            }
        }
    }
}