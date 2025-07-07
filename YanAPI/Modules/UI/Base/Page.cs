using UnityEngine;

#nullable enable
namespace YanAPI.Modules.UI.Base {
    public class Page {
        public Transform transform { get; private set; } = null!;
        public GameObject gameObject { get; private set; } = null!;
        public UIPanel UIPanelCompnt { get; internal set; } = null!;
        internal UIGrid UIGridCompnt { get; set; } = null!;
        public bool isOpen { get; internal set; } = false;

        protected void SetCore(GameObject go, UIPanel panel, UIGrid grid) {
            gameObject = go;
            transform = go.transform;
            UIPanelCompnt = panel;
            UIGridCompnt = grid;

            UIGridCompnt.arrangement = UIGrid.Arrangement.Vertical;
            UIGridCompnt.pivot = UIWidget.Pivot.Center;
            UIGridCompnt.hideInactive = true;
            UIGridCompnt.cellHeight = 70f;
            UIGridCompnt.cellWidth = 50f;
        }
        public virtual void OpenPage() { }
        public virtual void ClosePage() { }
    }
}
