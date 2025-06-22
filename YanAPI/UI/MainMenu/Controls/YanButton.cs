using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using YanAPI.UI.Base;
using YanAPI.Utils;

namespace YanAPI.UI.MainMenu.Controls {
    public class YanButton : Root {
        public Canvas CanvasComponent { get; private set; }

        public YanButton(Transform parent, string buttonText, UnityAction listener) {
            gameObject = new($"YanButton_{buttonText}");
            transform = gameObject.transform;

            CanvasComponent = gameObject.AddComponent<Canvas>();
            CanvasComponent.sortingOrder = 1;
            TMProCompnt = gameObject.CreateNewChild("Text_TMP").AddComponent<TMPro.TextMeshProUGUI>();
            
        }
    }
}
