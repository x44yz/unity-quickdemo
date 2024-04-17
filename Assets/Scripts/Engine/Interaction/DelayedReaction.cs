using UnityEngine;
using System.Collections;

public abstract class DelayedReaction : Reaction
{
    public float delay;
    protected WaitForSeconds wait;

    protected override void OnInit()
    {
        wait = new WaitForSeconds(delay);
    }

    public new void React(MonoBehaviour monoBehaviour, IInteractSource s)
    {
        monoBehaviour.StartCoroutine(ReactCoroutine(s));
    }

    protected IEnumerator ReactCoroutine(IInteractSource s)
    {
        yield return wait;
        OnReaction(s);
    }
}
