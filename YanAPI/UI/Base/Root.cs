using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;

namespace YanAPI.UI.Base; 
public class Root {
    public Transform transform { get; internal set; }
    public GameObject gameObject { get; internal set; }
    public TextMeshProUGUI TMProCompnt { get; internal set; }
    private string Text_Internal { get; set; }
    public string Text {
        get => Text_Internal; set {
            Text_Internal = value;
            if (TMProCompnt != null)
                TMProCompnt.text = Text_Internal;
        }
    }
    private float FontSize_Internal { get; set; }
    public float FontSize {
        get => FontSize_Internal; set {
            FontSize_Internal = value;
            if (TMProCompnt != null)
                TMProCompnt.fontSize = FontSize_Internal;
        }
    }
    public Button ButtonCompnt { get; internal set; }
    public UnityAction Listener { get; internal set; }

    public virtual void Destroy() {
        if (gameObject != null)
            Object.Destroy(gameObject);
    }
}