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

        public static Vector3 RandPos(Vector3 pt, float range)
        {
            float dist = Rand(0f, range);
            float rot = Rand(0f, 360f);
            var dir = Quaternion.AngleAxis(rot, Vector3.up) * Vector3.right;
            return pt + dir.normalized * dist;
        }

        public static bool IsEqual(float v1, float v2)
        {
            return IsZero(v1 - v2);
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

        // OnGUI
        public static string GUIButtonText(string text, int size = 40)
        {
            return $"<size={size}>{text}</size>";
        }

        public static GUILayoutOption[] GUIButtonOptions(float height)
        {
            return new GUILayoutOption[]{
                GUILayout.Height(height)
            };
        }

        public static GUILayoutOption[] DefaultGUIButtonOptions
        {
            get { return GUIButtonOptions(70); }
        }
    }
}
