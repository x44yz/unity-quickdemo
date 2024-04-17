using UnityEngine;

public class ConditionCollection : ScriptableObject
{
    public string description;
    public Condition[] requiredConditions = new Condition[0];
    public ReactionCollection reactionCollection;

    public bool CheckAndReact(IInteractSource s)
    {
        for (int i = 0; i < requiredConditions.Length; i++)
        {
            if (!AllConditions.CheckCondition(requiredConditions[i]))
                return false;
        }

        if (reactionCollection)
            reactionCollection.React(s);

        return true;
    }
}