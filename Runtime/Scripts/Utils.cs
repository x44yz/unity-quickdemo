using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace QuickDemo
{
    public static class Utils
    {
        public static Vector3 Truncate(this Vector3 v, float maxLength)
        {
            float maxLengthSquard = maxLength * maxLength;
            if (v.sqrMagnitude <= maxLengthSquard)
                return v;
            v = v.normalized * maxLength;
            return v;
        }

        public static Vector3 ZeroY(this Vector3 v)
        {
            v.y = 0f;
            return v;
        }

        public static float ZeroYLength(this Vector3 v)
        {
            v.y = 0f;
            return v.magnitude;
        }

        public static float ZeroYSquardLen(this Vector3 v)
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
        public static void GUILayoutLabel(string text, int size = 40, float height = 70)
        {
            GUILayout.Label(Utils.GUIText(text, size), GUIOptions(height));
        }

        public static bool GUILayoutButton(string text, int size = 40, float height = 70)
        {
            return GUILayout.Button(Utils.GUIText(text, size), GUIOptions(height));
        }

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

        // Time
        public static DateTime TimeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        public static double GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - TimeStampStartTime;
            return ts.TotalSeconds;
        }

        // String
        public static string ArrayToString<T>(T[] values, string separator = ",")
        {
            return string.Format("[{0}]", string.Join(separator, values.Select(x => x.ToString()).ToArray()));
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
    }
}
