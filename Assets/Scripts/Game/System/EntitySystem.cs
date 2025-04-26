using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntitySystem : GameBehaviour
{
    public Dictionary<int, Entity> All = new Dictionary<int, Entity>();

    private int UID_GEN = 100;

    public void Init()
    {
    }

    public void Tick(float dt)
    {
    }

    public void ClearAll()
    {
        var keys = All.Keys.ToList();
        foreach (var uid in keys)
        {
            var et = All[uid];
            if (et == null)
                continue;
            Destroy(et.gameObject);
        }
        All.Clear();
    }

    public void Register(Entity et)
    {
        int uid = UID_GEN++;
        et.uid = uid;
        All[uid] = et;
    }

    public void Unregister(Entity et)
    {
        All.Remove(et.uid);
    }

    public Entity GetEntity(int uid)
    {
        Entity et = null;
        All.TryGetValue(uid, out et);
        return et;
    }

    public T Get<T>(int uid) where T : Entity
    {
        Entity et = null;
        All.TryGetValue(uid, out et);
        return et as T;
    }

    public List<T> GetAll<T>() where T : Entity
    {
        var rt = new List<T>();
        foreach (var kv in All)
        {
            if (kv.Value is T)
                rt.Add(kv.Value as T);
        }
        return rt;
    }
}