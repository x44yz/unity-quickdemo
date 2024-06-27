using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResSystem : MonoBehaviour
{
    [NonSerialized]public List<ItemCfg> itemCfgs;

    public void Init()
    {
        // if (defaultSpr == null)
        //     Debug.LogError("default spr is null");

        itemCfgs = AssetMgr.Inst.LoadAssets<ItemCfg>("items", false);
    }

    public List<T> LoadAssets<T>(string key, bool cache = true) where T : UnityEngine.Object
    {
        return AssetMgr.Inst.LoadAssets<T>(key, cache);
    }

    public ItemCfg GetItemCfg(ItemId id)
    {
        foreach (var d in itemCfgs)
        {
            if (d.id == id)
                return d;
        }
        Debug.LogError($"cant get item cfg > {id}");
        return null;
    }
}
