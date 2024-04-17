using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "AI/AIConfig")]
    public class AIConfig : ScriptableObject
    {
        public Action[] actions;
        public ActionGroup[] actionGroups;
        [Header("DEBUG")]
        public Action soloAction;
        public Action[] mutexActions;

        public int len
        {
            get {
                int total = 0;
                if (actions != null)
                    total += actions.Length;
                if (actionGroups != null)
                {
                    foreach (var k in actionGroups)
                        total += k.len;
                }
                return total;
            }
        }
    }
}
