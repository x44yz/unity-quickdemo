using UnityEngine;

namespace AI.Utility
{
    public abstract class Precondition : ScriptableObject
    {
        // 已经在 AgentAI 里面统一处理了取反，不要在
        // 派生中再处理  
        public bool isNot;

        public abstract bool IsTrue(IContext ctx);
    }
}
