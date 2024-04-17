using UnityEngine;

public class BehaviourReaction : DelayedReaction
{
    public Behaviour behaviour;
    public bool enabledState;

    protected override void OnReaction(IInteractSource s)
    {
        behaviour.enabled = enabledState;
    }
}