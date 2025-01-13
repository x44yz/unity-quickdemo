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
        // public bool enableTickInterval;

        [Header("RUNTIME")]
        private float tick;
        private bool inited;
        public bool curDecisionRunning;
        public IContext ctx;
        [HideInInspector]public List<Decision> decisions;
        public Decision curDecision;
        // public IAction curAction;
        public Dictionary<Decision, DecisionDebug> decisionDebugs;
        public Dictionary<Decision, float> decisionCDStartTSs;

        public Decision soloDecision => config.soloDecision;
        public Decision[] mutexDecisions => config.mutexDecisions;

        public delegate void AgentHandleDelegate(AgentAI agent);
        public static AgentHandleDelegate onAgentCreate;
        public static AgentHandleDelegate onAgentDestroy;

        public delegate void DecisionChangedDelegate(Decision decision);
        public DecisionChangedDelegate onDecisionChanged;
        public DecisionChangedDelegate onScoreChanged;

#if UNITY_EDITOR
        public bool debugScore;
#endif

        private void Start()
        {
            onAgentCreate?.Invoke(this);
        }

        private void OnDestroy() 
        {
            onAgentDestroy?.Invoke(this);
        }

        public void Init(IContext ctx, bool log)
        {
            Init(ctx, config, true);
        }

        public void Init(IContext ctx, AIConfig cfg, bool log)
        {
            Reset();
            this.ctx = ctx;

            if (cfg == null && config == null)
            {
                if (log)
                    Debug.LogError($"{name} cant init because ai config is null");
                return;
            }

            inited = true;
            if (cfg != null)
                config = cfg;

            decisions = new List<Decision>();
            decisions.AddRange(config.decisions);

            // if (config.decisionGroups != null)
            // {
            //     foreach (var g in config.decisionGroups)
            //     {
            //         if (g == null)
            //         {
            //             if (showLog)
            //                 AILogger.LogError($"{config.name} decision group is null");
            //             continue;
            //         }
            //         decisions.AddRange(g.decisions);
            //     }
            // }

            decisionCDStartTSs = new Dictionary<Decision, float>();
            decisionDebugs = new Dictionary<Decision, DecisionDebug>();
            for (int i = 0; i < decisions.Count; ++i)
            {
                var decision = decisions[i];
                var dbg = new DecisionDebug();
                dbg.Init(decision);
                decisionDebugs[decision] = dbg;
            }
        }

        public void Reset()
        {
            inited = false;
            tick = 0f;
            if (decisions != null)
                decisions.Clear();
        }

        public void Tick(float dt)
        {
            if (inited == false)
                return;

#if UNITY_EDITOR
            if (debugScore)
            {
                DebugRefreshScore();
            }
#endif

            // if (enableTickInterval)
            {
                tick += dt;
                if (tick < tickInterval)
                    return;
                tick -= tickInterval;
            }

            if (curDecision != null && 
                curDecisionRunning == true &&
                curDecision.CanInterrupt(ctx) == false)
            {
                // 在其他地方 tick
                return;
            }
                
            // if (curDecision != null && curAction != null)
            // {
            //     var status = curAction.Execute(ctx, dt);
            //     if (status == Status.FINISHED)
            //     {
            //         curAction.Exit(ctx);
            //         StartCooldown(curDecision, ctx);
            //         if (showLog)
            //             AILogger.Log($"{name} exit decision > {curDecision.name}");
            //         curDecision = null;
            //         curAction = null;
            //     }
            // }

            // if (curDecision != null && !curDecision.CanInterrupt(ctx))
            //     return;

            var bestDecision = Select();
            if (bestDecision == curDecision &&
                curDecisionRunning)
            {
                // if (showLog)
                //     AILogger.Log($"same best descision > {bestDecision.name}");
                return;
            }

            // if (curDecision != null && curAction != null)
            // {
            //     curAction.Exit(ctx);
            //     StartCooldown(curDecision, ctx);
            //     if (showLog)
            //         AILogger.Log($"{name} exit decision > {curDecision.name}");
            // }

            curDecision = bestDecision;
            if (curDecision.Handle(ctx, OnDecisionDone))
            {
                curDecisionRunning = true;
            }
            else
            {
                curDecisionRunning = false;
            }
            // curAction = curDecision.GetAction();
            // if (curDecision != null && curAction != null)
            // {
            //     curAction.Enter(ctx);
            if (showLog)
                AILogger.Log($"{name} new decision > {curDecision.name}");
            // }

            if (onDecisionChanged != null)
                onDecisionChanged.Invoke(curDecision);
        }

        private void OnDecisionDone()
        {
            // if (action != curAction)
            // {
            //     AILogger.LogError("not current action");
            //     return;
            // }

            // curDecision.Exit(ctx);

            StartCooldown(curDecision);
            if (showLog)
                AILogger.Log($"{name} exit decision > {curDecision.name}");

            curDecisionRunning = false;
            // curDecision = null;
            // curAction = null;
        }

        private Decision Select()
        {
            if (decisions == null || decisions.Count == 0)
                return null;

            // debug solo & mutex
            if (soloDecision != null)
                return soloDecision;

            float bestScore = float.MinValue;
            Decision bestDecision = null;
            for (int i = 0; i < decisions.Count; ++i)
            {
                var dec = decisions[i];
                if (dec == null)
                    continue;

                if (IsMutexDecision(dec))
                    continue;
                if (IsInCooldown(dec))
                    continue;
                if (Evaluate(dec) == false)
                    continue;

                float score = Score(dec);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestDecision = dec;
                }
            }

            return bestDecision;
        }

        private float Score(Decision decision)
        {
            if (decision.considerations == null || decision.considerations.Length == 0)
                return 0f;

            var dbgInfo = GetDecisionDebugInfo(decision);

            float score = 0.0f;
            for (int i = 0; i < decision.considerations.Length; i++)
            {
                var con = decision.considerations[i];
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
                onScoreChanged.Invoke(decision);
            return score;
        }

        private bool Evaluate(Decision decision)
        {
            if (decision.preconditions == null || decision.preconditions.Length == 0)
                return true;

            var dbgInfo = GetDecisionDebugInfo(decision);

            dbgInfo.preBreakIdx = int.MaxValue;
            for (int i = 0; i < decision.preconditions.Length; ++i)
            {
                var pre = decision.preconditions[i];
                var isTrue = pre.IsTrue(ctx);
                if (pre.isNot) isTrue = !isTrue;
                dbgInfo.preBools[i] = isTrue;

                if (isTrue == false)
                {
                    dbgInfo.preBreakIdx = i;
                    return false;
                }
            }
            return true;
        }

        protected void StartCooldown(Decision decision)
        {
            if (decision.cooldown <= 0f)
                return;

            decisionCDStartTSs[decision] = ctx.GetDecisionCooldownTS();
        }

        public bool IsInCooldown(Decision decision)
        {
            if (decision.cooldown <= 0f)
            {
                return false;
            }

            float startTS = 0f;
            if (decisionCDStartTSs.TryGetValue(decision, out startTS) == false)
            {
                return false;
                // startTS = ctx.GetDecisionCooldownTS();
                // decisionCDStartTSs[decision] = startTS;
            }

            bool isCooldown = ctx.GetDecisionCooldownTS() < startTS + decision.cooldown;
            GetDecisionDebugInfo(decision).isInCooldown = isCooldown;
            return isCooldown;
        }

        private bool IsMutexDecision(Decision decision)
        {
            if (mutexDecisions == null || mutexDecisions.Length == 0)
                return false;
            for (int i = 0; i < mutexDecisions.Length; ++i)
            {
                if (mutexDecisions[i] == decision)
                    return true;
            }
            return false;
        }

        private void DebugRefreshScore()
        {
            if (decisions == null || decisions.Count == 0)
                return;

            for (int i = 0; i < decisions.Count; ++i)
            {
                var dec = decisions[i];
                if (dec == null)
                    continue;
                Score(dec);
            }
        }

        public DecisionDebug GetDecisionDebugInfo(Decision decision)
        {
            return decisionDebugs[decision];
        }
    }
}
