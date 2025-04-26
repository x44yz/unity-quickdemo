#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public static class EditorUtils
{
    // 只能针对 object asset
    public static void RenameObjectAsset(UnityEngine.Object asset, string name)
    {
        asset.name = name;
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void RenameAsset(UnityEngine.Object asset, string name)
    {
        try
        {
            if (EditorUtility.IsDirty(asset))
            {
                Debug.LogError($"asset is dirty, please save");
                return;
            }

            var assetPath = AssetDatabase.GetAssetPath(asset);
            var postfix = assetPath.Substring(assetPath.LastIndexOf("."));
            var nAssetPath = assetPath.Remove(assetPath.LastIndexOf("/") + 1) + name + postfix;

            var path = Application.dataPath + "/" + assetPath.Substring("Assets/".Length);
            var npath = Application.dataPath + "/" + nAssetPath.Substring("Assets/".Length);
            
            // Debug.Log($"xx-- path > {path}");
            // Debug.Log($"xx-- npath > {npath}");
            File.Move(path, npath);
            AssetDatabase.Refresh();

            asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(nAssetPath);
            Selection.activeObject = asset;
            asset.name = name;
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"success rename asset > {name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"failed rename asset > {ex}");
        }
    }
}
#endif