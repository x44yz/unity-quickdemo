using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "IC_ID", menuName = "CONFIG/Item")]
public class ItemSO : ScriptableObject
{
    public ItemId id;
    public string _name;
    [TextArea]
    public string comment;
    public ItemType itemType;
    public GameObject prefab;
    public Sprite spr;
    public float price;
}
