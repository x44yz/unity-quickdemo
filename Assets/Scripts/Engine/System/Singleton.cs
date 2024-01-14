using UnityEngine;
using System.Collections;

public class Singleton<T> where T : class, new()
{
    private static T _instance = null;

    private static object _lock = new object();

    public static T Inst
    {
        get
        {
            lock (_lock)
            {
                _instance = _instance ?? new T();
            }

            return _instance;
        }
    }
}