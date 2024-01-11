using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "1010A_Idle", menuName = "AI/A/AIdle")]
public class AIdle : ActorAction
{
    public override System.Type ActionObjType() => typeof(AIdleObj);

    public float minMins;
    // public StatVal[] statSpdVals;
}
