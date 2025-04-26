using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UISystem : MonoBehaviour
{
    public Canvas uiRoot;
    // TODO: 采用 prb 会加载  
    // public string hudAddr;
    // public string pageMainAddr;
    // public string pageUpgradeAddr;
    // public string dialogAddr;
    // public string pageDefeatAddr;

    // [Header("RUNTIME")]
    private Dictionary<Type, GUIWidget> openedPages;

    // public PageHUD hud => GetPage<PageHUD>();
    // public PageMain pageMain => GetPage<PageMain>();
    // public PageUpgrade pageUpgrade => GetPage<PageUpgrade>();
    // public PageDialog dialog => GetPage<PageDialog>();
    // public PageDefeat pageDefeat => GetPage<PageDefeat>();
    // public PageSkillSelect pageSkillSelect => GetPage<PageSkillSelect>();

    public void Init()
    {
        openedPages = new Dictionary<Type, GUIWidget>();
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
}