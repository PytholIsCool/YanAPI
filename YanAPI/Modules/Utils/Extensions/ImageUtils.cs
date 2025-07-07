using System.IO;
using UnityEngine;
using YanAPI.Logging;

namespace YanAPI.Modules.Utils.Extensions;
internal static class ImageUtils {
    public static Texture LoadTextureFromDisk(string path) {
        if (!File.Exists(path)) {
            CLogs.LogError($"Texture file not found: {path}");
            return null!;
        }

        byte[] fileData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(fileData);
        tex.name = Path.GetFileNameWithoutExtension(path);
        return tex;
    }

}
