
public class ConditionReaction : Reaction
{
    public Condition condition;
    public bool satisfied;

    protected override void OnReaction(IInteractSource s)
    {
        condition.satisfied = satisfied;
    }
}
