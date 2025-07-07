using UnityEngine;
using YanAPI.Core.Cache;
using YanAPI.Logging;
using Object = UnityEngine.Object;

#nullable enable
namespace YanAPI.Modules.UI.Base.Core {
    internal static class Fonts {

        internal static bool CacheFuturaCondensed() {
            if (CacheProperties.FuturaCondensed != null)
                return true;

            foreach (var label in Object.FindObjectsByType<UILabel>(FindObjectsSortMode.None)) {
                if (!label.ambigiousFont.name.Contains("Futura Condensed Medium"))
                    continue;
                CacheProperties.FuturaCondensed = label.trueTypeFont;
                Object.DontDestroyOnLoad(CacheProperties.FuturaCondensed);
                CacheProperties.FuturaCondensed.name = "Futura Condensed Medium";

                CLogs.LogInfo($"Cached reference to Futura Condensed dynamic font: {CacheProperties.FuturaCondensed.name}");
                return true;
            }
            return false;
        }
    }
}