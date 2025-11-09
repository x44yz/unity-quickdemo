using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public partial class Entity : GameBehaviour
{
    public int hp
    {
        get { return GetStatIntVal(Stat.Hp); }
        set { SetStatBaseVal(Stat.Hp, value); }
    }

    public int maxHp
    {
        get { return GetStatIntVal(Stat.MaxHp); }
        set { SetStatBaseVal(Stat.MaxHp, value); }
    }

    public virtual bool CanAttack()
    {
        return gameObject.activeSelf && IsDead() == false;
    }

    public virtual bool IsDead() => hp <= 0;
    public virtual GameObject GetProjectilePRB() => null;
    // public virtual TargetScope GetAttackTargetScope() => TargetScope.One;

    public virtual Entity GetEnemy() => null;
    public virtual Vector3 GetAttackWpos() => Vector3.zero;
    public virtual Entity GetSourceActor() => null;
    
    public virtual void TakeDamage(Entity source, int damage)
    {
        if (IsDead())
            return;

        // 无敌的
        if (HasStatus(Status.Invincible))
            return;

        ModifyHp(-damage);
        Debug.Log($"{dbgName} take damage {damage} > hp:{hp}");

        if (hp <= 0)
        {
            StartDeath();
            return;
        }
    }

    public virtual void StartDeath()
    {
        DestroySelf();
    }
}