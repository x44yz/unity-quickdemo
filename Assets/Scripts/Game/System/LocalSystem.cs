
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalSystem : MonoBehaviour
{
    public Lang defaultLang;

    [Header("RUNTIME")]
    public Lang curLang;
    public TMP_FontAsset font;

    private Dictionary<string, string> locs;

    public Func<bool> onLangChanged;

    public void Init()
    {
        locs = new Dictionary<string, string>();
        LoadLang(GameMgr.Inst.sSave.GetLang(defaultLang));
    }

    public void LoadLang(Lang lang)
    {
        Debug.Log($"[LOCAL]load lang > {lang}");
        curLang = lang;
        locs.Clear();

        string abbr = LangAbbr.GetAbbr(lang);
        var cfg = AssetMgr.Inst.LoadAsset<LocalCfg>($"locals/{abbr}", false);
        if (cfg == null)
            Debug.LogError($"[LOCAL]cant load local cfg {abbr}");
        else
        {
            foreach (var item in cfg.items)
            {
                locs[item.key] = item.val;
            }
        }

        font = AssetMgr.Inst.LoadAsset<TMP_FontAsset>($"fonts/{abbr}", false);
        if (font == null)
            Debug.LogError($"[LOCAL]cant load font {abbr}");
    }

    public string Get(string key)
    {
        string val;
        if (locs.TryGetValue(key, out val))
            return val;
        return $"NO-{key}";
    }

    public string Format(string key, object arg0)
    {
        return string.Format(Get(key), arg0);
    }

    public void ChangeLang(Lang lang)
    {
        if (lang == curLang)
        {
            Debug.LogWarning($"cant change lang because same > {lang}");
            return;
        }

        LoadLang(lang);
        if (onLangChanged != null)
            onLangChanged.Invoke();
    }
}