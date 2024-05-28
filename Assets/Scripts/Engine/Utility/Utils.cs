using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

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

    public static Vector3 ModX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 ModY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 ModZ(this Vector3 v, float z)
    {
        v.z = z;
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

    public static bool IsEqual(float v1, float v2)
    {
        return IsZero(v1 - v2);
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

    private static StringBuilder sb = new StringBuilder();
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

#region STRING
    public static string ArrayToString<T>(T[] values, string separator = ",")
    {
        return string.Format("[{0}]", string.Join(separator, values.Select(x => x.ToString()).ToArray()));
    }

    public static string[] ToStrArray(string str, char split = ';')
    {
        var rt = str.Split(new char[]{split}, StringSplitOptions.RemoveEmptyEntries);
        return rt;
    }

    public static float ToFloat(string str, float defaultValue = 0f)
    {
        if (string.IsNullOrEmpty(str) || !float.TryParse(str, out float value))
        {
            return defaultValue;
        }
        return value;
    }

    public static T ToVal<T>(string str, T defaultVal)
    {
        if (string.IsNullOrEmpty(str))
            return defaultVal;

        return (T)Convert.ChangeType(str, typeof(T));
    }

    public static T[] ToArray<T>(string str, char split = ';')
    {
        T[] result;
        if (string.IsNullOrEmpty(str))
        {
            result = new T[0];
        }
        else
        {
            var strResult = str.Split(new char[]{split}, 
                StringSplitOptions.RemoveEmptyEntries);
            result = new T[strResult.Length];
            for (var i = 0; i < strResult.Length; i++)
            {
                result[i] = ToVal<T>(strResult[i], default(T));
            }
        }
        return result;
    }

    public static int ToInt(string str, int defaultValue = 0)
    {
        if (string.IsNullOrEmpty(str) || !int.TryParse(str, out int value))
        {
            return defaultValue;
        }
        return value;
    }

    public static int[] ToIntArray(string str, char split = ';')
    {
        return ToArray<int>(str, split);
    }

    public static uint ToUInt(string str, uint defaultValue = 0)
    {
        if (string.IsNullOrEmpty(str) || !uint.TryParse(str, out uint value))
        {
            return defaultValue;
        }
        return value;
    }

    public static bool ToBool(string str, bool defaultValue = false)
    {
        if (str.Equals("FALSE") || str.Equals("0"))
        {
            return false;
        }
        if (str.Equals("TRUE") || str.Equals("1"))
        {
            return true;
        }

        if (string.IsNullOrEmpty(str) || !bool.TryParse(str, out var value))
        {
            return defaultValue;
        }
        return value;
    }

    public static uint[] ToUIntArray(string str, char splitChar = ';')
    {
        uint[] result;
        if (string.IsNullOrEmpty(str))
        {
            result = new uint[0];
        }
        else
        {
            var strResult = str.Split(new char[]{splitChar}, StringSplitOptions.RemoveEmptyEntries);
            result = new uint[strResult.Length];
            for (var i = 0; i < strResult.Length; i++)
            {
                result[i] = ToUInt(strResult[i]);
            }
        }
        return result;
    }

    public static string FromUIntArray(uint[] values, char splitChar = ';')
    {
        if (values == null || values.Length == 0)
            return string.Empty;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < values.Length; ++i)
        {
            sb.Append(values[i]);
            if (i + 1 < values.Length)
                sb.Append(splitChar);
        }
        return sb.ToString();
    }

    public static List<uint> ToUIntList(string str, char splitChar = ';')
    {
        uint[] array = ToUIntArray(str, splitChar);
        return array.ToList();
    }

    public static T ToEnum<T>(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            Debug.LogError($"[UTILS]failed ToEnum because str is null");
            return default(T);
        }

        Type enumType = typeof(T);
        try
        {
            return (T)Enum.Parse(enumType, str);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[UTILS]ToEnum {enumType}-{str} exception > {ex}");
            return default(T);
        }
    }

    public static T[] ToEnumArray<T>(string str, char split = ';')
    {
        T[] result;
        if (string.IsNullOrEmpty(str))
            result = new T[0];
        else
        {
            var strResult = str.Split(split);
            result = new T[strResult.Length];
            for (var i = 0; i < strResult.Length; i++)
            {
                result[i] = Utils.ToEnum<T>(strResult[i]);
            }
        }
        return result;
    }

    public static string FromUIntList(List<uint> values, char splitChar = ';')
    {
        if (values == null || values.Count == 0)
            return string.Empty;
        
        uint[] array = new uint[values.Count];
        for (int i = 0; i < values.Count; ++i)
        {
            array[i] = values[i];
        }
        return FromUIntArray(array, splitChar);
    }
#endregion // STRING

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

    public static Color Alpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    public static string GetHierarchyPath(Transform tf)
    {
        sb.Clear();
        sb.Append(tf.name);

        var parent = tf.parent;
        while (parent)
        {
            sb.Insert(0, "\\");
            sb.Insert(0, parent.name);
            parent = parent.parent;
        }

        return sb.ToString();
    }

    public static T GetComponentOnlyInParent<T>(Component component) where T : Component
    {
        var parent = component.transform.parent;
        while (parent != null)
        {
            var t = parent.GetComponent<T>();
            if (t != null)
            {
                return t;
            }
            parent = parent.parent;
        }
        return null;
    }

    public static void SetLayerRecursively (this UnityEngine.GameObject root, int layer)
    {
        root.gameObject.layer = layer;
        for (int i = 0; i < root.transform.childCount; ++i)
        {
            var ch = root.transform.GetChild(i);
            SetLayerRecursively(ch.gameObject, layer);
        }
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

    public static void DelaySeconds(this MonoBehaviour mb, float seconds, System.Action cb)
    {
        mb.StartCoroutine(_DelaySeconds(seconds, cb));
    }

    public static void DelayOneFrame(this MonoBehaviour mb, System.Action cb)
    {
        mb.StartCoroutine(_DelayOneFrame(cb));
    }

    private static IEnumerator _DelaySeconds(float seconds, System.Action cb)
    {
        yield return new WaitForSeconds(seconds);
        cb.Invoke();
    }

    public static IEnumerator _DelayOneFrame(System.Action cb)
    {
        yield return new WaitForEndOfFrame();
        cb.Invoke();
    }
}