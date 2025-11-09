using System;
using UnityEngine;

// itme object  
[DisallowMultipleComponent]
public class Item : Entity
{
    public static int ITEM_UID_GEN = 1000;

    public ItemId id;
    public int count;

    public Item(ItemId id, int count)
    {
        uid = ITEM_UID_GEN++;
        this.id = id;
        this.count = count;
    }

    public ItemSO cfg => sRes.GetItemSO(id);
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