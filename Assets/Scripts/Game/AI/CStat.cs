using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "1010C_Stat", menuName = "AI/C/CStat")]
public class CStat : Consideration
{
    public Stat stat;
    public Stat statMax;
    public AnimationCurve curve;

    public override float Score(IContext ctx)
    {
        var actx = ctx as ActorContext;
        var actor = actx.actor;
        float cur = actor.GetStatVal(stat);
        float max = actor.GetStatVal(statMax);
        float t = Mathf.Clamp01(cur / max);
        return curve.Evaluate(t);
    }

#if UNITY_EDITOR
    [Button("RemoveFromAsset", EButtonEnableMode.Editor)]
    private void RemoveFromAsset()
    {
        AIUtils.RemoveFromAsset(this);
    }

    [Button("AutoName", EButtonEnableMode.Editor)]
    private void AutoName()
    {
        this.name = $"CStat_{stat}";
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}