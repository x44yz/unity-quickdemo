using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : MonoBehaviour
{
    public string dbgName = "[INVENTORY]";
    public bool trackLog;

    public List<Item> items = new List<Item>();
    public System.Func<ItemId, int, bool> onChanged;

    public void AddItems(List<Item> its)
    {
        if (its == null)
        {
            Debug.LogError($"{dbgName} its is null");
            return;
        }
        foreach (var it in its)
        {
            AddItem(it.id, it.count);
        }
    }

    public void AddItems(Dictionary<ItemId, int> its)
    {
        if (its == null)
        {
            Debug.LogError($"{dbgName} its is null");
            return;
        }
        foreach (var kv in its)
        {
            if (kv.Value == 0)
                continue;
            AddItem(kv.Key, kv.Value);
        }
    }

    public void AddItems(List<ItemCount> its)
    {
        if (its == null || its.Count == 0)
        {
            Debug.LogError($"{dbgName} its is null");
            return;
        }
        foreach (var it in its)
        {
            AddItem(it.id, it.count);
        }
    }

    public void AddItems(ItemCount[] its)
    {
        if (its == null)
        {
            Debug.LogError($"{dbgName} its is null");
            return;
        }
        foreach (var it in its)
        {
            AddItem(it.id, it.count);
        }
    }

    public void AddItem(Item it)
    {
        if (it == null)
        {
            Debug.LogError("cant add item because is null");
            return;
        }

        AddItem(it.id, it.count);
    }

    public void AddItem(ItemId id, int count)
    {
        var it = GetOrAddItem(id);
        it.count += count;
        if (trackLog)
            Debug.Log($"{dbgName} add item {id} - {count}");
        
        onChanged?.Invoke(id, count);
    }

    public bool RemoveItems(List<ItemId> items)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (GetItemCount(items[i]) == 0)
                continue;
            RemoveItem(items[i], 1);
        }
        return true;
    }

    public bool RemoveItems(ItemCount[] its)
    {
        if (its == null || its.Length == 0)
        {
            Debug.LogError($"{dbgName} its is null");
            return false;
        }
        foreach (var it in its)
        {
            RemoveItem(it.id, it.count);
        }
        return true;
    }

    public bool RemoveItems(List<ItemCount> its)
    {
        return RemoveItems(its.ToArray());
    }

    public void RemoveItem(Item it, int count = 1)
    {
        if (it == null)
        {
            Debug.LogError($"{dbgName} cant reduce null item");
            return;
        }
        if (it.count < count)
        {
            Debug.LogError($"{dbgName} cant reduce item {it.id} - {count}");
            return;
        }

        it.count -= count;
        onChanged?.Invoke(it.id, -count);

        if (it.count <= 0)
            items.Remove(it);

        if (trackLog)
            Debug.Log($"{dbgName} reduce item {it.id} - {count}");
    }

    public void RemoveItem(ItemId id, int count)
    {
        var it = GetItem(id);
        if (it == null || it.count < count)
        {
            Debug.LogError($"{dbgName} cant reduce item {id} - {count}");
            return;
        }
        RemoveItem(it, count);
    }

    public int GetItemCount(ItemType tp)
    {
        int count = 0;
        foreach (var it in items)
        {
            if (tp.HasFlag(it.itemType))
                count += it.count;
        }
        return count;
    }

    public List<Item> GetItems(ItemType tp)
    {
        var rt = new List<Item>();
        foreach (var it in items)
        {
            if (tp.HasFlag(it.itemType))
                rt.Add(it);
        }
        return rt;
    }

    public Item GetItem(ItemId id, bool log = false)
    {
        var it = items.Find(x => x.id == id);
        if (log && it == null)
            Debug.LogError($"cant find item id > {id}");
        return it;
    }

    public int GetItemCount(ItemId id)
    {
        var it = GetItem(id);
        return it != null ? it.count : 0;
    }

    public Item GetOrAddItem(ItemId id)
    {
        var it = GetItem(id);
        if (it == null)
        {
            it = new Item(id, 0);
            items.Add(it);
        }
        return it;
    }

    public bool HasEnough(ItemCount[] items)
    {
        if (items == null || items.Length == 0)
            return false;

        for (int i = 0; i < items.Length; ++i)
        {
            var it = items[i];
            if (GetItemCount(it.id) < it.count)
                return false;
        }
        return true;
    }

    public bool HasEnough(List<ItemCount> items)
    {
        return HasEnough(items.ToArray());
    }

    public bool Has(ItemType tp)
    {
        if (tp == ItemType.NONE)
        {
            Debug.LogError($"{name} item type is > {tp}");
            return false;
        }
        return GetItems(tp).Count > 0;
    }

    public bool Has(ItemId id)
    {
        if (id == ItemId.NONE)
            return false;
        return GetItemCount(id) > 0;
    }

    public void ClearAll()
    {
        items.Clear();
    }

    public List<Item> GetItems(ItemType[] itemTypes)
    {
        List<Item> rt = new List<Item>();
        for (int i = 0; i < itemTypes.Length; ++i)
        {
            var tp = itemTypes[i];
            rt.AddRange(GetItems(tp));
        }
        return rt;
    }
}
