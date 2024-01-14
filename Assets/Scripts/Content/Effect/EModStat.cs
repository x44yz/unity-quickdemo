using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XX_ModStat", menuName = "EFFECT/ModStat")]
public class EModStat : Effect
{
    public Stat stat;
    public float val;
    // public float time;
    // public bool isSpeed;
    
    public override string Desc()
    {
        return $"ModStat+{stat}-{val}";
    }

    public override System.Type InstType() => null;
}
