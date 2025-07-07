#nullable enable
namespace YanAPI.Modules.Inputs; 
internal class InputAction {
    public string? Name;
    public InputBinding? KeyboardBinding;
    public InputBinding? GamepadBinding;

    public bool IsTriggered() {
        return (KeyboardBinding?.IsTriggered() ?? false)
            || (GamepadBinding?.IsTriggered() ?? false);
    }
}