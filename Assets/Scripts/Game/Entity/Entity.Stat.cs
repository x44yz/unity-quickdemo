using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public partial class Entity : GameBehaviour
{
    public Dictionary<Stat, StatData> statDatas = new Dictionary<Stat, StatData>();

    public bool HasStat(Stat stat)
    {
        return statDatas.ContainsKey(stat);
    }

    public virtual float GetStatVal(Stat stat)
    {
        var sv = GetStatData(stat);
        return sv != null ? sv.GetValue() : 0f;
    }

    public int GetStatIntVal(Stat stat)
    {
        return (int)GetStatVal(stat);
    }

    public StatData GetStatData(Stat stat, bool log = true)
    {
        StatData sf = null;
        if (statDatas.TryGetValue(stat, out sf))
        {
            return sf;
        }
        if (log)
            Debug.LogError($"{dbgName} cant find stat > {stat}");
        return null;
    }

    public StatData GetOrAddStatData(Stat stat)
    {
        StatData sf = GetStatData(stat, false);
        if (sf != null)
            return sf;
        
        sf = new StatData();
        sf.stat = stat;
        statDatas[stat] = sf;
        return sf;
    }


    public int GetMaxHp() => 100;

    public virtual int ModifyHp(int val)
    {
        Debug.Log($"{dbgName} modify hp > {val}");
        hp += val;
        hp = Mathf.Clamp(hp, 0, GetMaxHp());
        return hp;
    }

    public float GetMoveSpd()
    {
        if (HasStatus(Status.Rooted) ||
            HasStatus(Status.Frozen) ||
            HasStatus(Status.Stunned))
            return 0f;

        return GetStatVal(Stat.MoveSpeed);
    }

    public float GetAttackSpd() => GetStatVal(Stat.AttackSpeed);
    public float GetAttackInterval() => 1f / GetAttackSpd();
    public int GetDamage() => GetStatIntVal(Stat.Damage);

    protected virtual void OnStatValueChanged(Stat stat)
    {
    }

    public void SetStatBaseVal(Stat stat, float value)
    {
        StatData sd = GetOrAddStatData(stat);
        // Debug.Log($"{dbgName} stat base value before set > {sd}");
        sd.baseVal = value;
        // Debug.Log($"{dbgName} stat base value after set > {sd}");
        OnStatValueChanged(stat);
    }

    public void ModifyStat(Stat stat, ModifyValueType valueType, 
        float value, bool add)
    {
        var sd = GetOrAddStatData(stat);

        Debug.Log($"{dbgName} stat before modify > {sd}");
        if (valueType == ModifyValueType.Bonus)
        {
            ModifyStatBonus(stat, add ? value : -value);
        }
        else if (valueType == ModifyValueType.Scale)
        {
            float scale = add ? value : 1f / value;
            ModifyStatScale(stat, scale);
        }
        else
            Debug.LogError($"not implement value type > {valueType}");

        Debug.Log($"{dbgName} stat end modify > {sd}");
        OnStatValueChanged(stat);
    }

    private void ModifyStatBonus(Stat stat, float bonus)
    {
        var sd = GetOrAddStatData(stat);
        // if (sf == null)
        //     return;

        sd.ModifyBonus(bonus);
        Debug.Log($"{dbgName} modify {stat} bonus > {bonus}");

        // if (stat == Stat.MoveSpeed)
        //     moveSpdBonus += bonus;
        // else if (stat == Stat.AttackSpeed)
        //     attackSpdBonus += bonus;
        // else if (stat == Stat.Damage)
        //     damageBonus += bonus;
        // else
        // {
        //     Debug.LogError($"not implement stat > {stat}");
        //     return;
        // }

        // Debug.Log($"{dbgName} modify {stat} bonus > {bonus}");
    }

    private void ModifyStatScale(Stat stat, float scale)
    {
        if (scale <= 0f)
        {
            Debug.LogError($"{dbgName} cant modify {stat} > {scale} < 0f");
            return;
        }

        var sd = GetOrAddStatData(stat);
        // if (sf == null)
        //     return;

        sd.ModifyScale(scale);
        // if (stat == Stat.MoveSpeed)
        //     moveSpdScale *= scale;
        // else if (stat == Stat.AttackSpeed)
        //     attackSpdScale *= scale;
        // else if (stat == Stat.AttackSpeed)
        //     damageScale *= scale;
        // else
        // {
        //     Debug.LogError($"not implement stat > {stat}");
        //     return;
        // }
        Debug.Log($"{dbgName} modify {stat} scale > {scale}");
    }

    // protected float ModifyStatByBuffs(float baseVal, EffectStat effectStat)
    // {
    //     float bonus = 0f;
    //     float scale = 1f;
    //     foreach (var bff in buffs)
    //     {
    //         if (bff.isActive == false)
    //             continue;

    //         bonus += bff.GetEffectBonus(effectStat);
    //         scale *= bff.GetEffectScale(effectStat);
    //     }

    //     return (baseVal + bonus) * scale;
    // }

    public List<Buff> buffs;
    public void InitBuff()
    {
        buffs = new List<Buff>();
    }

    public void TickBuff(float dt)
    {
        foreach (var bf in buffs)
        {
            bf.Tick(dt);
        }

        // check inactived
        for (int i = buffs.Count - 1; i >= 0; --i)
        {
            var bf = buffs[i];
            if (bf.isActive)
                continue;

            buffs.RemoveAt(i);
        }
    }

    [HideInInspector]public List<string> forEnemyBuffs;
    public void AddBuff(string buffId, Entity caster)
    {
        // var cfg = sRes.GetBuffSO(buffId);
        // if ((cfg.targetScope == TargetScope.Hero && this is Rune) ||
        //     (cfg.targetScope == TargetScope.Enemy && this is Enemy))
        // {
        //     AddBuffToSelf(buffId, caster);
        // }
        // else if ((cfg.targetScope == TargetScope.Card && this is Enemy) ||
        //     (cfg.targetScope == TargetScope.Enemy && this is Rune))
        // {
        //     if (forEnemyBuffs == null)
        //         forEnemyBuffs = new List<string>();
        //     forEnemyBuffs.Add(buffId);
        // }
        // else
        //     Debug.LogError($"not implement buff target scope > {cfg.targetScope} - {entityType}");
    }

    private void AddBuffToSelf(string buffId, Entity caster)
    {
        // 检测层数  
        var hadBuff = GetBuff(buffId);
        if (hadBuff != null)
        {
            if (hadBuff.IsMaxStack())
            {
                Debug.LogWarning($"{dbgName} cant add more buff > {buffId}");
                return;
            }
            else
            {
                // 重置 buff 层数  
                hadBuff.AddStack(1);
                return;
            }
        }

        var bf = new Buff();
        buffs.Add(bf);
        Debug.Log($"{dbgName} add buff > {buffId}");

        bf.caster = caster;
        bf.owner = this;
        bf.Init(buffId);
        bf.Activate();
    }

    public Buff GetBuff(string buffId)
    {
        foreach (var bf in buffs)
        {
            if (bf.id == buffId)
                return bf;
        }
        return null;
    }

    public bool HasEffect<T>() where T : Effect
    {
        foreach (var bf in buffs)
        {
            if (bf.isActive == false)
                continue;

            foreach (var eff in bf.effects)
            {
                if (eff.isActive == false)
                    continue;
                if (eff.GetType() == typeof(T))
                    return true;
            }
        }
        return false;
    }

    public Dictionary<Status, int> statusMap;
    public Func<Status, bool> OnStatusAdd;
    public Func<Status, bool> OnStatusRemove;
    
    public void AddStatus(Status st)
    {
        // 注意优先级，互斥，共存关系
        if (statusMap == null)
            statusMap = new Dictionary<Status, int>();
        
        if (statusMap.ContainsKey(st) == false)
        {
            statusMap[st] = 0;
        }

        // 注意状态的互斥，优先级，并存

        statusMap[st] += 1;
        if (statusMap[st] == 1)
            OnStatusAdd?.Invoke(st);
    }

    public void RemoveStatus(Status st)
    {
        if (statusMap == null)
            return;

        if (statusMap.ContainsKey(st))
        {
            statusMap[st] = Mathf.Max(statusMap[st] - 1, 0);
        }
    }

    public bool HasStatus(Status st)
    {
        if (statusMap == null)
            return false;

        // 注意状态的互斥，优先级，并存
        int count = 0;
        statusMap.TryGetValue(st, out count);
        return count > 0;
    }
}