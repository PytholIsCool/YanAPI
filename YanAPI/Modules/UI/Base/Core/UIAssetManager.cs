using YanAPI.Modules.UI.Base.Core;

internal static class UIAssetManager {
    internal static bool areCoreAssetsReady { get; private set; } = false;

    internal static bool IsReady() => areCoreAssetsReady || Init();
    internal static bool Init() {
        if (areCoreAssetsReady == true)
            return true;

        areCoreAssetsReady = Fonts.CacheFuturaCondensed();
        areCoreAssetsReady = Textures.CacheGUISpriteSheet();
        areCoreAssetsReady = CursorManager.BuildCursorPrefab();

        return areCoreAssetsReady;
    }
}