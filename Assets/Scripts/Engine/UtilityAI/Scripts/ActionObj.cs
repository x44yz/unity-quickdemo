
// namespace AI.Utility
// {
//     public abstract class ActionObj
//     {
//         public enum Status
//         {
//             WAITING,
//             EXECUTING,
//             FINISHED,
//         }

//         public delegate void ScoreChangedDelegate(ActionObj act);
//         public ScoreChangedDelegate onScoreChanged;

//         public string dbgName => action ? action.name : GetType().ToString();
//         public Action action { get; protected set; }
//         public Precondition[] preconditions => action.preconditions;
//         public ConsiderationDeco[] considerations => action.considerations;

//         // for debug
//         public float curScore { get; protected set; }
//         private float[] conScores = null;
//         private bool[] preBools = null;
//         private int preBreakIdx = 0;
//         public bool isCooldown { get; protected set; } = false;
//         public float cooldownStartTS = float.MinValue;

//         public virtual void Init(Action action)
//         {
//             this.action = action;

//             conScores = new float[considerations.Length];
//             preBools = new bool[preconditions.Length];
//             // conTotalWeight = 0f;
//             // foreach (var con in considerations)
//             // {
//             //     conTotalWeight += con.weight;
//             // }
//         }

//         public virtual bool CanInterrupt(IContext ctx) => action.interruptable;

//         public bool Evaluate(IContext ctx)
//         {
//             if (preconditions == null || preconditions.Length == 0)
//                 return true;

//             preBreakIdx = int.MaxValue;
//             for (int i = 0; i < preconditions.Length; ++i)
//             {
//                 preBools[i] = preconditions[i].IsTrue(ctx);
//                 if (preBools[i] == false)
//                 {
//                     preBreakIdx = i;
//                     return false;
//                 }
//             }
//             return true;
//         }

//         // public float Score(IContext ctx)
//         // {
//         //     if (considerations == null || considerations.Length == 0)
//         //         return 1;

//         //     var totalScore = 1f;

//         //     var modificationFactor = 1f - 1f / considerations.Length;
//         //     for (int i = 0; i < considerations.Length; i++)
//         //     {
//         //         var con = considerations[i];
//         //         float score = con.Score(ctx);

//         //         var makeUpValue = (1f - score) * modificationFactor;
//         //         score += (makeUpValue * score);
//         //         conScores[i] = score;

//         //         totalScore *= score;
//         //         if (totalScore < 0.01f)
//         //             break;
//         //     }

//         //     CurScore = totalScore;

//         //     if (onScoreChanged != null)
//         //         onScoreChanged.Invoke(totalScore);
//         //     return totalScore;
//         // }

//         public float Score(IContext ctx)
//         {
//             if (considerations == null || considerations.Length == 0)
//                 return 0f;

//             float score = 0.0f;
//             for (int i = 0; i < considerations.Length; i++)
//             {
//                 var con= considerations[i];
//                 float s = con.Score(ctx) * con.weight; // / conTotalWeight;
//                 // Debug.Log($"xx-- {name} - {i}/{considerations.Length}");
//                 conScores[i] = s;

//                 score += s;
//             }

//             // why average
//             // average
//             // score = score / considerations.Length;
//             curScore = score;

//             if (onScoreChanged != null)
//                 onScoreChanged.Invoke(this);
//             return score;
//         }

//         public float GetConsiderationScore(int idx)
//         {
//             if (conScores == null || idx < 0 || idx >= conScores.Length)
//                 return 0f;
//             return conScores[idx];
//         }

//         public bool? GetPreconditionBool(int idx)
//         {
//             if (preBools == null || idx < 0 || idx >= preBools.Length)
//                 return null;
//             if (idx > preBreakIdx)
//                 return null;
//             return preBools[idx];
//         }

//         public bool IsPrecondtionsValid()
//         {
//             if (preBools == null || preBools.Length == 0)
//                 return true;
//             return preBreakIdx > preBools.Length;
//         }

//         public bool IsInCooldown(IContext ctx)
//         {
//             if (action.cooldown <= 0f)
//             {
//                 isCooldown = false;
//                 return false;
//             }
//             isCooldown = ctx.GetActionCooldownTS() < cooldownStartTS + action.cooldown;
//             return isCooldown;
//         }

//         protected void StartCooldown(IContext ctx)
//         {
//             cooldownStartTS = ctx.GetActionCooldownTS();
//         }

//         public void Enter(IContext ctx)
//         {
//             OnEnter(ctx);
//             action.Enter(ctx);
//         }

//         public Status Execute(IContext ctx, float dt)
//         {
//             // return OnExecute(ctx, dt);
//             return action.Execute(ctx, dt);
//         }

//         public void Exit(IContext ctx)
//         {
//             StartCooldown(ctx);
//             OnExit(ctx);
//             action.Exit(ctx);
//         }

//         protected virtual void OnEnter(IContext ctx)
//         {
//         }

//         protected virtual Status OnExecute(IContext ctx, float dt)
//         {
//             return Status.EXECUTING;
//         }

//         protected virtual void OnExit(IContext ctx)
//         {
//         }
//     }
// }