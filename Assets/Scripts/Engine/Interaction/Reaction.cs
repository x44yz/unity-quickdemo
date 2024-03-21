using UnityEngine;
using System.Collections;

public abstract class Reaction : ScriptableObject
{
    public void Init()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
    }

    public void React(MonoBehaviour monoBehaviour)
    {
        OnReaction();
    }

    protected abstract void OnReaction();
}
