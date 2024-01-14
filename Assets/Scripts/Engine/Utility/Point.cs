using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// [Serializable]
public class Point : MonoBehaviour
{
    public static int POINT_UID_GEN = 1000;
    public static List<Point> points = new List<Point>();

    public PointId id;
    public Color color = Color.red;
    // public Transform trans;
    [Header("RUNTIME")]
    public int uid;
    public Vector3 pos => transform.position;

    [Button("Auto Name", EButtonEnableMode.Editor)]
    private void AutoName()
    {
        name = $"PT_{id}";
    }

    private void Awake()
    {
        uid = POINT_UID_GEN++;
        points.Add(this);
    }

    private void OnDestroy()
    {
        points.Remove(this);
    }

    public static Point GetPoint(PointId pid, bool log = false)
    {
        foreach (var p in points)
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