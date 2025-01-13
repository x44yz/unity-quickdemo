using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "1010A_Idle", menuName = "AI/A/AIdle")]
public class AIdle : ActorDecision
{
    public float minMins;
    // public StatVal[] statSpdVals;

    public override bool Handle(IContext ctx, System.Action cbk)
    {
        return true;
    }
}
