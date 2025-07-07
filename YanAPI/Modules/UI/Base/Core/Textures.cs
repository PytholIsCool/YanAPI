using BepInEx;
using System.IO;
using System.Linq;
using UnityEngine;
using YanAPI.Core.Cache;
using YanAPI.Logging;
using YanAPI.Modules.Utils.Extensions;
using Object = UnityEngine.Object;

#nullable enable
namespace YanAPI.Modules.UI.Base.Core; 
internal static class Textures {
    public const string GUISheetName = "GUI";

    internal static bool CacheGUISpriteSheet() {
        if (CacheProperties.GUISpriteSheet != null)
            return true;

        string path = Path.Combine(Paths.PluginPath, "pythol-YanAPI/Assets/Textures/GUI.png");
        Texture loadedTexture = ImageUtils.LoadTextureFromDisk(path);
        if (loadedTexture == null)
            return false;

        foreach (var sprite in Object.FindObjectsByType<UISprite>(FindObjectsSortMode.None)) {
            if (sprite.atlas == null || sprite.mainTexture?.name != GUISheetName)
                continue;

            var clonedMaterial = Object.Instantiate(sprite.material);
            clonedMaterial.name = "GUISpriteSheet_Material";
            clonedMaterial.mainTexture = loadedTexture;
            Object.DontDestroyOnLoad(clonedMaterial);

            if (sprite.atlas is not UIAtlas originalAtlas) {
                CLogs.LogWarning("Could not clone atlas: not a UIAtlas.");
                return false;
            }

            GameObject atlasObj = new("GUISpriteSheet_AtlasObj");
            Object.DontDestroyOnLoad(atlasObj);

            var newAtlas = atlasObj.AddComponent<UIAtlas>();
            newAtlas.spriteMaterial = clonedMaterial;
            newAtlas.pixelSize = originalAtlas.pixelSize;
            newAtlas.replacement = null;

            newAtlas.spriteList = [];
            foreach (var data in originalAtlas.spriteList) {
                var copy = new UISpriteData {
                    name = data.name,
                    x = data.x,
                    y = data.y,
                    width = data.width,
                    height = data.height,
                    borderLeft = data.borderLeft,
                    borderRight = data.borderRight,
                    borderTop = data.borderTop,
                    borderBottom = data.borderBottom,
                    paddingLeft = data.paddingLeft,
                    paddingRight = data.paddingRight,
                    paddingTop = data.paddingTop,
                    paddingBottom = data.paddingBottom
                };
                newAtlas.spriteList.Add(copy);
            }

            GameObject dummyObj = new("GUISpriteSheet_UISprite");
            Object.DontDestroyOnLoad(dummyObj);
            var dummySprite = dummyObj.AddComponent<UISprite>();

            dummySprite.atlas = newAtlas;
            dummySprite.spriteName = newAtlas.spriteList.FirstOrDefault()?.name ?? "";
            dummySprite.material = clonedMaterial;
            dummySprite.MarkAsChanged();

            CacheProperties.GUISpriteSheet = loadedTexture;
            CacheProperties.UISpriteReference = dummySprite;

            CLogs.LogInfo("Successfully loaded and cached external GUI sprite sheet.");
            return true;
        }

        CLogs.LogWarning("Could not find UISprite to clone atlas data from.");
        return false;
    }

}
