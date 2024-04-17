using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class InteractionUtils
{
    public static bool AddEffectToAsset(Object asset, ref ConditionCollection[] effs, 
        string effectType, string nameSuffix)
    {
        var eff = ScriptableObject.CreateInstance(effectType) as ConditionCollection;
        if (eff == null)
        {
            Debug.LogError($"cant create effect > {effectType}");
            return false;
        }
        if (effs == null)
            effs = new ConditionCollection[1];
        else
            System.Array.Resize(ref effs, effs.Length + 1);
        effs[effs.Length - 1] = eff;

        string name = "";
        // string parentName = GetParentName(asset);
        // if (string.IsNullOrEmpty(parentName) == false)
        //     name += $"{parentName}_";
        name += $"{effectType}";
        if (string.IsNullOrEmpty(nameSuffix) == false)
            name += $"_{nameSuffix}";
        eff.name = name;
        
        var path = "Assets/Prefabs/Tent.prefab";
        AssetDatabase.AddObjectToAsset(eff, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return true;
    }
}