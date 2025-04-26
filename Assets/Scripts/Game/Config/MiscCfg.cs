using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Misc", menuName = "CONFIG/Misc")]
public class MiscCfg : ScriptableObject
{
    public ResolutionType initResolutionType;
}
