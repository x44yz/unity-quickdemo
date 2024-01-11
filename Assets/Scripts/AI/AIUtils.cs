using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

public static class AIUtils
{
    public static string[] considerationTypes = new string[]{
        typeof(CStat).ToString(),
        typeof(CItemCount).ToString(),
    };

    public static string[] preconditionTypes = new string[]{
        typeof(PHasItem).ToString(),
    };

    public static void RemoveFromAsset(Object asset)
    {
        string mainAssetPath = AssetDatabase.GetAssetPath(asset);
        var mainAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(mainAssetPath);
        if (mainAsset.GetInstanceID() == asset.GetInstanceID())
        {
            Debug.LogWarning($"this main asset, cant remove");
            return;
        }

        AssetDatabase.RemoveObjectFromAsset(asset);
        // 延时执行，否则会导入警告
        EditorApplication.delayCall += ()=> {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };
    }
}