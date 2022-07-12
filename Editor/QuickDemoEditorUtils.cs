using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace QuickDemo.Editor
{
    public static class QuickDemoEditorUtils
    {
        // % - CTRL on Windows / CMD on OSX
        // # - Shift
        // & -Alt
        // LEFT/RIGHT/UP/DOWN - Arrow keys
        // F1 … F2 - F keys
        // HOME,END,PGUP,PGDN
        // letter: - _ + letter, like "_g"
        [MenuItem("QuickDemo/Object Forward", false)]
        public static void GetObjectForward()
        {
            if (Selection.activeGameObject == null)
            {
                Debug.LogWarning("[EDITOR]not active gameobject selected");
                return;
            }
            
            var fw = Selection.activeGameObject.transform.forward;
            Debug.Log($"[EDITOR]{fw} - select gameobject foward");
        }
    }
}
