using System;
using UnityEngine;
using YanAPI.UI.Base;
using YanAPI.UI.Base.Assets;
using YanAPI.Wrappers;
using Object = UnityEngine.Object;

namespace YanAPI.UI.MainMenu.Controls; 
public class YanLabel : Root {
    public bool IsSelected { get; set; } = false;
    public YanLabel(YanPage parent, string buttonText, Action listener = null) {
        if (parent == null)
            throw new ArgumentNullException(nameof(parent), "YanPage is null.");
        Create(parent.UIPanelCompnt, buttonText, listener);
    }

    private void Create(UIPanel parent, string buttonText, Action listener = null) {
        gameObject = new($"YanAPI.UI.YanLabel.{buttonText}");
        transform = gameObject.transform;
        transform.SetParent(parent.transform, false);

        UILabelCompnt = gameObject.AddComponent<UILabel>();
        UILabelCompnt.text = buttonText;
        UILabelCompnt.ambigiousFont = Fonts.FuturaCondensed;
        UILabelCompnt.fontSize = 50;
        UILabelCompnt.color = Color.white;
        UILabelCompnt.effectColor = new Color(1f, 0.5f, 1f, 1f);
        UILabelCompnt.effectStyle = UILabel.Effect.Outline8;
        UILabelCompnt.effectDistance = new(2f, 2f);
        UILabelCompnt.gradientBottom = new Color(0.7f, 0.7f, 0.7f, 1f);
        UILabelCompnt.gradientTop = Color.white;
        UILabelCompnt.alignment = NGUIText.Alignment.Automatic;
        UILabelCompnt.overflowMethod = UILabel.Overflow.ShrinkContent;
        UILabelCompnt.gameObject.layer = parent.gameObject.layer;
        UILabelCompnt.width = 400;
        UILabelCompnt.height = 75;
        UILabelCompnt.depth = 1;

        parent.transform.GetComponent<YanPage>()?.RegisterButton(this);
        parent.AddWidget(UILabelCompnt);

        OnClickedAction = listener;

        InputWrapper.OnSubmit += () => {
            if (IsSelected == true)
                OnClickedAction?.Invoke();
        };
    }

}
