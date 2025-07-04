using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YanAPI.Utils.Extensions;
public static class UnityExtensions {

    public static Transform CreateNewChild(this Transform parent, string name) {
        var newChild = new GameObject(name).transform;
        newChild.SetParent(parent, false);
        return newChild;
    }

    public static GameObject CreateNewChild(this GameObject parent, string name) {
        var newChild = new GameObject(name);
        newChild.transform.SetParent(parent.transform, false);
        return newChild;
    }

    public static void MoveChildrenTo(this Transform oldParent, Transform newParent) {
        if (oldParent == null || newParent == null)
            throw new ArgumentNullException("Source or target transform cannot be null.");

        for (int i = oldParent.childCount - 1; i >= 0; i--) {
            Transform child = oldParent.GetChild(i);
            child.SetParent(newParent, false);
        }
    }

    public static void MoveChildrenToExcept(this Transform oldParent, Transform newParent, Transform[] exceptions) {
        if (oldParent == null || newParent == null)
            throw new ArgumentNullException("Source or target transform cannot be null.");
        for (int i = oldParent.childCount - 1; i >= 0; i--) {
            Transform child = oldParent.GetChild(i);

            if (exceptions != null && exceptions.Contains(child))
                continue;

            child.SetParent(newParent, false);
        }
    }

    public static void MoveChildrenToExcept(this Transform oldParent, Transform newParent, Transform exception) {
        if (oldParent == null || newParent == null)
            throw new ArgumentNullException("Source or target transform cannot be null.");
        for (int i = oldParent.childCount - 1; i >= 0; i--) {
            Transform child = oldParent.GetChild(i);

            if (exception != null && exception == child)
                continue;

            child.SetParent(newParent, false);
        }
    }

    public static void MoveChildrenToExcept(this Transform oldParent, Transform newParent, GameObject[] exceptions) {
        if (oldParent == null || newParent == null)
            throw new ArgumentNullException("Source or target transform cannot be null.");
        for (int i = oldParent.childCount - 1; i >= 0; i--) {
            Transform child = oldParent.GetChild(i);

            if (exceptions != null && exceptions.Contains(child.gameObject))
                continue;

            child.SetParent(newParent, false);
        }
    }

    public static void MoveChildrenToExcept(this Transform oldParent, Transform newParent, GameObject exception) {
        if (oldParent == null || newParent == null)
            throw new ArgumentNullException("Source or target transform cannot be null.");
        for (int i = oldParent.childCount - 1; i >= 0; i--) {
            Transform child = oldParent.GetChild(i);

            if (exception != null && exception == child.gameObject)
                continue;

            child.SetParent(newParent, false);
        }
    }

    public static void MoveChildrenToExcept(this Transform oldParent, Transform newParent, string[] exceptionNames) {
        if (oldParent == null || newParent == null)
            throw new ArgumentNullException("Source or target transform cannot be null.");
        for (int i = oldParent.childCount - 1; i >= 0; i--) {
            Transform child = oldParent.GetChild(i);

            if (exceptionNames != null && exceptionNames.Contains(child.name))
                continue;

            child.SetParent(newParent, false);
        }
    }

    public static void MoveChildrenToExcept(this Transform oldParent, Transform newParent, string exceptionName) {
        if (oldParent == null || newParent == null)
            throw new ArgumentNullException("Source or target transform cannot be null.");
        for (int i = oldParent.childCount - 1; i >= 0; i--) {
            Transform child = oldParent.GetChild(i);

            if (exceptionName != null && exceptionName == child.name)
                continue;

            child.SetParent(newParent, false);
        }
    }

    public static void DumpInfo(this Transform transform) {
        if (transform == null) {
            Debug.Log("Transform is null.");
            return;
        }
        Debug.Log($"Transform Name: {transform.name}");
        Debug.Log($"Position: {transform.position}");
        Debug.Log($"Rotation: {transform.rotation}");
        Debug.Log($"Scale: {transform.localScale}");
        Debug.Log($"Parent: {(transform.parent != null ? transform.parent.name : "None")}");
        Debug.Log($"Child Count: {transform.childCount}");
        foreach (Transform child in transform) {
            Debug.Log($"Child: {child.name}");
        }
    }
}
