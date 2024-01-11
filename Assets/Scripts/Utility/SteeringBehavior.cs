using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SteeringBehavior
{
    public static Vector3 SteeringToVelocity(ISteeringAgent agent, Vector3 steering, float dt)
    {
        steering = steering.Truncate(agent.steeringMaxForce);
            
        var accel = steering / agent.steeringMass;
        var velocity = agent.steeringVelocity + accel * dt;
        velocity = velocity.Truncate(agent.steeringMaxSpeed);
        return velocity;
    }

    public static Vector3 Arrival(ISteeringAgent agent, Vector3 targetPos)
    {
        Vector3 dir = (targetPos - agent.steeringPos).ZeroY();
        var desiredVelocity = Vector3.zero;

        float dist = dir.magnitude;
        if (dist <= agent.steeringStopRadius)
        {
            return Vector3.zero;
        }

        if (dist < agent.steeringSlowRadius)
            desiredVelocity = dir.normalized * agent.steeringMaxSpeed * (dist / agent.steeringSlowRadius);
        else
            desiredVelocity = dir.normalized * agent.steeringMaxSpeed;

        var steering = desiredVelocity - agent.steeringVelocity;
        return steering;
    }

    public static Vector3 Avoidance(ISteeringAgent agent, List<ISteeringAgent> avoidAgents)
    {
        var nvel = agent.steeringVelocity.normalized;
        var ahead = agent.steeringPos + nvel * agent.steeringAvoidAHead;
        var ahead2 = agent.steeringPos + nvel * agent.steeringAvoidAHead * 0.5f;

        // find most threatening obstacle
        ISteeringAgent obstacle = null;
        float obstacleDist = float.MaxValue;
        foreach (var a in avoidAgents)
        {
            if (a == null || a == agent || a.steeringNeedAvoid == false)
                continue;
            
            float dist = (ahead - a.steeringPos).ZeroYLength();
            float checkRadius = a.steeringCollisionRadius + agent.steeringCollisionRadius;
            if (dist > checkRadius)
            {
                dist = (ahead2 - a.steeringPos).ZeroYLength();
                if (dist > checkRadius)
                    continue;
            }

            dist = (agent.steeringPos - a.steeringPos).ZeroYLength();
            if (obstacle == null || dist < obstacleDist)
            {
                obstacle = a;
                obstacleDist = dist;
            }
        }

        if (obstacle == null)
            return Vector3.zero;

        var avoidance = (ahead - obstacle.steeringPos).ZeroY();
        avoidance = avoidance.normalized * agent.steeringAvoidForce;
        // Debug.DrawLine(obstacle.steeringPos, obstacle.steeringPos + avoidance, Color.red, 0.1f);
        return avoidance;
    }

    public static Vector3 Separation(ISteeringAgent agent, List<ISteeringAgent> sepAgents)
    {
        Vector3 force = Vector3.zero;
        int neighborCount = 0;
        foreach (var a in sepAgents)
        {
            if (a == null || a == agent)
                continue;

            var dist = (a.steeringPos - agent.steeringPos).ZeroYLength();
            if (dist > agent.steeringSeparationRadius)
                continue;

            neighborCount += 1;
            force += a.steeringPos - agent.steeringPos;
        }

        if (neighborCount > 0)
        {
            force /= neighborCount;
            force *= -1;
        }

        force = force.normalized * agent.steeringSeparationForce;
        return force;
    }

#if UNITY_EDITOR
    public static void DebugDrawArrival(ISteeringAgent agent, Vector3 targetPos, 
        Color slowRadiusColor, Color stopRadiusColor)
    {
        Handles.color = slowRadiusColor;
        Handles.DrawWireDisc(targetPos, Vector3.up, agent.steeringSlowRadius);

        Handles.color = stopRadiusColor;
        Handles.DrawWireDisc(targetPos, Vector3.up, agent.steeringStopRadius);
    }

    public static void DebugDrawAvoidance(ISteeringAgent agent, Color aheadColor)
    {
        Gizmos.color = aheadColor;
        Gizmos.DrawLine(agent.steeringPos, agent.steeringPos + agent.steeringVelocity.normalized * agent.steeringAvoidAHead);
    }
#endif
}
