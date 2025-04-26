using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

[Serializable]
public class LocalItem
{
    public string key;
    public string val;
}

[CreateAssetMenu(fileName = "LOCAL_XX", menuName = "CONFIG/Localization")]
public class LocalCfg : ScriptableObject
{
    public Lang lang;
    public LocalItem[] items;
}