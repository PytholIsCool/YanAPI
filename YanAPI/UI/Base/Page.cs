using System.Collections.Generic;
using UnityEngine;

namespace YanAPI.UI.Base {
    public class Page : MonoBehaviour {
        public bool isOpen { get; internal set; } = false;
        public UIPanel UIPanelCompnt { get; internal set; }
        public virtual void OpenPage() { }
        public virtual void ClosePage() { }
    }
}
