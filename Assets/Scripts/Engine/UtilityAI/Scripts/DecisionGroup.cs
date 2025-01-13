using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    [CreateAssetMenu(fileName = "DG_XXX", menuName = "UtilityAI/DecisionGroup")]
    public class DecisionGroup : ScriptableObject
    {
        public Decision[] decisions;

        public int len => decisions != null ? decisions.Length : 0;
    }
}
