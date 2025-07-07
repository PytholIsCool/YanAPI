using UnityEngine;

#nullable enable
namespace YanAPI.Core.Cache; 
internal static class CacheProperties {
    // UI
    public static Font? FuturaCondensed { get; internal set; } = null;
    public static Texture? GUISpriteSheet { get; internal set; } = null;
    public static UISprite? UISpriteReference { get; internal set; } = null;
    public static GameObject? UIPageCursorPrefab { get; internal set; } = null;
}
