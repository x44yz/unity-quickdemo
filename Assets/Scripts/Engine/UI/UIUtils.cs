using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class UIUtils
{
    public static StringBuilder sb = new StringBuilder();

    public static void HideListAllWidgets<T>(T tmp) where T : UIWidget
    {
        HandleListAllWidgets<T>(tmp, (wgt)=>{
            wgt.Hide();
        });
    }

    public static void HandleListAllWidgets<T>(T tmp, Action<T> callback) where T : UIWidget
    {
        var widgets = GetListAllWidgets(tmp);
        foreach (var wgt in widgets)
        {
            callback?.Invoke(wgt);
        }
    }

    public static List<T> GetListAllWidgets<T>(T tmp) where T : UIWidget
    {
        List<T> widgets = new List<T>();
        for (int i = 1; i < tmp.transform.parent.childCount; ++i)
        {
            var obj = tmp.transform.parent.GetChild(i).gameObject;
            var wgt = obj.GetComponent<T>();
            widgets.Add(wgt);
        }
        return widgets;
    }

    public static T GetListValidWidget<T>(int idx, T tmp) where T : UIWidget
    {
        GameObject obj = null;
        var wgtCount = tmp.transform.parent.childCount - 1;
        if (idx < wgtCount)
        {
            obj = tmp.transform.parent.GetChild(idx + 1).gameObject;
        }
        else
        {
            obj = GameObject.Instantiate(tmp).gameObject;
            obj.name = $"Widget_{tmp.name.Split('_')[1]}_{idx}";
            obj.transform.SetParent(tmp.transform.parent, false);
            obj.GetComponent<T>().Init();
        }
        obj.SetActive(true);
        return obj.GetComponent<T>();
    }

    public static T GetListValidWidget<T>(T tmp) where T : UIWidget
    {
        GameObject obj = null;
        var wgtCount = tmp.transform.parent.childCount - 1;
        for (int i = 0; i < wgtCount; ++i)
        {
            var child = tmp.transform.parent.GetChild(i + 1).gameObject;
            if (child.gameObject.activeSelf == false)
            {
                obj = child;
                break;
            }
        }

        if (obj == null)
        {
            obj = GameObject.Instantiate(tmp).gameObject;
            obj.name = $"Widget_{tmp.name.Split('_')[1]}_{wgtCount}";
            obj.transform.SetParent(tmp.transform.parent, false);
            obj.GetComponent<T>().Init();
        }
        obj.SetActive(true);
        return obj.GetComponent<T>();
    }

    public static string ToHex(this Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        int a = (int)(color.a * 255);
        string hex = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }

    public static string TextColor(string s, Color c)
    {
        return $"<color={c.ToHex()}>{s}</color>";
    }

    public static string FormatHMS(int seconds)
    {
        int h = (int)(seconds / 3600);
        seconds -= h * 3600;
        int m = (int)(seconds / 60);
        seconds -= m * 60;
        sb.Clear();
        if (h > 0)
            sb.Append($"{h}H");
        if (h > 0 || m > 0)
            sb.Append($"{m}M");
        sb.Append($"{seconds}S");
        return sb.ToString();
    }
}
