using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public partial class Entity : GameBehaviour
{
    public EntityType entityType;

    [HorizontalLine(color: EColor.Red)]
    public int uid = Defs.INVALID_UID;
    protected bool willDestory = false;

    public bool isAlive => !isDestoryed;
    public bool isDestoryed => willDestory;

    public override Vector3 pos
    {
        set { 
            base.pos = value;
            var sprR = GetSpriteRenderer();
            if (sprR != null)
                sprR.sortingOrder = (int)(-value.y * 100f);
        }
    }

    protected virtual SpriteRenderer GetSpriteRenderer() => null;

    private void Awake() 
    {
        willDestory = false;
        sEntity.Register(this);
        name = $"{name}_{uid}";
        OnCreate();

        InitBuff();
    }

    public void DestroySelf()
    {
        if (willDestory)
        {
            Debug.LogError($"{dbgName} had destoryed");
            return;
        }

        OnDestroySelf();
        sEntity.Unregister(this);
        willDestory = true;
        gameObject.SetActive(false);
        Destroy(gameObject, 0.1f);
    }

    private void Update() 
    {
        if (willDestory)
            return;

        if (game.world.state != World.State.Running)
            return;

        float dt = Time.deltaTime;
        TickAction(dt);
        TickBuff(dt);

        OnTick(dt);
    }

    protected virtual void OnCreate() {}
    protected virtual void OnDestroySelf() {}
    protected virtual void OnTick(float dt) {}

    public float GetDist(Entity other)
    {
        if (other == null || other == this)
            return 0f;

        var dist = (other.pos - pos).SetZ(0f).magnitude;
        return dist;
    }

    public float GetSqrDist(Entity other)
    {
        if (other == null || other == this)
            return 0f;

        var dist = (other.pos - pos).SetZ(0f).sqrMagnitude;
        return dist;
    }

#region Action
    public bool dbgAction;
    public List<GAction> actions = new List<GAction>();
    public GAction curAction;

    public void TickAction(float dt)
    {
        if (curAction != null)
        {
            var ret = curAction.Execute(dt);
            if (ret != GActionResult.Running)
            {
                FinishAction();
            }
        }

        if (curAction == null)
        {
            StartAction();
        }
    }

    public void StartAction()
    {
        if (actions.Count == 0)
            return;

        curAction = actions[0];

        if (dbgAction)
            Debug.Log($"{name} start action > {curAction}");

        actions.RemoveAt(0);
        curAction.Bind(this);
        curAction.Start();
    }

    public void SetNextAction(GAction action)
    {
        if (dbgAction)
            Debug.Log($"{name} set next action > {action}");
        if (curAction != null)
            FinishAction();

        actions.Insert(0, action);
        StartAction();
    }

    public void QueueAction(GAction action)
    {
        if (action == null)
        {
            Debug.LogError("failed queue null action");
            return;
        }
        if (dbgAction)
            Debug.Log($"{name} queue action > {action}");
        actions.Add(action);
    }

    public void FinishAction()
    {
        if (curAction == null)
        {
            if (dbgAction)
                Debug.Log($"{name} finish action > none");
            return;
        }

        // 这种写法是防止在 finish 的回调中设置新 action
        // 又调用 finish
        var action = curAction;
        curAction = null;
        if (action != null)
        {
            if (dbgAction)
                Debug.Log($"{name} finish action > {action}");
            action.End();
        }
    }

    public bool IsAction<T>() where T : GAction
    {
        return curAction != null && curAction is T;
    }

    public void StopAndClearAllAction()
    {
        FinishAction();
        actions.Clear();
    }

    public bool HasAnyActions()
    {
        return curAction != null || actions.Count > 0;
    }
#endregion
}
