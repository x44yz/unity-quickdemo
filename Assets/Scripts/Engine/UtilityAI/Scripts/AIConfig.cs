using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "UtilityAI/AIConfig")]
    public class AIConfig : ScriptableObject
    {
        public Decision[] decisions;
        // public DecisionGroup[] decisionGroups;
        [Header("DEBUG")]
        public Decision soloDecision;
        public Decision[] mutexDecisions;

        public int len
        {
            get {
                int total = 0;
                if (decisions != null)
                    total += decisions.Length;
                // if (decisionGroups != null)
                // {
                //     foreach (var k in decisionGroups)
                //         total += k.len;
                // }
                return total;
            }
        }
    }
}
