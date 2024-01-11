using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffChip
{
    public Buff buff;
    // public int weight;
    public int count;
}

[CreateAssetMenu(fileName = "BP_DAY_XXX", menuName = "BUFF/BuffDayPool")]
public class BuffDayPool : ScriptableObject
{
    public int day;
    public BuffChip[] buffs;
}
