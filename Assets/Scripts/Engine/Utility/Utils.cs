using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static partial class Utils
{
    public static Vector3 Truncate(this Vector3 v, float maxLength)
    {
        float maxLengthSquard = maxLength * maxLength;
        if (v.sqrMagnitude <= maxLengthSquard)
            return v;
        v = v.normalized * maxLength;
        return v;
    }

    public static Vector3 SetX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 SetY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 SetZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    public static float ZeroYLength(this Vector3 v)
    {
        v.y = 0f;
        return v.magnitude;
    }

    // sqr - squared
    public static float ZeroYSqrLength(this Vector3 v)
    {
        v.y = 0f;
        return v.sqrMagnitude;
    }

    public static Vector3 Vector3ZeroY(Vector3 v3)
    {
        return Vector3Y(v3, 0);
    }

    public static Vector3 Vector3MaxY(Vector3 v, float y)
    {
        return Vector3Y(v, Mathf.Max(v.y, y));
    }

    public static Vector3 Vector3MinY(Vector3 v, float y)
    {
        return Vector3Y(v, Mathf.Min(v.y, y));
    }

    public static Vector3 Vector3X(Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 Vector3Y(Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 Vector3Z(Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T ret = go.GetComponent<T>();
        if (ret == null)
        {
            ret = go.AddComponent<T>();
        }
        return ret;
    }

    public static bool IsZero(this Vector3 v3)
    {
        return Mathf.Approximately(v3.sqrMagnitude, 0f);
    }

    public static bool IsZero(this float v)
    {
        return Mathf.Approximately(v, 0f);
    }

    public static bool IsEqual(this float v1, float v2)
    {
        return IsZero(v1 - v2);
    }

    

    // Path find
    public static List<T> BFSFind<T>(IGraph graph, T from, T to, bool debugLog = false) where T : UnityEngine.Object
    {
        if (graph == null || from == null || to == null)
        {
            Debug.LogError("[BFS]]failed because graph is null");
            return null;
        }
        if (from == null)
        {
            Debug.LogError("[BFS]]failed because from point is null");
            return null;
        }
        if (to == null)
        {
            Debug.LogError("[BFS]]failed because to point is null");
            return null;
        }
        if (!graph.HasPoint(from))
        {
            Debug.Log("[BFS]graph doesn't contain from point");
            return null;
        }
        if (!graph.HasPoint(to))
        {
            Debug.Log("[BFS]graph doesn't contain to point");
            return null;
        }

        var visited = new HashSet<T>();
        visited.Add(from);

        var frontiers = new Queue<T>();
        frontiers.Enqueue(from);
        
        Dictionary<T, T> parents = new Dictionary<T, T>();
        parents[from] = null;

        while (frontiers.Count > 0)
        {
            T current = frontiers.Dequeue();
    
            if (current == to)
            {
                break;
            }

            foreach (var neighbor in graph.GetNeighbors(current))
            {
                if (visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                frontiers.Enqueue(neighbor);
                parents[neighbor] = current;

                if (debugLog)
                    Debug.Log($"[BFS]{neighbor.name} parent is {current.name}");
            }
        }

        var path = new List<T>();
        var parent = to;
        while (parent != null)
        {
            path.Add(parent);
            parent = parents[parent];
        }
        path.Reverse();

        if (debugLog)
            Debug.Log($"[BFS]path > {ArrayToString(path.ToArray())}");
        return path;
    }

    public static List<T> ObjToList<T>(object obj)
    {
        switch (obj)
        {
            case List<T> list:
                return list;
            case IEnumerable<T> genericEnumerable:
                return genericEnumerable.ToList();
            case IEnumerable enumerable:
                return enumerable.OfType<T>().ToList();
            default:
                return new List<T>(0);
        }
    }

    public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}