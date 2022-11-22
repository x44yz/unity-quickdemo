using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace QuickDemo
{
    public class UIWidget : MonoBehaviour
    {
        // NOTE:@k021Ng
        // Why not cache it while editing?
        // Because the editor can change UIElement name at any time and forget cache, the element cache
        // with UIElement name key will be invalid
        public Dictionary<string, UIElement> elements = new Dictionary<string, UIElement>();

        public static string GetElementMapKey(UIElement t)
        {
            return GetElementMapKey(t.elementType, t.refName);
        }

        public static string GetElementMapKey(UIElementType elementType, string refName)
        {
            return ((int)elementType) + refName;
        }

        private void Awake() 
        {
            // only called when actived
            CacheUIElements(elements);
        }

        public T Get<T>(string name) where T : UnityEngine.Object
        {
            UIElement ret = null;
            string key = GetElementMapKey(UIElement.ObjType2UIElementType(typeof(T)), name);

            if (elements.Count == 0 && gameObject.activeInHierarchy == false)
            {
                Debug.LogError($"[UI]cant get because elements not cache, make sure its Parenet visible");
                return null;
            }

            if (elements.TryGetValue(key, out ret) == false)
            {
                Debug.LogError($"[UI]cant get ui element > {typeof(T)} - {name}");
                return null;
            }
            return (T)ret.obj;
        }

        public string CacheUIElements(Dictionary<string, UIElement> cache, bool isErrLog = false)
        {
            string err = isErrLog ? "" : null;

            var ts = transform.GetComponentsInChildren<UIElement>(true);
            foreach(var t in ts)
            {
                if (t == null)
                    continue;

                // NOTE:
                // if contain other UIWidget, dont cache its UIElements
                // TODO: performance optimize
                if (t.GetComponentInParent<UIWidget>() != this)
                    continue;

                if (t.obj == null)
                {
                    Debug.LogError($"[UI]ui element obj is null {t.refName} at {Utils.GetHierarchyPath(t.transform)}");
                    continue;
                }

                string key = UIWidget.GetElementMapKey(t);
                if (cache.ContainsKey(key))
                {
                    Debug.LogError($"[UI]duplicated ui element ref name {t.refName} at {Utils.GetHierarchyPath(t.transform)}");
                    
    #if UNITY_EDITOR
                    if (isErrLog)
                    {
                        err += $"{t.refName} at {Utils.GetHierarchyPath(t.transform)}\n";
                    }
    #endif
                    continue;
                }

                cache[key] = t;
            }
            return err;
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UIWidget))]
    public class UIWidgetEditor : UnityEditor.Editor
    {
        Dictionary<string, UIElement> tmpElements = new Dictionary<string, UIElement>();
        
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();

            UIWidget widget = target as UIWidget;

            foreach (var kv in widget.elements)
            {
                if (kv.Value == null)
                    continue;

                EditorGUILayout.ObjectField(kv.Value.refName, kv.Value, typeof(UIElement), true);
            }

            if (Application.isPlaying == false)
            {
                foreach (var kv in tmpElements)
                {
                    if (kv.Value == null)
                        continue;

                    EditorGUILayout.ObjectField(kv.Value.refName, kv.Value, typeof(UIElement), true);
                }

                if (GUILayout.Button("CHECK"))
                {
                    string err = widget.CacheUIElements(tmpElements, true);
                    if (string.IsNullOrEmpty(err) == false)
                    {
                        EditorUtility.DisplayDialog("ERROR", "CHECK failed because duplicated ui element ref name: " + err, "OK");
                    }
                }
            }
        }
    }
    #endif
}