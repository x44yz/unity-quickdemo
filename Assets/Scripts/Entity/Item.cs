using System;
using UnityEngine;

// [DisallowMultipleComponent]
[Serializable]
public class Item
{
    public static ResSystem sRes => GameMgr.Inst.sRes;
    
    public static int ITEM_UID_GEN = 1000;

    public int uid;
    public ItemId id;
    public int count;

    public Item(ItemId id, int count)
    {
        uid = ITEM_UID_GEN++;
        this.id = id;
        this.count = count;
    }

    public ItemCfg cfg => sRes.GetItemCfg(id);
    public ItemType itemType => cfg.itemType;
    public Sprite spr => cfg.spr;

    public bool IsItemType(ItemType t)
    {
        return itemType.HasFlag(t);
    }

    // [Header("RUNTIME")]
    // public ItemCfg itemCfg;
    // public ItemData itemData;

    // public static void Create(ItemData itemData, Action<Item> callback)
    // {
    //     var itemCfg = GameConfig.Default.GetItem(itemData.cid);
    //     AssetMgr.InstGameObjectAsync(itemCfg.asset, (x)=>{
    //         Item item = null;
    //         if (x != null)
    //         {
    //             item = x.GetOrAddComponent<Item>();
    //             item.itemCfg = itemCfg;
    //             item.itemData = itemData;
    //         }
    //         callback?.Invoke(item);
    //     });
    // }
}