using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemCount
{
    public ItemId id;
    public int count;
}

[Serializable]
public class StatVal
{
    public Stat stat;
    public float val;
}

[Serializable]
public class BuffValue
{
    public ValueType valType;
    public float value;

    public override string ToString()
    {
        if (valType == ValueType.CONSTANT)
            return value >= 0f ? $"+{value}" : $"{value}";
        else if (valType == ValueType.PERCENT)
            return value >= 0f ? $"+{value}%" : $"{value}%";
        else
            Debug.LogError($"not implement value type > {valType}");
        return value.ToString();
    }
}
