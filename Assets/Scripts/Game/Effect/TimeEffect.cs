using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StackRemoveType
{
    All = 0,
    One = 1,
}

public abstract class TimeEffect : Effect
{
    public float duration;

    protected float tick;

    public virtual StackRemoveType GetStackRemoveType() => StackRemoveType.All;

    protected override void OnTick(float dt)
    {
        tick += dt;
        if (tick >= duration)
        {
            Deactivate();
            // var stackRemoveType = GetStackRemoveType();
            // if (stackRemoveType == StackRemoveType.All)
            // {
            //     Deactivate();
            // }
            // else if (stackRemoveType == StackRemoveType.One)
            // {
            //     tick -= duration;
            //     ModifyStack(-1);
            // }
            // else
            //     Debug.LogError($"not implement statck remove type > {stackRemoveType}");
        }
    }
}