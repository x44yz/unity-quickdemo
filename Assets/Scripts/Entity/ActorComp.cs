using System;
using UnityEngine;

public abstract class ActorComp : MonoBehaviour 
{
    public static ResSystem sRes => GameMgr.Inst.sRes;
    // public static EntitySystem sEntity => GameMgr.Inst.sEntity;
    public static InputSystem sInput => GameMgr.Inst.sInput;

    protected Actor actor;
    
    public bool _active;
    public bool active
    {
        get { return _active; }
        set { _active = value; }
    }

    public virtual void Init(Actor actor)
    {
        this.actor = actor;
        active = true;
    }
    
    public virtual void Tick(float dt)
    {

    }
}