using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

public enum BuffLifeStackType
{
    ResetLife = 0, // 重置到默认生命
    ExtendLife = 1, // 累加生命
    ExtendAndResetLife = 2, // 累加同时重置
    // StandaloneLife = 3, // 分开计算  
}

[CreateAssetMenu(fileName = "Buff", menuName = "CONFIG/Buff")]
public class BuffSO : ScriptableObject, IIdGenTarget
{
    [OnValueChanged("OnIdChanged")]
    public string id;
    public string _name;
    [TextArea]
    public string comment;
    [TextArea]
    public string desc;
    public Sprite spr;

    public TargetScope targetScope; 

    public float life;
    public int maxStack = 1;
    public BuffLifeStackType lifeStackType;

    public EffectSO[] effects;

#region IIdGenTarget
    public string Id() => id;
    public string Name() => _name;
    public bool IsExport() => true;
#endregion // IIdGenTarget

#if UNITY_EDITOR
    private void OnIdChanged()
    {
        id = id.Trim().Replace(' ', '_');
        id = id.ToLower();
    }

    // [Button("Auto Name", EButtonEnableMode.Editor)]
    // private void AutoName()
    // {
    //     ConfigUtils.RenameAssetName(this, id);
    // }

    // [BoxGroup("EFFECT")]
    // public string effectSuffix;
    [BoxGroup("EFFECT")]
    [Dropdown("effectTypes")]
    public string effectType;
    private static string[] effectTypes = ConfigUtils.EffectSOTypeStrs;

    [Button("AddEffect", EButtonEnableMode.Editor)]
    private void AddEffect()
    {
        ConfigUtils.AddEffectToAsset(this, ref effects, effectType);
    }
#endif
}