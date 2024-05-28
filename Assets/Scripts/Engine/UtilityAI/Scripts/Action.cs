using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    public enum Status
    {
        WAITING,
        EXECUTING,
        FINISHED,
    }

    [System.Serializable]
    public class ConsiderationDeco
    {
        public Consideration con;
        public float weight = 1f;

        public string name => con.name;
        public float Score(IContext ctx) => con.Score(ctx);
    }

    public abstract class Action : ScriptableObject
    {
        public Precondition[] preconditions;
        public ConsiderationDeco[] considerations;
        public float cooldown = -1f;
        public bool interruptable = true;

        // public abstract System.Type ActionObjType();
        public virtual bool CanInterrupt(IContext ctx) => interruptable;
        public virtual Status Execute(IContext ctx, float dt) { return Status.FINISHED; }
        public virtual void Enter(IContext ctx) {}
        public virtual void Exit(IContext ctx) {}
    }
}
