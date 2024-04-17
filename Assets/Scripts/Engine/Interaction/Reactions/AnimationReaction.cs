using UnityEngine;

public class AnimationReaction : DelayedReaction
{
    public Animator animator;
    public string trigger;

    private int triggerHash;

    protected override void OnInit()
    {
        triggerHash = Animator.StringToHash(trigger);
    }

    protected override void OnReaction(IInteractSource s)
    {
        animator.SetTrigger(triggerHash);
    }
}
