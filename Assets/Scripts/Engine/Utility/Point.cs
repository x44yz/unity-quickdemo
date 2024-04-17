using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// [Serializable]
public class Point : MonoBehaviour
{
    public static int POINT_UID_GEN = 1000;
    public static List<Point> all = new List<Point>();

    public PointId id;
    public Color color = Color.red;
    // public Transform trans;
    [Header("RUNTIME")]
    public int uid;
    public Vector3 pos => transform.position;
    public Vector3 fwd => transform.forward;
    public Quaternion rot => transform.rotation;

    [Button("Auto Name", EButtonEnableMode.Editor)]
    private void AutoName()
    {
        name = $"PT_{id}";
    }

    private void Awake()
    {
        uid = POINT_UID_GEN++;
        all.Add(this);
    }

    private void OnDestroy()
    {
        all.Remove(this);
    }

    public static Point FindNearestPoint(PointId pid, Vector3 p, bool log = false)
    {
        Point rt = null;
        var pts = GetPoints(pid, log);
        if (pts != null)
        {
            float minDist = float.MaxValue;

            foreach (var pt in pts)
            {
                float dist = (pt.pos - p).sqrMagnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    rt = pt;
                }
            }
        }
        if (log && rt == null)
            Debug.LogError($"cant find nearest point > {pid}");
        return rt;
    }

    public static List<Point> GetPoints(PointId pid, bool log = false)
    {
        var pts = new List<Point>();
        foreach (var p in all)
        {
            if (p.id == pid)
            {
                pts.Add(p);
            }
        }
        if (log && pts.Count == 0)
            Debug.LogError($"cant find any point > {pid}");
        return pts;
    }

    public static Point GetPoint(PointId pid, bool log = false)
    {
        foreach (var p in all)
        {
            if (p.id == pid)
                return p;
        }
        if (log)
            Debug.LogError($"cant find point > {pid}");
        return null;
    }

    public static Vector3 GetPointPos(PointId pid, bool log = false)
    {
        var p = GetPoint(pid, log);
        return p ? p.pos : Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pos, 0.1f);
    }
}