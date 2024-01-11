using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntitySystem : MonoBehaviour
{
    public ResSystem sRes => GameMgr.Inst.sRes;
    public Player plr => GameMgr.Inst.plr;

    // public Point survivorSpawnPT;
    // public float spawnNPCInterval;
    // public int maxNPCCount;
    public Transform deactiveNPCRoot;

    [Header("RUNTIME")]
    // public float spawnNPCTick;
    public Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

    public void Init()
    {
        var ets = GameObject.FindObjectsOfType<Entity>();
        if (ets != null)
        {
            foreach (var et in ets)
            {
                et.Init(et.id);
            }
        }
    }

    public void Reset()
    {
        var keys = entities.Keys.ToList();
        foreach (var uid in keys)
        {
            var et = entities[uid];
            et.Reset();
        }
    }

    public void Tick(float dt)
    {
        plr.Tick(dt);

        var keys = entities.Keys.ToList();
        foreach (var k in keys)
        {
            var et = entities[k];
            if (et == plr)
                continue;
            et.Tick(dt);
        }
    }

    public void RegisterEntity(Entity et)
    {
        if (et == null)
        {
            Debug.LogError($"cant add null entity");
            return;
        }

        if (et.uid == Defs.INVALID_UID)
        {
            Debug.LogError($"enity uid is invalid");
            return;
        }

        if (entities.ContainsKey(et.uid))
        {
            Debug.LogError($"has register entity > {et.uid} - {et.name}");
            return;
        }

        entities[et.uid] = et;
        // Debug.LogError($"not implement entity type > {et.entityType}");
    }

    public void UnregisterEntity(Entity et)
    {
        if (et == null)
        {
            Debug.LogError($"cant remove null entity");
            return;
        }

        if (entities.ContainsKey(et.uid) == false)
        {
            Debug.LogError($"not register entity > {et.name}");
            return;
        }

        entities.Remove(et.uid);
    }

    public List<T> Find<T>(int id, Vector2 pos, float range) where T : Entity
    {
        var rt = new List<T>();
        foreach (var kv in entities)
        {
            if (kv.Value.id != id)
                continue;

            var tt = kv.Value as T;
            if (tt == null)
            {
                // Debug.LogError($"{kv.Value} not type > {typeof(T)}");
                continue;
            }

            var dist = pos - kv.Value.pos;
            if (dist.magnitude <= range)
                rt.Add(tt);
        }
        return rt;
    }

    public T FindNearest<T>(int id, Vector2 pos, float range, List<int> excludes = null) where T : Entity
    {
        var rt = Find<T>(id, pos, range);
        rt = GameUtils.Filter<T>(rt, excludes);
        return GameUtils.Nearest<T>(pos, rt);
    }

    public void SetAllActorAgentStop(bool v)
    {
        foreach (var kv in entities)
        {
            var et = kv.Value;
            var actor = et as Actor;
            if (actor == null)
                continue;

            actor.motion.agent.isStopped = v;
        }
    }
}
