using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "1010P_HasItem", menuName = "AI/P/PHasItem")]
public class PHasItem : Precondition
{
    public ItemType itemType;
    public ItemId itemId;

    public override bool IsTrue(IContext ctx)
    {
        var actx = ctx as ActorContext;
        var actor = actx.actor;

        if (itemType != ItemType.NONE)
        {
            return actor.bag.Has(itemType);
        }
        return actor.bag.Has(itemId);
    }
}