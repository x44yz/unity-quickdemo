using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickVal
{
    public float current;
    public float last;

    public bool IsAtRange(float v)
    {
        return last < v && current >= v;
    }

    public void Reset()
    {
        current = 0f;
        last = 0f;
    }

    public void Set(float v)
    {
        last = current;
        current = v;
    }

    public static bool operator >=(TickVal a, float b)
    {
        return a.current >= b;
    }

    public static bool operator <=(TickVal a, float b)
    {
        return a.current <= b;
    }

    public static bool operator >(TickVal a, float b)
    {
        return a.current > b;
    }

    public static bool operator <(TickVal a, float b)
    {
        return a.current < b;
    }

    public static TickVal operator -(TickVal a, float b)
    {
        a.Set(a.current - b);
        return a;
    }

    public static TickVal operator +(TickVal a, float b)
    {
        a.Set(a.current + b);
        return a;
    }
}
