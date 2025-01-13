using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

public abstract class ActorDecision : Decision
{
#if UNITY_EDITOR
    [BoxGroup("Pre&Con")]
    public string preOrConAssetName;
    [BoxGroup("Pre&Con")]
    [Dropdown("preconditionTypes")]
    public string preconditionType;
    private static string[] preconditionTypes = AIUtils.preconditionTypes;

    [BoxGroup("Pre&Con")]
    [Dropdown("considerationTypes")]
    public string considerationType;
    private static string[] considerationTypes = AIUtils.considerationTypes;

    [Button("AddPrecondition", EButtonEnableMode.Editor)]
    private void AddPrecondition()
    {
        var pre = ScriptableObject.CreateInstance(preconditionType) as Precondition;
        if (pre == null)
        {
            Debug.LogError($"cant create precondition > {preconditionType}");
            return;
        }
    
        Array.Resize(ref preconditions, preconditions.Length + 1);
        preconditions[preconditions.Length - 1] = pre;
        
        if (string.IsNullOrEmpty(preOrConAssetName))
            pre.name = $"{preconditionType}";
        else
            pre.name = $"{preconditionType}_{preOrConAssetName}";
        AssetDatabase.AddObjectToAsset(pre, this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [Button("AddConsideration", EButtonEnableMode.Editor)]
    private void AddConsideration()
    {
        var con = ScriptableObject.CreateInstance(considerationType) as Consideration;
        if (con == null)
        {
            Debug.LogError($"cant create consideration > {considerationType}");
            return;
        }
    
        var cd = Activator.CreateInstance<ConsiderationDeco>();
        if (considerations == null)
            considerations = new ConsiderationDeco[1];
        else
            Array.Resize(ref considerations, considerations.Length + 1);
        considerations[considerations.Length - 1] = cd;
        
        cd.con = con;
        if (string.IsNullOrEmpty(preOrConAssetName))
            con.name = $"{considerationType}";
        else
            con.name = $"{considerationType}_{preOrConAssetName}";
        AssetDatabase.AddObjectToAsset(con, this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [Button("CleanUselessAssets", EButtonEnableMode.Editor)]
    private void CleanUselessAssets()
    {
        string mainAssetPath = AssetDatabase.GetAssetPath(this);
        UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(mainAssetPath);
        for (int i = assets.Length - 1; i >= 0; --i)
        {
            var a = assets[i];
            var instId = a.GetInstanceID();

            if (a.GetType().IsSubclassOf(typeof(Consideration)))
            {
                if (CheckConsiderationRefValid(instId) == false)
                {
                    AssetDatabase.RemoveObjectFromAsset(a);
                }
            }
            else if (a.GetType().IsSubclassOf(typeof(Precondition)))
            {
                if (CheckPreconditionRefValid(instId) == false)
                {
                    AssetDatabase.RemoveObjectFromAsset(a);
                }
            }
        }
        // 延时执行，否则会导入警告
        EditorApplication.delayCall += ()=> {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };
    }

    private bool CheckPreconditionRefValid(int instId)
    {
        foreach (var p in preconditions)
        {
            if (p != null && p.GetInstanceID() == instId)
                return true;
        }

        return false;
    }

    private bool CheckConsiderationRefValid(int instId)
    {
        foreach (var cd in considerations)
        {
            if (cd != null && cd.con != null && 
                cd.con.GetInstanceID() == instId)
                return true;
        }

        return false;
    }
#endif
}
