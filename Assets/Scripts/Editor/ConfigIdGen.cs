using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

public class ConfigIdGen
{
    public static List<string> GetCfgIds<T>(string path) where T : ScriptableObject, IIdGenTarget
    {
        List<string> ids = new List<string>();

        string [] files = Directory.GetFiles(path);
        foreach(string fileName in files)
        {
            if (fileName.EndsWith(".asset") == false)
                continue;

            string assetPath = fileName.Replace(Application.dataPath, "Assets");
            Debug.Log($"file > {fileName} - {assetPath}");
            var cfg = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (cfg.IsExport() == false)
                continue;

            if (string.IsNullOrEmpty(cfg.Id()))
            {
                Debug.LogError($"file {fileName} > {cfg.Name()} id is null");
                continue;
            }
            if (ids.Contains(cfg.Id()))
            {
                Debug.LogError($"file {fileName} > {cfg.Name()} has dup id > {cfg.Id()}");
                continue;
            }
            ids.Add(cfg.Id());
        }
        return ids;
    }

    public static void OutputIds(string path, List<string> ids, 
        string clsName, bool all)
    {
        if (File.Exists(path))
            File.Delete(path);
        using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n");
            sb.Append($"public static class {clsName}");
            sb.Append("\n{\n");
            for (int i = 0; i < ids.Count; ++i)
            {
                string key = ids[i].Trim().ToUpper().Replace(" ", "_");
                sb.Append($"    public const string {key} = \"{ids[i]}\";\n");
            }

            if (all)
            {
                sb.Append("\n");
                sb.Append("    public static string[] all = new string[] {\n");
                for (int i = 0; i < ids.Count; ++i)
                {
                    string key = ids[i].Trim().ToUpper().Replace(" ", "_");
                    sb.Append($"        {key},\n");
                }
                sb.Append("    };\n");
            }

            sb.Append("}");

            Byte[] info = new UTF8Encoding(true).GetBytes(sb.ToString());
            fs.Write(info, 0, info.Length);
        }
    }

    [MenuItem("Tools/GenLocalKey", false, 1000)]
    public static void GenLangKeys()
    {
        Debug.Log("GenLangKeys");

        List<string> ids = GetCfgIds<LocalItemCfg>(Application.dataPath + "/Locals");
        OutputIds(Application.dataPath + "/Scripts/Game/Gen/LocalKeys.cs",
            ids, "LocalKey", false);
        AssetDatabase.Refresh();
    }
}
