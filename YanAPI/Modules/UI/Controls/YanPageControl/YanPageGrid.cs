namespace YanAPI.Modules.UI.Controls; 
public partial class YanPage {
    public UIGrid.Arrangement arrangement {
        get => UIGridCompnt.arrangement;
        set => UIGridCompnt.arrangement = value;
    }
    public UIWidget.Pivot pivot {
        get => UIGridCompnt.pivot;
        set => UIGridCompnt.pivot = value;
    }
    public float cellHeight {
        get => UIGridCompnt.cellHeight;
        set => UIGridCompnt.cellHeight = value;
    }
    public float cellWidth {
        get => UIGridCompnt.cellWidth;
        set => UIGridCompnt.cellWidth = value;
    }
    public void UpdateGrid() => UIGridCompnt.Reposition();
}