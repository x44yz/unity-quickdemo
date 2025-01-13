// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// // #if UNITY_EDTOR

// public class AQueueLine : EntityAbility
// {
//     public IQueueLineTarget target;

//     [Header("RUNTIME")]
//     public QueueLine queue;
//     public List<Actor> actors = new List<Actor>();

//     public override void Init(Entity et)
//     {
//         base.Init(et);

//         if (queue == null)
//             queue = GetComponentInChildren<QueueLine>();
//         if (queue == null)
//             Debug.LogError($"cant find queue line > {name}");
//         else
//             queue.Init();
//     }

//     public override void Reset()
//     {
//         base.Reset();

//         actors.Clear();
//     }

//     public override void Tick(float dt)
//     {
//         base.Tick(dt);

//         if (actors.Count <= 0)
//             return;

//         if (target.CanServeActor() == false)
//             return;

//         var curActor = actors[0];
//         actors.RemoveAt(0);
//         target.ServeActor(curActor);

//         for (int i = 0; i < actors.Count; ++i)
//         {
//             var k = actors[i];
//             k.motion.MoveToPos(queue.GetSubPos(i));
//         }
//     }

//     public void HandleActorJoin(Actor actor)
//     {        
//         int idx = actors.Count;
//         var p = queue.GetSubPos(idx);
//         actor.motion.MoveToPos(p, ()=>{
//             var p = queue.GetSubPos(actors.Count);
//             actor.motion.MoveToPos(p);
//             actors.Add(actor);
//         });
//     }

//     public void HandleActorLeave(Actor actor)
//     {
//         actors.Remove(actor);

//         for (int i = 0; i < actors.Count; ++i)
//         {
//             var k = actors[i];
//             k.motion.MoveToPos(queue.GetSubPos(i));
//         }
//     }
// }
