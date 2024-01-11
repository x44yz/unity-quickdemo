// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using AI.FSM;

// public class ActorState : State
// {
//     public static ResSystem sRes => GameMgr.Inst.sRes;
//     public static TimeSystem sTime => GameMgr.Inst.sTime;
//     public static EntitySystem sEntity => GameMgr.Inst.sEntity;

//     protected Actor actor;
//     protected ActorAI ai => actor.ai;
//     // protected ActorANI ani => actor.ani;
//     // protected ActorMotion mot => actor.motion;
//     protected ActorStat stat => actor.stat;

//     public ActorState(Actor actor)
//     {
//         this.actor = actor;
//     }
// }