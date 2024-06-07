using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "1010C_ItemCount", menuName = "AI/C/CItemCount")]
public class CItemCount : Consideration
{
    public ItemType itemType;
    public ItemId itemId;
    public AnimationCurve curve;
    public int normalizeCount;

    public override float Score(IContext ctx)
    {
        var actx = ctx as ActorContext;
        var actor = actx.actor;

        int count = 0;
        if (itemType != ItemType.NONE)
            count = actor.bag.GetItemCount(itemType);
        else
            count = actor.bag.GetItemCount(itemId);
        float t = count * 1f / normalizeCount;
        return curve.Evaluate(t);
    }

#if UNITY_EDITOR
    [Button("RemoveFromAsset", EButtonEnableMode.Editor)]
    private void RemoveFromAsset()
    {
        AIUtils.RemoveFromAsset(this);
    }
#endif
}