using System;
using UnityEngine;
using YanAPI.Core.Cache;
using YanAPI.Modules.Inputs;
using YanAPI.Modules.UI.Base;

// For better ease of use, the namespace is cut off at controls
namespace YanAPI.Modules.UI.Controls;
public class YanLabel : Root {
    public bool IsSelected { get; internal set; } = false;
    public YanLabel(YanPage parent, string buttonText, Action listener = null) {
        if (parent == null)
            throw new ArgumentNullException(nameof(parent), "YanPage is null.");
        if (!UIAssetManager.IsReady())
            throw new NullReferenceException($"Could not find the needed assets for the following UI control: {nameof(YanLabel)}");
        Create(parent, buttonText, listener);
    }

    private void Create(YanPage parent, string buttonText, Action listener = null) {
        GameObject gObj = new($"YanAPI.UI.YanLabel.{buttonText}");
        SetCore(gObj, gObj.AddComponent<UILabel>());
        transform.SetParent(parent.transform, false);

        SetDefaultProperties(parent.UIPanelCompnt, buttonText);

        parent.RegisterButton(this);
        parent.UIPanelCompnt.AddWidget(UILabelCompnt);

        OnClickedAction = listener;

        InputWrapper.OnSubmit += () => {
            if (IsSelected == true)
                OnClickedAction?.Invoke();
        };
    }

    private void SetDefaultProperties(UIPanel parent, string buttonText) {
        UILabelCompnt.text = buttonText;
        UILabelCompnt.ambigiousFont = CacheProperties.FuturaCondensed;
        UILabelCompnt.fontSize = 50;
        UILabelCompnt.color = Color.white;
        UILabelCompnt.effectColor = new Color(1f, 0.5f, 1f, 1f);
        UILabelCompnt.effectStyle = UILabel.Effect.Outline8;
        UILabelCompnt.effectDistance = new(2f, 2f);
        UILabelCompnt.gradientBottom = new Color(0.7f, 0.7f, 0.7f, 1f);
        UILabelCompnt.gradientTop = Color.white;
        UILabelCompnt.alignment = NGUIText.Alignment.Automatic;
        UILabelCompnt.overflowMethod = UILabel.Overflow.ShrinkContent;
        UILabelCompnt.gameObject.layer = LayerMask.NameToLayer("UI");
        UILabelCompnt.width = 400;
        UILabelCompnt.height = 75;
        UILabelCompnt.depth = 1;
    }

}
