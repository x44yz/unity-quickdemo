using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "LI_XX", menuName = "LocalItem")]
public class LocalItemCfg : ScriptableObject, IIdGenTarget
{
    public string Id() => key;
    public string Name() => key;
    public bool IsExport() => genId;

    public string key;
    public bool genId;
    // public LocalType localType;
    [TextArea]
    public string en;
    [TextArea]
    public string zhcn;

    public string GetVal(Lang lang)
    {
        if (lang == Lang.ZHCN)
            return zhcn;
        else if (lang == Lang.EN)
            return en;
        Debug.LogError($"not implement lang abbr > {lang}");
        return en;
    }

#if UNITY_EDITOR
    [Button("Auto Name", EButtonEnableMode.Editor)]
    private void AutoName()
    {
        key = key.Trim().ToUpper();
        EditorUtils.RenameAsset(this, $"LI_{key}");
    }
#endif
}