using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResSystem : MonoBehaviour
{
    public List<AudioCfg> audioCfgs { get; protected set; }
    public List<ItemCfg> itemCfgs { get; protected set; }

    public MiscCfg miscCfg;
    public GameObject ingameDebugPRB;
    public void Init()
    {
        // if (defaultSpr == null)
        //     Debug.LogError("default spr is null");

        itemCfgs = AssetMgr.Inst.LoadAssets<ItemCfg>("items", false);
        audioCfgs = AssetMgr.Inst.LoadAssets<AudioCfg>("audios", false);
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

    public AudioCfg GetAudioCfg(AudioId id)
    {
        foreach (var d in audioCfgs)
        {
            if (d.audioId == id)
                return d;
        }
        Debug.LogError($"cant get audio cfg > {id}");
        return null;
    }
}
