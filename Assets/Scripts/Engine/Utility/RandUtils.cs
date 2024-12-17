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

    public static bool FlipCoin(int val)
    {
        return Rand(val) == 0;
    }
}