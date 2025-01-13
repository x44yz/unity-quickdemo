using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    // public enum Status
    // {
    //     WAITING,
    //     EXECUTING,
    //     FINISHED,
    // }

    [System.Serializable]
    public class ConsiderationDeco
    {
        public Consideration con;
        public float weight = 1f;

        public string name => con.name;
        public float Score(IContext ctx) => con.Score(ctx);
    }

    // Decision 做出决定的，不要在里面 Tick 里面再做判断分支
    // 决定应该由 AgentAI 评分来决定，Decison 只开始执行和退出  
    public abstract class Decision : ScriptableObject
    {
        public Precondition[] preconditions;
        public ConsiderationDeco[] considerations;
        public float cooldown = -1f;
        public bool interruptable = true;

        // public abstract System.Type ActionObjType();
        public virtual bool CanInterrupt(IContext ctx) => interruptable;
        // public virtual Status Execute(IContext ctx, float dt) { return Status.FINISHED; }
        // public virtual void Enter(IContext ctx) {}
        // public virtual void Exit(IContext ctx) {}
        // public abstract IAction GetAction(IContext ctx);
        // public virtual List<IAction> GetActions(IContext ctx) => null;
        public abstract bool Handle(IContext ctx, System.Action cbk);
    }
}
