// using UnityEngine;
// using AI.FSM;

// public class ASIdle : ActorState
// {
//     public const float WANDER_RANGE = 4f;

//     private float tick = 0f;

//     public ASIdle(Actor actor) : base(actor)
//     {
//     }

//     public override void OnEnter(State from)
//     {
//         tick = 0f;

//         // if (ani.HasANI(AnimId.NOACTION))
//         //     ani.PlayANI(AnimId.NOACTION);
//         // if (ani.HasANI(AnimId.NOACTION2))
//         //     ani.PlayANI(AnimId.NOACTION2);
//     }

//     public override void OnUpdate(float dt) 
//     {
//         // if (mot.IsMoving)
//         //     return;
//     }

//     // public bool IsTranslateToCutting()
//     // {
//     //     // NOTE:
//     //     // 如果确保添加 AddTranslate 的优先级，此次并不需要判断
//     //     // if (IsTranslateToMove())
//     //     //     return false;
//     //     return owner.taskType == Actor.TaskType.Cutting;
//     // }

//     // public bool IsTranslateToPick()
//     // {
//     //     return owner.taskType == Actor.TaskType.Pick;
//     // }

//     // public bool IsTranslateToMakeTool()
//     // {
//     //     return owner.taskType == Actor.TaskType.MakeTool;
//     // }

//     // public void OnTranslateByTask()
//     // {
//     //     Debug.Log("xx-- OnTranslateByTask");
//     //     owner.taskType = Actor.TaskType.None;
//     // }

//     // public bool IsTranslateToEscape()
//     // {
//     //     // if (owner.targetZombie == null)
//     //     //     return false;
//     //     // // TODO: random
//     //     // return owner.atk < owner.targetZombie.atk * 10.3f;
//     //     return owner.IsZombieDangerNearby();
//     // }

//     // public bool IsTranslateToAttack()
//     // {
//     //     if (owner.targetZombie == null)
//     //         return false;
//     //     var dist = GameUtils.Vector3ZeroY(owner.pos - owner.targetZombie.pos).magnitude;
//     //     return dist <= owner.actorCfg.atkRange;
//     // }
// }