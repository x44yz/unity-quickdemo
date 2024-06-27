using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public static ResSystem sRes => GameMgr.Inst.sRes;
    public static CameraSystem sCamera => GameMgr.Inst.sCamera;
    public static TimeSystem sTime => GameMgr.Inst.sTime;
    public static PageHUD hud => GameMgr.Inst.hud;
    public static Player plr => GameMgr.Inst.plr;
    public static InputSystem sInput => GameMgr.Inst.sInput;

    public static int ENTITY_UID_GEN = 1000;

    [Header("RUNTIME")]
    public int uid = Defs.INVALID_UID;
    public EntityType entityType = EntityType.INVALID;
    public int id;
    protected List<EntityAbility> abilities;

    public Vector2 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector3 forward
    {
        get { return transform.forward; }
        set { transform.forward = value; }
    }

    public virtual bool isActive => true;

    public virtual void Init(int id)
    {
        uid = ENTITY_UID_GEN++;
        this.id = id;
        // if (id == EntityId.NONE)
        //     Debug.LogError($"{name} invalid entity id > {id}");
        name = $"{name}_{id}_{uid}";

        abilities = new List<EntityAbility>();
        var abs = GetComponentsInChildren<EntityAbility>();
        if (abs != null)
        {
            abilities.AddRange(abs);
            foreach (var ab in abs)
            {
                ab.Init(this);
            }
        }
    }

    public virtual void Reset()
    {
        if (abilities != null)
        {
            foreach (var ab in abilities)
            {
                ab.Reset();
            }
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void Tick(float dt)
    {
        if (!isActive)
            return;

        TickAbilities(dt);
    }

    protected void TickAbilities(float dt)
    {
        if (abilities != null)
        {
            foreach (var ab in abilities)
                ab.Tick(dt);
        }
    }

    public T GetAbility<T>(bool log = true) where T : EntityAbility
    {
        var ab = gameObject.GetComponent<T>();
        if (ab == null && log)
            Debug.Log($"{name} cant get ability > {typeof(T)}");
        return ab;
    }

    public T AddAbility<T>() where T : EntityAbility
    {
        var ab = gameObject.GetOrAddComponent<T>();
        ab.Init(this);
        return ab;
    }

    private Collider[] colliders = null;
    public Collider[] GetColliders(bool force = false)
    {
        if (colliders == null || force)
            colliders = GetComponentsInChildren<Collider>();
        return colliders;
    }
}
