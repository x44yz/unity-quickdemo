
using System;
using System.Collections.Generic;
using UnityEngine;

// buff 是 effect 的管理，不参与 effect 的
public class Buff
{
    public static GameMgr game => GameMgr.Inst;

    public string id;

    public Entity caster;
    public Entity owner;

    private float life;
    private BuffLifeStackType lifeStackType;
    private int maxStack;

    private float tick;
    // private float duration;
    private int stack;
    public bool isActive { get; protected set; }
    public List<Effect> effects;

    public Buff()
    {
        effects = new List<Effect>();
        stack = 1;
        // duration = 0;
    }

    public void Init(string buffId)
    {
        id = buffId;

        var cfg = game.sRes.GetBuffSO(buffId);
        life = cfg.life;
        lifeStackType = cfg.lifeStackType;
        maxStack = cfg.maxStack;

        var effCfgs = cfg.effects;
        foreach (var effCfg in effCfgs)
        {
            var eff = Activator.CreateInstance(effCfg.GetInstType()) as Effect;
            eff.Init(effCfg);
            effects.Add(eff);
        }

        // duration = life;
    }

    public bool IsMaxStack()
    {
        return stack >= maxStack;
    }

    public void AddStack(int val)
    {
        if (val <= 0)
        {
            Debug.LogError($"{owner.dbgName} cant add buff {id} stack > {val}");
            return;
        }
        stack += val;
        Debug.Log($"{owner.dbgName} add buff {id} stack > {stack}/{val}");

        if (lifeStackType == BuffLifeStackType.ResetLife)
        {
            tick = 0f;
        }
        else if (lifeStackType == BuffLifeStackType.ExtendLife)
        {
            // nothing
        }
        else if (lifeStackType == BuffLifeStackType.ExtendAndResetLife)
        {
            tick = 0f;
        }
        // else if (lifeStackType == BuffLifeStackType.StandaloneLife)
        // {
        //     // 如果是分开的，那么就是重新创建
        //     Debug.LogError($"cant add stack > {lifeStackType}");
        // }
        else
            Debug.LogError($"not implement buff stack type > {lifeStackType}");
    }

    public void Tick(float dt)
    {
        if (!isActive)
            return;

        for (int i = 0; i < effects.Count; ++i)
        {
            var eff = effects[i];
            eff.Tick(dt);
        }

        tick += dt;
        if (tick >= life)
        {
            tick -= life;
            if (lifeStackType == BuffLifeStackType.ResetLife)
            {
                Deactivate();
            }
            else if (lifeStackType == BuffLifeStackType.ExtendLife ||
                lifeStackType == BuffLifeStackType.ExtendAndResetLife)
            {
                stack -= 1;
                Debug.Log($"{owner.dbgName} remove buff {id} stack > {stack}");
                if (stack <= 0)
                    Deactivate();
            }
        }
    }

    public void Activate()
    {
        Debug.Log($"{owner.dbgName} buff {id} activate");
        isActive = true;
        foreach (var eff in effects)
        {
            eff.owner = owner;
            eff.Activate();
        }
    }

    public void Deactivate()
    {
        Debug.Log($"{owner.dbgName} buff {id} deactivate");
        isActive = false;
        foreach (var eff in effects)
        {
            if (eff.isActive)
                eff.Deactivate();
        }
    }

    // public float GetEffectBonus(Stat effectStat)
    // {
    //     float bonus = 0f;
    //     foreach (var eff in effects)
    //     {
    //         if (eff.GetEffectStat() == effectStat)
    //             bonus += eff.GetBonus();
    //     }
    //     return bonus;
    // }

    // public float GetEffectScale(Stat effectStat)
    // {
    //     float scale = 1f;
    //     foreach (var eff in effects)
    //     {
    //         if (eff.GetEffectStat() == effectStat)
    //             scale *= eff.GetScale();
    //     }
    //     return scale;
    // }
}