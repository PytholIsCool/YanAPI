using UnityEngine;
using YanAPI.Logging;

namespace YanAPI.UI.Base.Assets {
    internal static class Fonts {
        internal static Font FuturaCondensed = null;

        internal static void CacheFuturaCondensedFromReference(UILabel label) {
            if (label == null || label.trueTypeFont == null) {
                CLogs.LogError("Could not cache Futura Condensed font — label or trueTypeFont is null.");
                return;
            }

            FuturaCondensed = label.trueTypeFont;
            Object.DontDestroyOnLoad(FuturaCondensed);
            FuturaCondensed.name = "Futura Condensed";

            CLogs.LogInfo($"Cached reference to Futura Condensed dynamic font: {FuturaCondensed.name}");
        }
    }
}
