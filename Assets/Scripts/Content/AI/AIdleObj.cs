using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

public class AIdleObj : ActionObj
{
    public AIdle idleAct => action as AIdle;
    public float tick;

    public override bool CanInterrupt(IContext ctx)
    {
        return action.interruptable && tick >= idleAct.minMins;
    }

    protected override void OnEnter(IContext ctx)
    {
        tick = 0f;

        var actx = ctx as ActorContext;
        var actor = actx.actor;
        // if (actor.IsAtPoint(pointType) == false)
        //     actor.moveToPoint = actx.querySystem.GetPoint(pointType);

        // if (idle.pointId != PointId.NONE)
        // {
        //     var fl = actor.GetFloorAt();
        //     actor.SetMoveToPoint(fl.GetPointPos(idle.pointId));
        // }
        actor.motion.StopMove(false);
    }

    protected override Status OnExecute(IContext ctx, float dt)
    {
        var actx = ctx as ActorContext;
        var actor = actx.actor;
        // if (actor.IsMoving)
        //     return Status.WAITING;

        tick += dt;
        
        // // tick += dt;
        // // if (tick >= idle.minutes)
        // // {
        // //     // actor.ModStat(Stat.THIRSTY, drink.thirsty);
        // //     // actor.ModStat(Stat.EXCRETE, drink.excrete);
        // //     return Status.FINISHED;
        // // }
        // actor.ModStat(Stat.ENERGY, idle.energySpd * actx.aiDeltaMins);

        // actor.stat.ModStatSpds(idleAct.statSpdVals, dt);
        return Status.EXECUTING;
    }
}