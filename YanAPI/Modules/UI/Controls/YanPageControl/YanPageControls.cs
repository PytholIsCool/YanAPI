using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YanAPI.Modules.UI.Controls; 
public partial class YanPage {
    public int Selection { get; internal set; } = 0;

    public void HandleSelection() {
        if (Buttons.Count == 0)
            return;

        // Wrap around
        if (Selection < 0)
            Selection = Buttons.Count - 1;
        else if (Selection >= Buttons.Count)
            Selection = 0;
    }
}