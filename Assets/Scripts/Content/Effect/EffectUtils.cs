using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class EffectUtils
{
    public static string[] EffectTypeStrs = new string[]{
        typeof(EModStat).ToString(),
    };

    public static StringBuilder sb = new StringBuilder();

    public static string ToDesc(List<Effect> effs, string splite = null)
    {
        sb.Clear();
        bool hasSplite = string.IsNullOrEmpty(splite) == false;
        foreach (var cfg in effs)
        {
            sb.Append(cfg.Desc());
            if (hasSplite)
                sb.Append(splite);
        }
        return sb.ToString();
    }

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

    // public static string ToEffectDesc(Event cfg, string splite = null)
    // {
    //     sb.Clear();
    //     bool hasSplite = string.IsNullOrEmpty(splite) == false;
    //     if (cfg.sureEffects != null)
    //     {
    //         foreach (var eff in cfg.sureEffects)
    //         {
    //             sb.Append(eff.GetDesc());
    //             if (hasSplite)
    //                 sb.Append(splite);
    //         }
    //     }
        
    //     if (cfg.probEffects != null)
    //     {
    //         int totalProb = cfg.GetTotalProb();
    //         foreach (var peff in cfg.probEffects)
    //         {
    //             sb.Append(peff.eff.GetDesc());
    //             float p = peff.prob * 1f / totalProb * 100f;
    //             sb.Append($"<color=#00ff00>{p.ToString("F1")}%</color>");
    //             if (hasSplite)
    //                 sb.Append(splite);
    //         }
    //     }

    //     return sb.ToString();
    // }
}