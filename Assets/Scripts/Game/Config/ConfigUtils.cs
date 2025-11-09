#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using System.Linq;

public static class ConfigUtils
{
    public static void RenameAssetName(UnityEngine.Object asset, string id)
    {
        var name = id.ToTitleCase().Replace("_", "");
        EditorUtils.RenameAsset(asset, name);
    }

    public static List<string> GetConfigIds<T>(string folderName, 
        System.Func<T, string> callback) where T : ScriptableObject
    {
        string folderPath = Application.dataPath + $"/Configs/{folderName}/";
        string[] allFilePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
            .Where(path => !path.EndsWith(".meta")).ToArray(); // 忽略.meta文件

        var ret = new List<string>();
        foreach (string filePath in allFilePaths)
        {
            string relativePath = filePath.Replace(Application.dataPath, "Assets");
            T cfg = AssetDatabase.LoadAssetAtPath<T>(relativePath);
            if (cfg == null)
            {
                Debug.LogError($"failed load config at path > {relativePath}");
                continue;
            }
            ret.Add(callback(cfg));
        }
        return ret;
    }

    public static List<string> GetBuffIdValues()
    {
        var ret = GetConfigIds<BuffSO>("Buffs", (cfg) => cfg.id);
        return ret;
    }

    public static string[] EffectSOTypeStrs = new string[]{
        typeof(MoveSpeedEffectSO).ToString(),
    };

   public static bool AddEffectToAsset(UnityEngine.Object asset, 
        ref EffectSO[] effs, string effectType)
    {
        var eff = ScriptableObject.CreateInstance(effectType) as EffectSO;
        if (eff == null)
        {
            Debug.LogError($"cant create effect > {effectType}");
            return false;
        }
        if (effs == null)
            effs = new EffectSO[1];
        else
            System.Array.Resize(ref effs, effs.Length + 1);
        effs[effs.Length - 1] = eff;
        eff.name = effectType;
        
        AssetDatabase.AddObjectToAsset(eff, asset);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
        return true;
    }

    public static async void RemoveFroMainAsset(UnityEngine.Object asset)
    {
        string mainAssetPath = AssetDatabase.GetAssetPath(asset);
        var mainAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(mainAssetPath);
        if (mainAsset.GetInstanceID() == asset.GetInstanceID())
        {
            Debug.LogWarning($"this main asset, cant remove");
            return;
        }

        AssetDatabase.RemoveObjectFromAsset(asset);
        await System.Threading.Tasks.Task.Delay(100);
        // 延时执行，否则会导入警告
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif