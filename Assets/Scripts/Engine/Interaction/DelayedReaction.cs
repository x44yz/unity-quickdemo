using UnityEngine;
using System.Collections;

public abstract class DelayedReaction : Reaction
{
    public float delay;

    protected WaitForSeconds wait;

    public new void Init()
    {
        wait = new WaitForSeconds(delay);

        OnInit();
    }

    public new void React(MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(ReactCoroutine());
    }


    protected IEnumerator ReactCoroutine()
    {
        yield return wait;
        OnReaction();
    }
}
