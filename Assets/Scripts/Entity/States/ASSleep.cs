// using UnityEngine;
// using AI.FSM;

// public class ASSleep : ActorState
// {
//     private bool isSleeping = false;
//     private float sleepEnergySpd;

//     public ASSleep(Actor actor) : base(actor)
//     {
        
//     }

//     public override void OnEnter(State from)
//     {
//         base.OnEnter(from);

//         isSleeping = false;

//         if (suv.sleepRoomUid == GameDef.INVALID_UID)
//         {
//             Debug.LogError($"{suv.name} sleep room uid is invalid");
//             return;
//         }

//         var r = shelter.GetRoomByUid(suv.sleepRoomUid);
//         if (suv.IsAtSleepRoom == false)
//         {
//             suv.GetComp<ActorMotion>().MoveToFloor(r.floorNum, HandleAtSleepRoom);
//         }
//         else
//         {
//             HandleAtSleepRoom();
//         }

//         sleepEnergySpd = suv.cfg.sleepEnergySpd;
//     }

//     public override void OnExit(State to)
//     {
//         base.OnExit(to);

//         ani.PlayANI(AnimId.NOACTION2);
//     }

//     public override void OnUpdate(float dt)
//     {
//         base.OnUpdate(dt);

//         if (mot.IsMoving)
//             return;

//         if (isSleeping)
//         {
//             stat.ModStat(Stat.ENERGY, sleepEnergySpd * dt);
        
//             // 没有工作，如果天亮 & 并且能量满了，起床
//             if (suv.workRoomUid == GameDef.INVALID_UID && sTime.isDay && 
//                 stat.GetStat(Stat.ENERGY) >= stat.GetMaxStat(Stat.ENERGY))
//             {
//                 ai.ChangeToState<ASIdle>();
//             }
//         }

//         if (isSleeping == false)
//         {
//             isSleeping = true;
//             ani.PlayANI(AnimId.SLEEP);
//         }
//     }

//     private void HandleAtSleepRoom()
//     {
//         var r = shelter.GetRoomByUid(suv.sleepRoomUid);
//         var pt = r.GetSurvivorIdxPoint(suv, PointId.SLEEP);
//         mot.SetMovePoint(pt);
//     }
// }