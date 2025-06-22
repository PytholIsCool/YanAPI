using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using YanAPI.Utils;
using YanAPI.Wrappers;

namespace YanAPI.UI.MainMenu; 
public static class MMUIModuleBase {

    public static Transform MMPanel { get; private set; }
    public static VerticalLayoutGroup MMPVLytGrp { get; private set; }

    internal static void Init() {
        if (!GameWrapper.IsInMainMenu())
            return;

        // Main Menu Panel setup
        //MMPanel = GameObject.Find("MainMenuPanel/TitleMenu").transform;
        //MMPVLytGrp = MMPanel.CreateNewChild("YanAPI_VerticalLayoutGroup").gameObject.AddComponent<VerticalLayoutGroup>();
        //MMPanel.MoveChildrenToExcept(MMPVLytGrp.transform, "Highlight");

        // Settings Setup
    }
}
