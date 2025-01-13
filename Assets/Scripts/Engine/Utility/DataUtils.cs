using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static partial class Utils
{
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
}