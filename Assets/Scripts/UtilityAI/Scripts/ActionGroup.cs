using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    [CreateAssetMenu(fileName = "AG_XXX", menuName = "AI/ActionGroup")]
    public class ActionGroup : ScriptableObject
    {
        public Action[] actions;

        public int len => actions != null ? actions.Length : 0;
    }
}
