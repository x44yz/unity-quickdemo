using UnityEngine;

public class GameObjectReaction : DelayedReaction
{
    public GameObject gameObject;
    public bool activeState;

    protected override void OnReaction(IInteractSource s)
    {
        gameObject.SetActive(activeState);
    }
}