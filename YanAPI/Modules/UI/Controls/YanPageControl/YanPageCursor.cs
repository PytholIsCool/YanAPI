using UnityEngine;
using YanAPI.Modules.UI.Base.Core;

namespace YanAPI.Modules.UI.Controls;

public partial class YanPage {
    public UISprite CursorSprite { get; internal set; } = null!;
    private void InitializeCursor() {
        CursorSprite = CursorManager.CreateAndGetInstance(transform, out UISprite borderSprite);
        CursorSprite.name = "YanPage_CursorBase";
        borderSprite.name = "YanPage_CursorBorder";
        CursorSprite.gameObject.SetActive(false);

        UIPanelCompnt.AddWidget(CursorSprite);
        UIPanelCompnt.AddWidget(borderSprite);
    }
    internal void UpdateCursorPosition() {
        if (CursorSprite == null || Buttons.Count == 0)
            return;

        Selection = Mathf.Clamp(Selection, 0, Buttons.Count - 1);

        var selected = Buttons[Selection];
        Vector3 worldTarget = selected.transform.position + Vector3.left * 100f;

        Vector3 localTarget = CursorSprite.transform.parent.InverseTransformPoint(worldTarget);
        localTarget.z = 0f;

        CursorSprite.transform.localPosition = Vector3.Lerp(
            CursorSprite.transform.localPosition,
            localTarget,
            Time.deltaTime * 10f
        );
    }

    public void SetCursorVisible(bool visible) => CursorSprite?.gameObject.SetActive(visible);
}