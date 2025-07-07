using System;
using UnityEngine;
using YanAPI.Core.Cache;
using YanAPI.Logging;
using Object = UnityEngine.Object;

namespace YanAPI.Modules.UI.Base.Core;
internal static class CursorManager {
    public static bool BuildCursorPrefab() {

        // Base
        var cursorObj = new GameObject("HeartCursor");
        var cursor = cursorObj.AddComponent<UISprite>();
        cursor.spriteName = "Heart";
        cursor.type = UIBasicSprite.Type.Sliced;
        cursor.depth = 1;
        cursor.atlas = CacheProperties.UISpriteReference.atlas;
        cursor.material = CacheProperties.UISpriteReference.material;
        cursor.width = 64;
        cursor.height = 64;
        cursor.color = new Color(1f, 1f, 1f, 1f);

        // Border
        var borderObj = new GameObject("HeartCursorBorder");
        borderObj.transform.SetParent(cursorObj.transform, false);
        var border = borderObj.AddComponent<UISprite>();
        border.spriteName = "Heart";
        border.type = UIBasicSprite.Type.Sliced;
        border.depth = 0;
        border.atlas = cursor.atlas;
        border.material = cursor.material;
        border.width = 74;
        border.height = 74;
        border.color = new Color(1f, 0.7529f, 1f, 1f);

        Object.DontDestroyOnLoad(cursorObj);
        cursorObj.SetActive(false);

        CacheProperties.UIPageCursorPrefab = cursorObj;
        CLogs.LogInfo("Successfully built cursor prefab!");

        return true;
    }

    public static UISprite CreateAndGetInstance(Transform parent, out UISprite borderSprite) {
        if (CacheProperties.UIPageCursorPrefab == null)
            throw new InvalidOperationException("Cursor prefab has not been built.");

        var cursorBase = Object.Instantiate(CacheProperties.UIPageCursorPrefab, parent, false);
        cursorBase.name = $"Cursor_Heart_{Guid.NewGuid()}";
        cursorBase.layer = LayerMask.NameToLayer("UI");

        var baseSprite = cursorBase.GetComponent<UISprite>();
        baseSprite.type = UIBasicSprite.Type.Sliced;
        baseSprite.depth = 1;

        var borderTransform = cursorBase.transform.Find("HeartCursorBorder") ?? throw new MissingComponentException("Expected child 'HeartCursorBorder' was not found.");
        borderSprite = borderTransform.GetComponent<UISprite>() ?? throw new MissingComponentException("Child 'HeartCursorBorder' does not have a UISprite component.");

        cursorBase.SetActive(true);
        baseSprite.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        return baseSprite;
    }

}