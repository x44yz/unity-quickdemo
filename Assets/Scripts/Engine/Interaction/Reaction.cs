using UnityEngine;
using System.Collections;

public abstract class Reaction : ScriptableObject
{
    protected Interactable owner;

    public void Init(Interactable owner)
    {
        this.owner = owner;
        OnInit();
    }

    protected virtual void OnInit()
    {
    }

    public void React(MonoBehaviour monoBehaviour, IInteractSource s)
    {
        OnReaction(s);
    }

    protected abstract void OnReaction(IInteractSource s);
}
