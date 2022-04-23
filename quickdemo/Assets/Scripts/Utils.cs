using System;
using UnityEngine;

namespace QuickDemo
{
    public static class Utils
    {
        public static Vector3 Vector3ZeroY(Vector3 v3)
        {
            v3.y = 0;
            return v3;
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
    }
}
