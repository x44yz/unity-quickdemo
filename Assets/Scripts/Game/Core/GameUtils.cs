using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Globalization;

public static class GameUtils
{
    public static List<T> Filter<T>(List<T> ets, List<int> excludes) where T : Entity
    {
        if (excludes != null && excludes.Count > 0)
        {
            for (int i = ets.Count - 1; i >= 0; --i)
            {
                if (excludes.Contains(ets[i].uid))
                    ets.RemoveAt(i);
            }
        }
        return ets;
    }

    public static T Nearest<T>(Vector2 pos, List<T> entities) where T : Entity
    {
        float minDist = float.MaxValue;
        T target = null;
        for (int i = 0; i < entities.Count; ++i)
        {
            var et = entities[i];
            var dist = Vector2.Distance(pos, et.pos);
            if (dist < minDist)
            {
                minDist = dist;
                target = et;
            }
        }
        return target;
    }
    
    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        return textInfo.ToTitleCase(input.ToLower());
    }
}
