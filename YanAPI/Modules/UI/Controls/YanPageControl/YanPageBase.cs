using System;
using System.Collections.Generic;
using UnityEngine;
using YanAPI.Modules.UI.Base;
using YanAPI.Modules.UI.Controls.YanPageControl;

// For better ease of use, the namespace is cut off at controls
#nullable enable
namespace YanAPI.Modules.UI.Controls;
public partial class YanPage : Page {
    public static List<Page> pageIndex { get; internal set; } = [];
    public List<YanLabel> Buttons { get; internal set; } = [];
    internal YanPageComponent YanPageCompnt { get; set; } = null!;

    public YanPage(string pageName, Transform? parent = null) {
        if (!UIAssetManager.IsReady())
            throw new NullReferenceException($"Could not find the needed assets for the following UI control: {nameof(YanPage)}");
        Create(pageName, parent);
    }

    private void Create(string pageName, Transform? parent = null) {
        var go = new GameObject($"YanAPI.UI.YanPage.{pageName}_{Guid.NewGuid()}") {
            layer = LayerMask.NameToLayer("UI")
        };
        if (parent != null) {
            go.transform.SetParent(parent, false);
            parent.gameObject.layer = LayerMask.NameToLayer("UI");
        }

        SetCore(go, go.AddComponent<UIPanel>(), go.AddComponent<UIGrid>());
        InitializeCursor();
        go.AddComponent<YanPageComponent>().Page = this;

        SetCursorVisible(true); // Debug

        if (Buttons.Count > 0) {
            Selection = 0;
            CursorSprite.transform.position = Buttons[0].transform.position + Vector3.left * 100f;
        }
    }

    public override void OpenPage() {
        if (UIPanelCompnt == null)
            throw new NullReferenceException("UIPanel component is null. Make sure to initialize the page correctly.");
        if (isOpen == true)
            return;
        // TODO: Add logic to hide other pages if needed
        UIPanelCompnt.enabled = true;
        pageIndex.Add(this);

        isOpen = true;
    }

    public override void ClosePage() {
        if (UIPanelCompnt == null)
            throw new NullReferenceException("UIPanel component is null. Make sure to initialize the page correctly.");
        if (isOpen == false)
            return;
        // TODO: Add logic to enable whatever page came before this one in the index (if any)
        UIPanelCompnt.enabled = false;
        pageIndex.Remove(this);

        isOpen = false;
    }

    internal void RegisterButton(YanLabel btn) {
        Buttons.Add(btn);
        CursorSprite.gameObject.SetActive(false);
        UIGridCompnt.Reposition();
        CursorSprite.gameObject.SetActive(true);
    }
}
