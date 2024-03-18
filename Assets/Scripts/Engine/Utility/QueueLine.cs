using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QueueLine : MonoBehaviour
{
    public Color lineColor;
    public float lineThickness;
    public float subPGap;
    public Color subPColor;
    public float subPRadius;

    [Header("RUNTIME")]
    public Point[] points;
    public List<Vector3> subPs;

    public void Init()
    {
        points = GetComponentsInChildren<Point>();

        subPs = new List<Vector3>();
        float len = 0f;
        for (int i = 1; i < points.Length; ++i)
        {
            var p0 = points[i - 1];
            var p1 = points[i];
            var dir = p1.pos - p0.pos;
            float prev = len;
            len = dir.magnitude - prev;
            var dirN = dir.normalized;
            int count = (int)(len / subPGap) + 1;
            len = subPGap - (len - (count - 1) * subPGap);
            for (int j = 0; j < count; ++j)
            {
                var pos = p0.pos + dirN * (prev + j * subPGap);
                subPs.Add(pos);
            }
        }
    }

    public Vector3 GetSubPos(int idx)
    {
        if (idx < 0 || idx >= subPs.Count)
            return subPs[subPs.Count - 1];
        return subPs[idx];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // points = GetComponentsInChildren<Point>();

        if (points != null)
        {
            Handles.color = lineColor;
            for (int i = 1; i < points.Length; ++i)
            {
                var p0 = points[i - 1];
                var p1 = points[i];
                Handles.DrawLine(p0.pos, p1.pos, lineThickness);
            }
        }

        if (subPs != null)
        {
            Gizmos.color = subPColor;
            for (int i = 0; i < subPs.Count; ++i)
            {
                Gizmos.DrawSphere(subPs[i], subPRadius);
            }
        }
    }
#endif
}
