using UnityEngine;

namespace YanAPI.Modules.Inputs; 
public abstract class InputBinding {
    public abstract bool IsTriggered();
}

public class KeyBinding : InputBinding {
    public KeyCode Key { get; }

    public KeyBinding(KeyCode key) => Key = key;

    public override bool IsTriggered() => Input.GetKeyDown(Key);
}

public class GamepadBinding : InputBinding {
    public string ButtonName { get; }

    public GamepadBinding(string buttonName) => ButtonName = buttonName;

    public override bool IsTriggered() => Input.GetButtonDown(ButtonName);
}