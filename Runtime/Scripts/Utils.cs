using System;
using UnityEngine;
using System.Text;

namespace QuickDemo
{
    public static class Utils
    {
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
        public static string GUIText(string text, int size = 40)
        {
            return $"<size={size}>{text}</size>";
        }

        public static GUILayoutOption[] GUIOptions(float height)
        {
            return new GUILayoutOption[]{
                GUILayout.Height(height)
            };
        }

        public static GUILayoutOption[] GUIDefaultButtonOptions
        {
            get { return GUIOptions(70); }
        }

        public static GUILayoutOption[] GUIDefaultLabelOptions
        {
            get { return GUIOptions(70); }
        }

        public static StringBuilder sb = new StringBuilder();
        public static string GetGameObjectPath(GameObject obj)
        {
            if (obj == null)
                return string.Empty;

            sb.Clear();
            sb.Insert(0, obj.transform.name);
            var parent = obj.transform.parent;
            while (parent != null)
            {
                sb.Insert(0, "/");
                sb.Insert(0, parent.name);
                parent = parent.parent;
            }
            return sb.ToString();
        }
    }
}
