using System;
using UnityEngine;
using Object = UnityEngine.Object;

#nullable enable
namespace YanAPI.Modules.UI.Base;
public class Root {
    public Transform transform { get; private set; } = null!;
    public GameObject gameObject { get; private set; } = null!;
    public UILabel UILabelCompnt { get; private set; } = null!;

    protected void SetCore(GameObject go, UILabel label) {
        gameObject = go;
        transform = go.transform;
        UILabelCompnt = label;
    }
    private string? Text_Internal { get; set; }
    public string Text {
        get => Text_Internal ??= ""; set {
            Text_Internal = value;
            if (UILabelCompnt != null)
                UILabelCompnt.text = Text_Internal;
        }
    }
    private int FontSize_Internal { get; set; } = 50;
    public int FontSize {
        get => FontSize_Internal; set {
            FontSize_Internal = value;
            if (UILabelCompnt != null)
                UILabelCompnt.fontSize = FontSize_Internal;
        }
    }
    public Action? OnClickedAction;

    public virtual void Destroy() {
        if (gameObject != null)
            Object.Destroy(gameObject);
    }
}
