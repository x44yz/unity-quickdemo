using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorContext : MonoBehaviour, AI.Utility.IContext
{
    public TimeSystem sTime => GameMgr.Inst.sTime;
    public Actor actor;

    // public float viewRange => actor.viewRange;
    public float dayHour => sTime.dayHours;

    public float GetActionCooldownTS()
    {
        return sTime.totalMins;
    }
    
    public float GetStatNOR(Stat s)
    {
        var v = actor.stat.GetStat(s);
        var max = actor.stat.GetStatMax(s);
        return Mathf.Clamp01(v / max);
    }
}
