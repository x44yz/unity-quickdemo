using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "IC_ID", menuName = "CONFIG/Item")]
public class ItemCfg : ScriptableObject
{
    public ItemId id;
    public string itemName;
    public string comment;
    public ItemType itemType;
    public GameObject prefab;
    public Sprite spr;
    public float price;

#if UNITY_EDITOR
    [BoxGroup("EFFECT")]
    public string effectSuffix;
    [BoxGroup("EFFECT")]
    [Dropdown("effectTypes")]
    public string effectType;
    private static string[] effectTypes = EffectUtils.EffectTypeStrs;

    [Button("AddActionEffect", EButtonEnableMode.Editor)]
    private void AddActionEffect()
    {
        Effect[] effects = null;
        EffectUtils.AddEffectToAsset(this, ref effects, effectType, effectSuffix);
    }
#endif
}
