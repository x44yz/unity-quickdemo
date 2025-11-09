using System;
using UnityEngine;

[Serializable]
public class StatBaseVal
{
    public Stat stat;
    public float baseVal;
}

public class StatData
{
    public Stat stat;
    public float baseVal;
    public float bonus = 0f;
    public float scale = 1f;

    public float GetValue()
    {
        return (baseVal + bonus) * scale;
    }

    // public void SetBaseVal(float v)
    // {
    //     baseVal = v;
    // }

    // public void Modify(ModifyValueType valueType, float val)
    // {
    //     if (valueType == ModifyValueType.Bonus)
    //         ModifyBonus(val);
    //     else if (valueType == ModifyValueType.Scale)
    //         ModifyScale(val);
    //     else
    //         Debug.LogError($"not implement modify value type > {valueType}");
    // }

    public void ModifyBonus(float val)
    {
        bonus += val;
    }

    public void ModifyScale(float val)
    {
        if (val <= 0f)
        {
            Debug.LogError($"cant modify {stat} > {val} < 0f");
            return;
        }

        scale *= val;
    }

    public override string ToString()
    {
        return $"stat {stat} value {GetValue()} - {baseVal} b{bonus} s{scale}";
    }
}