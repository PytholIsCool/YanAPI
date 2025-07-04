using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YanAPI.UI.Base;

namespace YanAPI.UI.MainMenu.Controls; 
public class YanPage : Page {

    public static List<Page> pageIndex = [];

    public UISprite CursorSprite;
    public List<YanLabel> Buttons = [];
    public int Selection = 0;

    public static YanPage Create(string pageName, Transform parent = null) {
        var go = new GameObject($"YanAPI.UI.YanPage.{pageName}_{Guid.NewGuid()}");
        if (parent != null)
            go.transform.SetParent(parent, false);

        var page = go.AddComponent<YanPage>();
        page.UIPanelCompnt = go.AddComponent<UIPanel>();

        return page;
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

    public void RegisterButton(YanLabel btn) {
        Buttons.Add(btn);
    }

}
