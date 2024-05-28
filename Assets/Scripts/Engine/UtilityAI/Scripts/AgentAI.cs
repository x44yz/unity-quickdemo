using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    public class AgentAI : MonoBehaviour
    {
        public float tickInterval = 1f;
        public AIConfig config;
        public bool showLog;
        public bool enableTickInterval;

        [Header("RUNTIME")]
        public List<Action> actions;
        public Action curAction;
        [SerializeField] private float tick;
        [SerializeField] private bool inited;
        public bool debugScore;
        public Dictionary<Action, ActionDebug> actionDebugs;
        public Dictionary<Action, float> actionCDStartTSs;

        public Action soloAction => config.soloAction;
        public Action[] mutexActions => config.mutexActions;

        public delegate void AgentHandleDelegate(AgentAI agent);
        public static AgentHandleDelegate onAgentCreate;
        public static AgentHandleDelegate onAgentDestroy;

        public delegate void ActionChangedDelegate(Action act);
        public ActionChangedDelegate onActionChanged;
        public ActionChangedDelegate onScoreChanged;

        private void Start()
        {
            onAgentCreate?.Invoke(this);
        }

        private void OnDestroy() 
        {
            onAgentDestroy?.Invoke(this);
        }

        public void Init(bool log)
        {
            Init(config, true);
        }

        public void Init(AIConfig cfg, bool log)
        {
            Reset();

            if (cfg == null && config == null)
            {
                if (log)
                    Debug.LogError($"{name} cant init because ai config is null");
                return;
            }

            inited = true;
            if (cfg != null)
                config = cfg;

            actions = new List<Action>();
            actions.AddRange(config.actions);

            if (config.actionGroups != null)
            {
                foreach (var g in config.actionGroups)
                {
                    if (g == null)
                    {
                        if (showLog)
                            AILogger.LogError($"{config.name} action group is null");
                        continue;
                    }
                    actions.AddRange(g.actions);
                }
            }

            actionCDStartTSs = new Dictionary<Action, float>();
            actionDebugs = new Dictionary<Action, ActionDebug>();
            for (int i = 0; i < actions.Count; ++i)
            {
                var action = actions[i];
                var dbg = new ActionDebug();
                dbg.Init(action);
                actionDebugs[action] = dbg;
            }
        }

        // private ActionObj CreateActionObj(Action action)
        // {
        //     if (action == null)
        //     {
        //         if (showLog)
        //             AILogger.LogError($"{config.name} action is null");
        //         return null;
        //     }

        //     var obj = (ActionObj)Activator.CreateInstance(action.ActionObjType());
        //     if (obj == null)
        //     {
        //         if (showLog)
        //             AILogger.LogError($"{config.name} action {action.name} cant create obj");
        //         return null;
        //     }
        //     obj.Init(action);

        //     // solo & mutex
        //     if (config.soloAction == action)
        //         soloActionObj = obj;
        //     if (config.mutexActions != null && config.mutexActions.Length > 0)
        //     {
        //         for (int i = 0; i < config.mutexActions.Length; ++i)
        //         {
        //             if (config.mutexActions[i] != action)
        //                 continue;
        //             mutexActionObjs.Add(obj);
        //             break;
        //         }
        //     }

        //     return obj;
        // }

        public void Reset()
        {
            // if (actionObjs != null && actionObjs.Length > 0)
            // {
            //     actionObjs = null;
            // }

            inited = false;
            tick = 0f;
            actions.Clear();
        }

        public void Tick(IContext ctx, float dt)
        {
            if (inited == false)
                return;

            if (debugScore)
            {
                DebugRefreshScore(ctx);
            }

            if (enableTickInterval)
            {
                tick += dt;
                if (tick < tickInterval)
                    return;
                tick -= tickInterval;
            }
                
            if (curAction != null)
            {
                var status = curAction.Execute(ctx, dt);
                if (status == Status.FINISHED)
                {
                    curAction.Exit(ctx);
                    StartCooldown(curAction, ctx);
                    if (showLog)
                        AILogger.Log($"{name} exit action > {curAction.name}");
                    curAction = null;
                }
            }

            if (curAction != null && !curAction.CanInterrupt(ctx))
                return;

            var bestAction = Select(ctx);
            if (bestAction == curAction)
                return;

            if (curAction != null)
            {
                curAction.Exit(ctx);
                StartCooldown(curAction, ctx);
                if (showLog)
                    AILogger.Log($"{name} exit action > {curAction.name}");
            }
            curAction = bestAction;
            if (curAction != null)
            {
                curAction.Enter(ctx);
                if (showLog)
                    AILogger.Log($"{name} enter action > {curAction.name}");
            }

            if (onActionChanged != null)
                onActionChanged.Invoke(curAction);
        }

        private Action Select(IContext ctx)
        {
            if (actions == null || actions.Count == 0)
                return null;

            // debug solo & mutex
            if (soloAction != null)
                return soloAction;

            float bestScore = float.MinValue;
            Action bestAction = null;
            for (int i = 0; i < actions.Count; ++i)
            {
                var act = actions[i];
                if (act == null)
                    continue;

                if (IsMutexAction(act))
                    continue;
                if (IsInCooldown(act, ctx))
                    continue;
                if (Evaluate(act, ctx) == false)
                    continue;

                float score = Score(act, ctx);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestAction = act;
                }
            }

            return bestAction;
        }

        private float Score(Action action, IContext ctx)
        {
            if (action.considerations == null || action.considerations.Length == 0)
                return 0f;

            var dbgInfo = GetActionDebugInfo(action);

            float score = 0.0f;
            for (int i = 0; i < action.considerations.Length; i++)
            {
                var con = action.considerations[i];
                float s = con.Score(ctx) * con.weight; // / conTotalWeight;
                // Debug.Log($"xx-- {name} - {i}/{considerations.Length}");
                dbgInfo.conScores[i] = s;

                score += s;
            }

            // why average
            // average
            // score = score / considerations.Length;
            dbgInfo.curScore = score;

            if (onScoreChanged != null)
                onScoreChanged.Invoke(action);
            return score;
        }

        private bool Evaluate(Action action, IContext ctx)
        {
            if (action.preconditions == null || action.preconditions.Length == 0)
                return true;

            var dbgInfo = GetActionDebugInfo(action);

            dbgInfo.preBreakIdx = int.MaxValue;
            for (int i = 0; i < action.preconditions.Length; ++i)
            {
                var isTrue = action.preconditions[i].IsTrue(ctx);
                dbgInfo.preBools[i] = isTrue;

                if (isTrue == false)
                {
                    dbgInfo.preBreakIdx = i;
                    return false;
                }
            }
            return true;
        }

        protected void StartCooldown(Action action, IContext ctx)
        {
            if (action.cooldown <= 0f)
                return;

            actionCDStartTSs[action] = ctx.GetActionCooldownTS();
        }

        public bool IsInCooldown(Action action, IContext ctx)
        {
            if (action.cooldown <= 0f)
            {
                return false;
            }

            float startTS = 0f;
            if (actionCDStartTSs.TryGetValue(action, out startTS) == false)
            {
                startTS = ctx.GetActionCooldownTS();
                actionCDStartTSs[action] = startTS;
            }

            bool isCooldown = ctx.GetActionCooldownTS() < startTS + action.cooldown;
            GetActionDebugInfo(action).isInCooldown = isCooldown;
            return isCooldown;
        }

        private bool IsMutexAction(Action action)
        {
            if (mutexActions == null || mutexActions.Length == 0)
                return false;
            for (int i = 0; i < mutexActions.Length; ++i)
            {
                if (mutexActions[i] == action)
                    return true;
            }
            return false;
        }

        private void DebugRefreshScore(IContext ctx)
        {
            if (actions == null || actions.Count == 0)
                return;

            for (int i = 0; i < actions.Count; ++i)
            {
                var act = actions[i];
                if (act == null)
                    continue;
                Score(act, ctx);
            }
        }

        public ActionDebug GetActionDebugInfo(Action action)
        {
            return actionDebugs[action];
        }
    }
}
