using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

// [CreateAssetMenu(fileName = "Effect", menuName = "CONFIG/Effect")]
public class EffectSO : ScriptableObject
{
    public virtual Type GetInstType()
    {
        throw new System.NotImplementedException();
    }

#if UNITY_EDITOR
    [Button("RemoveFromAsset", EButtonEnableMode.Editor)]
    private void RemoveFromAsset()
    {
        ConfigUtils.RemoveFroMainAsset(this);
    }
#endif
}