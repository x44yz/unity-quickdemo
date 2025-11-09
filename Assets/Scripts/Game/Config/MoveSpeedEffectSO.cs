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
public class MoveSpeedEffectSO : EffectSO
{
    public override Type GetInstType()
    {
        return typeof(MoveSpeedEffect);
    }

    public float duration;
    public ModifyValueType valueType;
    public float value;
}