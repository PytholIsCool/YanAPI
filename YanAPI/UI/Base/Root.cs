using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YanAPI.UI.Base; 
public class Root {
    public Transform transform { get; internal set; }
    public GameObject gameObject { get; internal set; }
    public UILabel UILabelCompnt { get; internal set; }
    private string Text_Internal { get; set; }
    public string Text {
        get => Text_Internal; set {
            Text_Internal = value;
            if (UILabelCompnt != null)
                UILabelCompnt.text = Text_Internal;
        }
    }
    private int FontSize_Internal { get; set; }
    public int FontSize {
        get => FontSize_Internal; set {
            FontSize_Internal = value;
            if (UILabelCompnt != null)
                UILabelCompnt.fontSize = FontSize_Internal;
        }
    }
    public Action OnClickedAction;

    public virtual void Destroy() {
        if (gameObject != null)
            Object.Destroy(gameObject);
    }
}