using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum EffectType
// {
//     Positive = 0,
//     Negative = 1,
// }

public abstract class Effect
{
    public virtual Stat GetEffectStat() => Stat.MoveSpeed;
    public virtual Status GetApplyStatus() => Status.None;

    public Entity owner { get; set; }
    // public int stack { get; set; } // 叠加的层数
    // public virtual float GetBonus() => 0f;
    // public virtual float GetScale() => 1f;
    public bool isActive { get; protected set; }

    public virtual void Init(EffectSO cfg)
    {
    }

    public void Tick(float dt)
    {
        if (isActive == false)
            return;

        OnTick(dt);
    }

    // protected virtual void ApplyInstant(Entity target)
    // {
    // }

    public void Activate()
    {
        if (isActive)
        {
            Debug.LogWarning($"effect had actived");
            return;
        }

        isActive = true;
        OnActivate();
    }

    public void Deactivate()
    {
        if (!isActive)
        {
            Debug.LogWarning($"effect had deactived");
            return;
        }

        isActive = false;
        OnDeactivate();
    }

    // public void ModifyStack(int val)
    // {
    //     stack = Mathf.Max(0, stack + val);
    //     if (stack <= 0)
    //         Deactivate();
    // }

    protected virtual void OnTick(float dt) {}
    protected virtual void OnActivate() {}
    protected virtual void OnDeactivate() {}
}
