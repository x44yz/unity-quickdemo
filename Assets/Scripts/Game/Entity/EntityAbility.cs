using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAbility : MonoBehaviour
{
    public static ResSystem sRes => GameMgr.Inst.sRes;
    public static PageHUD hud => GameMgr.Inst.hud;
    public static TimeSystem sTime => GameMgr.Inst.sTime;
    public static Player plr => GameMgr.Inst.plr;

    [SerializeField]private bool active;
    public bool IsActive => active;
    protected Entity owner;

    public virtual void Init(Entity et)
    {
        this.owner = et;
    }

    public virtual void Reset()
    {
    }

    public virtual void Tick(float dt)
    {
    }

    public virtual void Activate()
    {
        active = true;
    }

    public virtual void Deactivate()
    {
        active = false;
    }

    // public void HandleAction(ActionId aid)
    // {
    //     if (active == false)
    //         return;
    //     OnHandleAction(aid);
    // }

    // protected virtual void OnHandleAction(ActionId aid)
    // {
    // }
}
