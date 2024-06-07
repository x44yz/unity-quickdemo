using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.AI;

[DisallowMultipleComponent]
public class ActorMotion : ActorComp
{
    // public LayerMask layerMask;
    // public float movePowerDrainSpd;

    [Header("RUNTIME")]
    public Vector2? moveToPos;
    // public Vector2 lastDir;
    public NavMeshAgent agent;

    public bool isMoving => moveToPos != null;

    private System.Action onMoveEnd;
    public System.Action globalMoveEndCallback;

    public override void Init(Actor actor)
    {
        base.Init(actor);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public override void Tick(float dt)
    {
        if (moveToPos != null)
        {
            // Vector2 dir = (moveToPos.Value - actor.pos);
            // // actor.forward = dir.ZeroZ();
            // Vector2 dist = dir.normalized * actor.walkSpd * dt;
            // if (dist.sqrMagnitude > dir.sqrMagnitude)
            // {
            //     dist = dir;
            //     // Debug.Log($"[TEST]{name} reach to pos > {moveToPos.Value}");
            //     moveToPos = null;
            // }
            // actor.pos += dist;
            // Debug.Log($"xx-- remin > {agent.remainingDistance}");
            if (agent.remainingDistance <= 0.01f)
            {
                moveToPos = null;
            }

            if (moveToPos == null)
            {
                // CheckCollide();
                onMoveEnd?.Invoke();
                globalMoveEndCallback?.Invoke();
            }
        }
    }

    public void MoveToPos(Vector2 p, System.Action cb = null)
    {
        // Debug.Log($"xx-- set move to pos > {p}");
        moveToPos = p;
        onMoveEnd = cb;
        agent.SetDestination(moveToPos.Value);
    }

    public void StopMove(bool callback)
    {
        moveToPos = null;
        if (callback && onMoveEnd != null)
            onMoveEnd.Invoke();
        onMoveEnd = null;
    }

    public bool IsAtPoint(Vector2 p, float dist = 0.1f)
    {
        // if (p == null)
        // {
        //     Debug.LogWarning($"[TEST]point is null");
        //     return false;
        // }

        // if (Mathf.Abs(actor.pos.y - p.y) > 0.01f)
        //     return false;

        float d = Vector2.Distance(actor.pos, p);
        return d <= dist;
    }
}