using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

// [CreateAssetMenu(fileName = "IC_ID", menuName = "CONFIG/Item")]
public abstract class Effect : ScriptableObject
{
    public EffectId effId;
    public abstract string Desc();
    public abstract System.Type InstType();

#if UNITY_EDITOR
    [Button("RemoveFromAsset", EButtonEnableMode.Editor)]
    private void RemoveFromAsset()
    {
        string mainAssetPath = AssetDatabase.GetAssetPath(this);
        var mainAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(mainAssetPath);
        if (mainAsset.GetInstanceID() == this.GetInstanceID())
        {
            Debug.LogWarning($"this main asset, cant remove");
            return;
        }

        AssetDatabase.RemoveObjectFromAsset(this);
        // 延时执行，否则会导入警告
        EditorApplication.delayCall += ()=> {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        };
    }
#endif
}
