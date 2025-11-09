using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResSystem : MonoBehaviour
{
    public List<AudioSO> audioSOs { get; protected set; }
    public List<ItemSO> itemSOs { get; protected set; }
    public List<BuffSO> buffSOs { get; protected set; }

    public MiscSO miscSO;
    public GameObject ingameDebugPRB;
    public void Init()
    {
        // if (defaultSpr == null)
        //     Debug.LogError("default spr is null");

        itemSOs = AssetMgr.Inst.LoadAssets<ItemSO>("items", false);
        audioSOs = AssetMgr.Inst.LoadAssets<AudioSO>("audios", false);
    }

    public List<T> LoadAssets<T>(string key, bool cache = true) where T : UnityEngine.Object
    {
        return AssetMgr.Inst.LoadAssets<T>(key, cache);
    }

    public ItemSO GetItemSO(ItemId id)
    {
        foreach (var d in itemSOs)
        {
            if (d.id == id)
                return d;
        }
        Debug.LogError($"cant get item so > {id}");
        return null;
    }

    public AudioSO GetAudioSO(string id)
    {
        foreach (var d in audioSOs)
        {
            if (d.id == id)
                return d;
        }
        Debug.LogError($"cant get audio so > {id}");
        return null;
    }
    public BuffSO GetBuffSO(string id)
    {
        foreach (var d in buffSOs)
        {
            if (d.id == id)
                return d;
        }
        Debug.LogError($"cant get buff cfg > {id}");
        return null;
    }
}
