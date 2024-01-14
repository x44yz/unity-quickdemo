using UnityEngine;

public class EffectData
{
    public virtual void Init(Effect eff, Entity caster, Entity target) {}
    public virtual void Tick(float dt) {}
    public virtual bool IsDone() { return true; }
}

// public class EDKnockBack : EffectData
// {
//     private Entity target;
//     private float accel;
//     private float spd;
//     private Vector3 velocity;

//     public  override void Init(Effect eff, Entity caster, Entity target)
//     {
//         this.target = target;

//         var eknockBack = eff as EKnockBack;
//         var dir = GetForceDir(caster, target);
//         spd = eknockBack.force / target.mass;
//         accel = -eknockBack.force / target.mass;
//         velocity = dir * spd;
//         target.forward = -dir;
//     }

//     public override void Tick(float dt)
//     {
//         spd += accel * dt;
//         if (spd <= 0f)
//         {
//             return;
//         }

//         velocity = velocity.normalized * spd;
//         target.pos += velocity * dt;
//     }

//     public override bool IsDone()
//     {
//         return spd <= 0f;
//     }

//     private Vector3 GetForceDir(Entity caster, Entity target)
//     {
//         if (caster.entityType == EntityType.ITEM)
//         {
//             var itemType = caster.itemType;
//             if (itemType == ItemType.PROJECTILE)
//             {
//                 var abProj = caster.GetAbility<AProjectile>();
//                 return abProj.velocity.ZeroY();
//             }
//             else
//                 Debug.LogError($"not implement itemType > {itemType}");
//         }
//         else if (caster.entityType == EntityType.ACTOR)
//         {
//             return (target.pos - caster.pos).ZeroY();
//         }
//         else
//             Debug.LogError($"not implement entity type > {caster.entityType}");
//         return Vector3.zero;
//     }
// } 

// public class EDStun : EffectData
// {
//     private float tick = 0f;
//     public override void Init(Effect eff, Entity caster, Entity target)
//     {
//         var estun = eff as EStun;
//         tick = estun.time;
//     }

//     public override void Tick(float dt)
//     {
//         tick -= dt;
//     }

//     public override bool IsDone()
//     { 
//         return tick <= 0f; 
//     }
// }