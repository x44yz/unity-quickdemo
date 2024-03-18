using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class UIUtils
{
    public static StringBuilder sb = new StringBuilder();

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
}
