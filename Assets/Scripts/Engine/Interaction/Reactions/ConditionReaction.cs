
public class ConditionReaction : Reaction
{
    public Condition condition;
    public bool satisfied;

    protected override void OnReaction()
    {
        condition.satisfied = satisfied;
    }
}
