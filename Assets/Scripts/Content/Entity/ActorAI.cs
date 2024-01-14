// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using AI.FSM;

// public class ActorAI : ActorComp, IStateMachineOwner
// {
//     [Header("RUNTIME")]
//     public bool debugFSMState = false;
//     public string curStateType;
//     public StateMachine<ActorAI> fsm;
//     // private Stack<Type> stackStats = new Stack<Type>();
//     public Type waitState = null;

//     public bool IsFSMDebug => debugFSMState;
//     public string FSMDebugLogPrefix => name;

//     public bool IsInAttack
//     {
//         get {
//             return IsState<ASAttack>() || IsState<ASReload>();
//         }
//     }

//     public override void Init(Actor actor)
//     {
//         base.Init(actor);

//         fsm = new StateMachine<ActorAI>(this);

//         if (actor is Zombie)
//         {
//             ASIdle idle = new ASIdle(actor);
//             // ASSearchEnemy searchEnemy = new ASSearchEnemy(actor);
//             ASTrackEnemy trackEnemy = new ASTrackEnemy(actor);
//             ASAttack attack = new ASAttack(actor);
//             ASHit hit = new ASHit(actor);
//             ASDead dead = new ASDead(actor);
            
//             fsm.Register(idle);
//             // fsm.Register(searchEnemy);
//             fsm.Register(trackEnemy);
//             fsm.Register(attack);
//             fsm.Register(hit);
//             fsm.Register(dead);
//         }
//         else if (actor is Survivor)
//         {
//             ASIdle idle = new ASIdle(actor);
//             fsm.Register(idle);

//             ASWaitJoin waitJoin = new ASWaitJoin(actor);
//             fsm.Register(waitJoin);

//             // ASCutting cutting = new ASCutting(actor);
//             ASAttack attack = new ASAttack(actor);
//             // ASPick pick = new ASPick(actor);
//             // ASMakeTool makeTool = new ASMakeTool(actor);
//             // ASMoveToFloor moveToFloor = new ASMoveToFloor(actor);
//             ASBuild build = new ASBuild(actor);
//             ASSleep sleep = new ASSleep(actor);
//             ASEat eat = new ASEat(actor);
//             ASDead dead = new ASDead(actor);
//             ASHit hit = new ASHit(actor);
//             ASReload reload = new ASReload(actor);
//             ASWork work = new ASWork(actor);
//             ASWildWork wildWork = new ASWildWork(actor);
//             ASRest rest = new ASRest(actor);

//             // fsm.Register(cutting);
//             fsm.Register(attack);
//             // fsm.Register(pick);
//             // fsm.Register(makeTool);
//             // fsm.Register(moveToFloor);
//             fsm.Register(build);
//             fsm.Register(sleep);
//             fsm.Register(eat);
//             fsm.Register(dead);
//             fsm.Register(hit);
//             fsm.Register(reload);
//             fsm.Register(work);
//             fsm.Register(wildWork);
//             fsm.Register(rest);
//         }
//         else
//         {
//             Debug.LogError($"not implement actor type > {actor.GetType()}");
//         }

//         // fsm.AddTransition(new Transition(idle, cutting, idle.IsTranslateToCutting, idle.OnTranslateByTask));
//         // fsm.AddTransition(new Transition(idle, pick, idle.IsTranslateToPick, idle.OnTranslateByTask));
//         // fsm.AddTransition(new Transition(idle, makeTool, idle.IsTranslateToMakeTool, idle.OnTranslateByTask));
//         // fsm.AddTransition(new Transition(idle, escape, idle.IsTranslateToEscape));
//         // fsm.AddTransition(new Transition(idle, attack, idle.IsTranslateToAttack));
//         // fsm.AddTransition(new Transition(cutting, idle, cutting.IsTranslateToIdle));
//         // fsm.AddTransition(new Transition(cutting, escape, cutting.IsTranslateToEscape));
//         // fsm.AddTransition(new Transition(pick, idle, pick.IsTranslateToIdle));
//         // fsm.AddTransition(new Transition(pick, escape, pick.IsTranslateToEscape));
//         // fsm.AddTransition(new Transition(attack, idle, attack.IsTranslateToIdle));
//         // fsm.AddTransition(new Transition(attack, escape, attack.IsTranslateToEscape));
//         // fsm.AddTransition(new Transition(makeTool, idle, makeTool.IsTranslateToIdle));
//         // fsm.AddTransition(new Transition(escape, idle, escape.IsTranslateToIdle));

//         // fsm.Translate(typeof(ASIdle));
//         ChangeToState<ASIdle>();
//     }

//     public override void Tick(float dt)
//     {
//         fsm.Update(dt);
//     }

//     public ActorState GetState(Type tp)
//     {
//         if (fsm.states.ContainsKey(tp) == false)
//         {
//             Debug.LogError($"cant find state > {tp}");
//             return null;
//         }
//         return fsm.states[tp] as ActorState;
//     }

//     public bool IsState<T>()
//     {
//         return fsm.curState != null && fsm.curState.GetType() == typeof(T);
//     }

//     public T GetState<T>() where T : ActorState
//     {
//         var tp = typeof(T);
//         return GetState(tp) as T;
//     }

//     // public void PushStat<T>() where T : ActorState
//     // {
//     //     var tp = typeof(T);
//     //     if (stackStats.Count > 0)
//     //     {
//     //         var t = stackStats.Peek();
//     //         if (t == tp)
//     //         {
//     //             Debug.LogWarning($"{t} in stat stack");
//     //             return;
//     //         }
//     //     }
//     //     if (debugFSMState)
//     //         Debug.Log($"{actor.name} push stat {tp}");
//     //     stackStats.Push(tp);
//     // }

//     // public Type PopStat()
//     // {
//     //     if (stackStats.Count == 0)
//     //     {
//     //         Debug.LogError($"{actor.name} stat stack is empty");
//     //         return typeof(ASIdle);
//     //     }

//     //     var tp = stackStats.Pop();
//     //     if (debugFSMState)
//     //         Debug.Log($"{actor.name} pop stat {tp}");
//     //     return tp;
//     // }

//     // public void TryChangeToIdle()
//     // {
//     //     if (stackStats.Count > 0)
//     //     {
//     //         var tp = PopStat();
//     //         curStateType = tp.ToString();
//     //         fsm.Translate(tp);
//     //         return;
//     //     }
//     //     ChangeToState<ASIdle>();
//     // }

//     public void ChangeToState<T>(System.Action<T> initCallback = null) where T : ActorState
//     {
//         if (initCallback != null)
//         {
//             var s = GetState<T>();
//             initCallback.Invoke(s);
//         }

//         // if (fsm.curState != null && fsm.curState.GetType() != typeof(ASIdle)
//         //     && typeof(T) != typeof(ASIdle))
//         // {
//         //     // PushStat<T>();
//         //     waitState = typeof(T);
//         //     if (debugFSMState)
//         //         Debug.Log($"{actor.name} cache wait state > {waitState}");
//         //     return;
//         // }

//         // if (typeof(T) == typeof(ASIdle) && waitState != null)
//         // {
//         //     curStateType = waitState.ToString();
//         //     if (debugFSMState)
//         //         Debug.Log($"{actor.name} change to wait state > {curStateType}");
//         //     fsm.Translate(waitState);
//         //     waitState = null;
//         //     return;
//         // }

//         curStateType = typeof(T).ToString();
//         if (debugFSMState)
//             Debug.Log($"{actor.name} change to state > {curStateType}");
//         fsm.Translate<T>();
//     }
// }