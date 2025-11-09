using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static partial class Utils
{
    private static StringBuilder sb = new StringBuilder();

    public static Color SetAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    public static string GetHierarchyPath(Transform tf)
    {
        sb.Clear();
        sb.Append(tf.name);

        var parent = tf.parent;
        while (parent)
        {
            sb.Insert(0, "\\");
            sb.Insert(0, parent.name);
            parent = parent.parent;
        }

        return sb.ToString();
    }

    public static T GetComponentOnlyInParent<T>(Component component) where T : Component
    {
        var parent = component.transform.parent;
        while (parent != null)
        {
            var t = parent.GetComponent<T>();
            if (t != null)
            {
                return t;
            }
            parent = parent.parent;
        }
        return null;
    }

    public static void SetLayerRecursively(this UnityEngine.GameObject root, int layer)
    {
        root.gameObject.layer = layer;
        for (int i = 0; i < root.transform.childCount; ++i)
        {
            var ch = root.transform.GetChild(i);
            SetLayerRecursively(ch.gameObject, layer);
        }
    }

    public static void DelaySeconds(this MonoBehaviour mb, float seconds, System.Action cb)
    {
        mb.StartCoroutine(_DelaySeconds(seconds, cb));
    }

    public static void DelayOneFrame(this MonoBehaviour mb, System.Action cb)
    {
        mb.StartCoroutine(_DelayOneFrame(cb));
    }

    private static IEnumerator _DelaySeconds(float seconds, System.Action cb)
    {
        yield return new WaitForSeconds(seconds);
        cb.Invoke();
    }

    public static IEnumerator _DelayOneFrame(System.Action cb)
    {
        yield return new WaitForEndOfFrame();
        cb.Invoke();
    }

    // 这个不区分 PhysicsRaycaster 和 GraphicsRaycaster
    // 所以当 UI 遮挡在可点击的 3d 物品前，会穿透
    public static bool IsPointerOverGameObject()
    {
        // 当场景中有 PhysicsRaycaster 时候，两者都能触发  
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            PointerEventData pe = new PointerEventData(EventSystem.current);
            pe.position = Input.mousePosition;
            List<RaycastResult> hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pe, hits);
            return hits.Count > 0;
        }
    }

    public static bool IsPointerOverUI(GraphicRaycaster raycaster)
    {
        // 当场景中有 PhysicsRaycaster 时候，两者都能触发  
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = Input.mousePosition;
        List<RaycastResult> hits = new List<RaycastResult>();
        raycaster.Raycast(pe, hits);
        return hits.Count > 0;
    }

    public static void GUILayoutLabel(string text, int size = 40, float height = 70)
    {
        GUILayout.Label(GUIText(text, size), GUIOptions(height));
    }

    public static bool GUILayoutButton(string text, int size = 40, float height = 70)
    {
        return GUILayout.Button(GUIText(text, size), GUIOptions(height));
    }

    public static string GUIText(string text, int size = 40)
    {
        return $"<size={size}>{text}</size>";
    }

    public static GUILayoutOption[] GUIOptions(float height)
    {
        return new GUILayoutOption[]{
            GUILayout.Height(height)
        };
    }

    public static GUILayoutOption[] GUIDefaultButtonOptions
    {
        get { return GUIOptions(70); }
    }

    public static GUILayoutOption[] GUIDefaultLabelOptions
    {
        get { return GUIOptions(70); }
    }

    public static string GetGameObjectPath(GameObject obj)
    {
        if (obj == null)
            return string.Empty;

        sb.Clear();
        sb.Insert(0, obj.transform.name);
        var parent = obj.transform.parent;
        while (parent != null)
        {
            sb.Insert(0, "/");
            sb.Insert(0, parent.name);
            parent = parent.parent;
        }
        return sb.ToString();
    }

    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    public static Bounds GetBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0)
        {
            Debug.LogError("cant find any renders on obj");
            return new Bounds();
        }

        Bounds compositeBounds = new Bounds(renderers[0].bounds.center, Vector3.zero);
        foreach (Renderer renderer in renderers)
        {
            compositeBounds.Encapsulate(renderer.bounds);
        }

        Debug.Log("Composite Bounds Center: " + compositeBounds.center);
        Debug.Log("Composite Bounds Size: " + compositeBounds.size);
        Debug.Log("Composite Bounds Min: " + compositeBounds.min);
        Debug.Log("Composite Bounds Max: " + compositeBounds.max);
        return compositeBounds;
    }

}