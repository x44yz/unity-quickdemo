using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "B_XX_MoveSpd", menuName = "BUFF/MoveSpd")]
public class BMoveSpd : Buff
{
    public BuffValue val;
    
    public override string Desc()
    {
        return $"Move Spd {val}";
    }
    public override System.Type InstType() => null;

    public override void ApplyToActor(Actor actor)
    {
        actor.stat.AddBuffVal(BuffKey.MOVE_SPD, val.value, val.valType);
    }
}