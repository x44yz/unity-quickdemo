using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UISystem : MonoBehaviour
{
    public Canvas uiRoot;

    private Dictionary<Type, GUIWidget> openedPages;

    public PageHUD hud => GetPage<PageHUD>();

    public void Init()
    {
        openedPages = new Dictionary<Type, GUIWidget>();
        var pages = GameObject.FindObjectsOfType<GUIPage>(true);
        if (pages != null && pages.Length > 0)
        {
            foreach (var p in pages)
            {
                p.Init();
                openedPages[p.GetType()] = p;
            }
        }
    }

    public T GetPage<T>() where T : GUIWidget
    {
        var tp = typeof(T);
        if (openedPages.ContainsKey(tp))
        {
            return openedPages[tp] as T;
        }
        return null;
    }

    public T OpenPage<T>() where T : GUIWidget
    {
        var tp = typeof(T);
        if (openedPages.ContainsKey(tp))
        {
            var p = openedPages[tp] as T;
            p.Show();
            return p;
        }

        Debug.Log($"[UI]open page > {tp}");

        string addr = $"ui/{tp.ToString().ToLower()}";
        var prefab = AssetMgr.Inst.LoadAsset<GameObject>(addr, false);
        if (prefab == null)
        {
            Debug.LogError($"cant load page > {addr}");
            return null;
        }

        var obj = Instantiate(prefab);
        obj.transform.SetParent(uiRoot.transform, false);
        var page = obj.GetComponent<T>();
        page.Show();
        page.Init();
        openedPages[tp] = page;
        return page;
    }

    public void ClosePage<T>() where T : GUIWidget
    {
        var page = GetPage<T>();
        if (page != null)
            page.Hide();
    }

    public void CloseAllPages()
    {
        foreach (var kv in openedPages)
            kv.Value.Hide();
    }
}