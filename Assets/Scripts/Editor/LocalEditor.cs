using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

public static class LocalEditor
{
    public static List<LocalItem> GetLocalItems(Lang lang, string path)
    {
        List<LocalItem> items = new List<LocalItem>();
        HashSet<string> keys = new HashSet<string>();

        string [] files = Directory.GetFiles(path);
        foreach(string fileName in files)
        {
            if (fileName.EndsWith(".asset") == false)
                continue;

            string assetPath = fileName.Replace(Application.dataPath, "Assets");
            Debug.Log($"file > {fileName} - {assetPath}");
            var cfg = AssetDatabase.LoadAssetAtPath<LocalItemCfg>(assetPath);

            if (string.IsNullOrEmpty(cfg.key))
            {
                Debug.LogError($"file {fileName} > {cfg.key} id is null");
                continue;
            }
            if (keys.Contains(cfg.key))
            {
                Debug.LogError($"file {fileName} > {cfg.key} has dup id > {cfg.Id()}");
                continue;
            }

            string cfgVal = cfg.GetVal(lang);
            if (string.IsNullOrEmpty(cfgVal))
                Debug.LogError($"file {fileName} > {cfg.key} val is null");

            keys.Add(cfg.Id());
            items.Add(new LocalItem(){
                key = cfg.Id(),
                val = cfgVal
            });
        }
        
        return items;
    }

    public static void OutputLocal(Lang lang, List<LocalItem> localItems)
    {
        var cfg = ScriptableObject.CreateInstance<LocalCfg>();
        
        cfg.lang = lang;
        cfg.items = localItems.ToArray();

        string path = $"Assets/Configs/Locals/LOCAL_{lang}.asset";
        AssetDatabase.CreateAsset(cfg, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"success output local LOCAL_{lang}");
    }

    [MenuItem("Tools/BuildLocals", false, 1000)]
    public static void GenLangKeys()
    {
        Debug.Log("start build locals");

        string path = Application.dataPath + "/Locals";
        List<LocalItem> items = GetLocalItems(Lang.ZHCN, path);
        OutputLocal(Lang.ZHCN, items);

        items = GetLocalItems(Lang.EN, path);
        OutputLocal(Lang.EN, items);

        AssetDatabase.Refresh();
    }
}