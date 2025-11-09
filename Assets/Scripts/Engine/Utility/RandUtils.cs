using System;
using System.Collections.Generic;
using UnityEngine;

public static class RandUtils
{
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Rand(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // [0, 1)
    public static float Float()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }

    // [0, max)
    public static float Float(float max)
    {
        return UnityEngine.Random.Range(0f, max);
    }

    // [0, max)
    public static int Int(int max)
    {
        return UnityEngine.Random.Range(0, max);
    }

    // [min, max)
    public static int Int(int min, int max)
    {
        return min + Int(max - min);
    }

    //returns a uniformly distributed int in the range [min, max]
    public static int IntRange(int min, int max)
    {
        return min + Int(max - min + 1);
    }

    //returns a triangularly distributed int in the range [min, max]
    public static int NormalIntRange(int min, int max)
    {
        return min + (int)((Float() + Float()) * (max - min + 1) / 2f);
    }

    public static void InitSeed(int seed)
    {
        UnityEngine.Random.InitState(seed);
    }

    public static float Rand(float maxExclusive)
    {
        return Rand(0f, maxExclusive);
    }

    public static float Rand(float minInclusive, float maxExclusive)
    {
        return UnityEngine.Random.Range(minInclusive, maxExclusive);
    }

    public static int Rand(int maxExclusive)
    {
        return Rand(0, maxExclusive);
    }

    public static int Rand(int minInclusive, int maxExclusive)
    {
        return UnityEngine.Random.Range(minInclusive, maxExclusive);
    }

    public static T RandOne<T>(T[] ts)
    {
        if (ts == null || ts.Length == 0)
            return default(T);
        int idx = Rand(ts.Length);
        return ts[idx];
    }

    public static T RandOne<T>(List<T> ts)
    {
        if (ts == null || ts.Count == 0)
            return default(T);
        int idx = Rand(ts.Count);
        return ts[idx];
    }

    public static Vector3 RandXZ(float radius)
    {
        return new Vector3(Rand(-radius, radius), 0f, Rand(-radius, radius));
    }

    public static Vector3 RandXZ(float x, float z)
    {
        return new Vector3(Rand(-x * 0.5f, x * 0.5f), 0f, Rand(-z * 0.5f, z * 0.5f));
    }

    public static Vector3 RandRadiusAroundZ(float radius)
    {
        return RandRadiusAroundZ(radius, radius);
    }

    public static Vector3 RandRadiusAroundZ(float minRadius, float maxRadius)
    {
        // return RandPos(Vector3.zero, minRadius, maxRadius, Vector3.forward, 360f);
        float dist = Rand(minRadius, maxRadius);
        float rot = Rand(360f);
        var dir = Quaternion.AngleAxis(rot, Vector3.forward) * Vector3.up;
        return dir.normalized * dist;
    }

    public static Vector3 RandPos(Vector3 pt, float range)
    {
        return RandPos(pt, 0f, range);
    }

    public static Vector3 RandPos(Vector3 pt, float minRange, float maxRange)
    {
        return RandPos(pt, minRange, maxRange, Vector3.forward, 0f, 360f);
    }

    public static Vector3 RandPos(Vector3 pt, float range, Vector3 fw, float rot)
    {
        return RandPos(pt, 0f, range, fw, -rot * 0.5f, rot * 0.5f);
    }

    public static Vector3 RandPos(Vector3 pt, float minRange, float maxRange, Vector3 fw, float rot)
    {
        return RandPos(pt, minRange, maxRange, fw, -rot * 0.5f, rot * 0.5f);
    }

    public static Vector3 RandPos(Vector3 pt, float minRange, float maxRange,
                                Vector3 fw, float minRot, float maxRot)
    {
        float dist = Rand(minRange, maxRange);
        float rot = Rand(minRot, maxRot);
        var dir = Quaternion.AngleAxis(rot, Vector3.up) * fw;
        return pt + dir.normalized * dist;
    }

    public static bool FlipCoin(int v = 2)
    {
        return Rand(v) == 0;
    }
}