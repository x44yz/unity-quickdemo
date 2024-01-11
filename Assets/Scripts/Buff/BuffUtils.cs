using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class BuffUtils
{
    public static string[] BuffTypeStrs = new string[]{
        // typeof(EKnockBack).ToString(),
    };

    public static StringBuilder sb = new StringBuilder();

    // public static string ToDesc(List<Effect> effs, string splite = null)
    // {
    //     sb.Clear();
    //     bool hasSplite = string.IsNullOrEmpty(splite) == false;
    //     foreach (var cfg in effs)
    //     {
    //         sb.Append(cfg.Desc());
    //         if (hasSplite)
    //             sb.Append(splite);
    //     }
    //     return sb.ToString();
    // }

    public static string GetParentName(Object asset)
    {
        string mainAssetPath = AssetDatabase.GetAssetPath(asset);
        var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(mainAssetPath);
        if (obj.GetInstanceID() == asset.GetInstanceID())
            return null;
        return asset.name;
    }

#if UNITY_EDITOR
    public static bool AddEffectToAsset(Object asset, ref Effect[] effs, 
        string effectType, string nameSuffix)
    {
        var eff = ScriptableObject.CreateInstance(effectType) as Effect;
        if (eff == null)
        {
            Debug.LogError($"cant create effect > {effectType}");
            return false;
        }
        if (effs == null)
            effs = new Effect[1];
        else
            System.Array.Resize(ref effs, effs.Length + 1);
        effs[effs.Length - 1] = eff;

        string name = "";
        string parentName = GetParentName(asset);
        if (string.IsNullOrEmpty(parentName) == false)
            name += $"{parentName}_";
        name += $"{effectType}";
        if (string.IsNullOrEmpty(nameSuffix) == false)
            name += $"_{nameSuffix}";
        eff.name = name;
        
        AssetDatabase.AddObjectToAsset(eff, asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return true;
    }
#endif
}