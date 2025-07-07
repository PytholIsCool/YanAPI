using UnityEngine;
using YanAPI.Modules.Inputs;

namespace YanAPI.Modules.UI.Controls.YanPageControl; 
public class YanPageComponent : MonoBehaviour {
    internal YanPage Page;
    private void Update() {
        if (Page == null)
            return;

        // Rotation animation
        Page.CursorSprite.transform.Rotate(Vector3.right * (Time.deltaTime * 100f), Space.Self);

        // Inputs
        if (InputWrapper.IsUpRaised) {
            Page.Selection--;
            Page.HandleSelection();
        }
        if (InputWrapper.IsDownRaised) {
            Page.Selection++;
            Page.HandleSelection();
        }

        // Selection
        Page.UpdateCursorPosition();
    }
}