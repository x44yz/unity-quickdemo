using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

[DisallowMultipleComponent]
public class Actor : Entity
{
    public SpriteRenderer spr;

    [Header("ACTOR-RUNTIME")]
    // public int uid;
    public bool debug;
    public bool isKill; // 清理
    public Inventory bag;

    public virtual string aniAssetLabel => "";
    public virtual float walkSpd => 0.5f;
    public virtual float rotSpd => 180f;

    protected List<ActorComp> comps = new List<ActorComp>();
    public ActorStat stat => GetComp<ActorStat>();
    public ActorMotion motion => GetComp<ActorMotion>();
    public ActorANI ani => GetComp<ActorANI>();

    public override void Init(int id)
    {
        base.Init(id);

        // entityType = EntityType.ACTOR;
        // uid = ACTOR_UID_GEN++;
        // gActors[uid] = this;
        bag = GetComponent<Inventory>();
        if (bag == null)
            Debug.LogError($"cant find bag on actor > {name}");

        var cps = GetComponentsInChildren<ActorComp>();
        for (int i = 0; i < cps.Length; ++i)
        {
            var cp = cps[i];
            cp.Init(this);
            comps.Add(cp);
        }
    }

    public override void Reset()
    {
        base.Reset();

        bag.ClearAll();
    }

    public override void Tick(float dt)
    {
        foreach (var cp in comps)
        {
            if (cp.active == false)
                continue;

            cp.Tick(dt);
        }
    }

    public T AddComp<T>() where T : ActorComp
    {
        var cp = GetComp<T>(false);
        if (cp != null)
            return cp;
        cp = gameObject.AddComponent<T>();
        comps.Add(cp);
        cp.Init(this);
        return cp;
    }

    public T GetComp<T>(bool log = true) where T : ActorComp
    {
        foreach (var cp  in comps)
        {
            if (cp.GetType() == typeof(T))
                return (T)cp;
        }
        if (log)
            Debug.LogError($"cant find comp > {typeof(T)}");
        return null;
    }
}






