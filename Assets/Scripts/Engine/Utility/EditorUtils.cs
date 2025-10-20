#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Threading.Tasks;

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

    public static async void RenameAsset(UnityEngine.Object asset, string name)
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
            Selection.activeObject = null;
            File.Move(path, npath);

            // 延时目的是防止 gui 刷新 button 时候目标丢失  
            // 或者使用 EditorApplication.delayCall
            await System.Threading.Tasks.Task.Delay(100);

            AssetDatabase.Refresh();
            asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(nAssetPath);
            Selection.activeObject = asset;
            asset.name = name;
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Success rename asset > {name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"failed rename asset > {ex}");
        }
    }
}
#endif