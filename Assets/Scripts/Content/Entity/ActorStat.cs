using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorStat : ActorComp
{
    public StatVal[] initStats;
    public StatVal[] initStatMaxs;

    [Header("RUNTIME")]
    public float[] stats;
    public int[] statMaxs;

    public Func<Stat, bool> onStatChanged;

    public override void Init(Actor actor)
    {
        base.Init(actor);

        stats = new float[(int)Stat.COUNT];
        for (int i = 0; i < initStats.Length; ++i)
        {
            stats[(int)initStats[i].stat] = initStats[i].val;
        }

        statMaxs = new int[(int)Stat.COUNT];
        for (int i = 0; i < initStatMaxs.Length; ++i)
        {
            statMaxs[(int)initStatMaxs[i].stat] = (int)initStatMaxs[i].val;
        }
    }

    public override void Tick(float dt)
    {

    }

    public void ModStat(Stat s, int v)
    {
        SetStat(s, GetStat(s) + v);
    }

    public void ModStat(Stat s, float v)
    {
        SetStat(s, GetStat(s) + v);
    }

    public void SetStat(Stat s, float v)
    {
        int max = GetStatMax(s);
        if (max > 0) v = Mathf.Min(v, max);
        if (v < 0) v = 0;
        stats[(int)s] = v;
        onStatChanged?.Invoke(s);
    }

    public void SetMaxStat(Stat s, int v)
    {
        statMaxs[(int)s] = v;
    }

    public float GetStat(Stat s)
    {
        return stats[(int)s];
    }

    public int GetStatMax(Stat s)
    {
        return statMaxs[(int)s];
    }

    public void ModStatSpds(StatVal[] spds, float dt)
    {
        if (spds == null || spds.Length == 0)
            return;
        for (int i = 0; i < spds.Length; ++i)
        {
            ModStat(spds[i].stat, spds[i].val * dt);
        }
    }

    public void ModStats(StatVal[] ss)
    {
        if (ss == null || ss.Length == 0)
            return;
        for (int i = 0; i < ss.Length; ++i)
        {
            ModStat(ss[i].stat, ss[i].val);
        }
    }

    public Dictionary<BuffKey, float> buffVals = new Dictionary<BuffKey, float>();
    public Dictionary<BuffKey, float> buffPVals = new Dictionary<BuffKey, float>();
    public void ApplyBuff(Buff bf)
    {
        bf.ApplyToActor(actor);
    }

    public void AddBuffVal(BuffKey k, float v, ValueType valueType = ValueType.CONSTANT)
    {
        float old = 0f;
        if (valueType == ValueType.CONSTANT)
        {
            buffVals.TryGetValue(k, out old);
            buffVals[k] = old + v;
        }
        else if (valueType == ValueType.PERCENT)
        {
            buffPVals.TryGetValue(k, out old);
            buffPVals[k] = old + v;
        }
        else
            Debug.LogError($"not implement value type > {valueType}");
    }

    public float GetBuffVal(BuffKey k, ValueType valueType = ValueType.CONSTANT)
    {
        float v = 0f;
        if (valueType == ValueType.CONSTANT)
            buffVals.TryGetValue(k, out v);
        else if (valueType == ValueType.PERCENT)
            buffPVals.TryGetValue(k, out v);
        else
            Debug.LogError($"not implement value type > {valueType}");
        return v;
    }

    public float GetBuffPVal(BuffKey k)
    {
        return GetBuffVal(k, ValueType.PERCENT);
    }

    public float FixWithBuff(float v, BuffKey k, float? min = null)
    {
        v += v * GetBuffPVal(k) / 100f;
        v += GetBuffVal(k);
        if (min != null)
            v = Mathf.Max(v, min.Value);
        return v;
    }

    public bool IsStat(Stat s)
    {
        return GetStat(s) > 0f;
    }

    public void SetStat(Stat s, bool v)
    {
        SetStat(s, v ? 1f : 0f);
    }
}