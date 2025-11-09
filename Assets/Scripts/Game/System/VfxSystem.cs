using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VfxPoolCfg
{
    public VfxObject prefab;
    public int poolCount;
    public int maxCount;

    public string name => prefab.name;
    public bool IsMaxLimit() => maxCount > 0;
}

public class VfxSystem : MonoBehaviour
{
    public VfxPoolCfg[] vfxPoolCfgs;

    private Dictionary<string, List<VfxObject>> pooledObjects;

    private void Start() 
    {
        CreatePool();
    }

    private void CreatePool()
    {
        if (pooledObjects != null)
        {
            Debug.LogError($"object pool had inited");
            return;
        }

        pooledObjects = new Dictionary<string, List<VfxObject>>();

        if (vfxPoolCfgs != null && vfxPoolCfgs.Length > 0)
        {
            for(int i = 0; i < vfxPoolCfgs.Length; i++)
            {
                var cfg = vfxPoolCfgs[i];
                if (cfg.prefab == null)
                {
                    Debug.LogError("vfx system exist null vfx prefab");
                    continue;
                }

                for (int m = 0; m < cfg.poolCount; ++m)
                {
                    VfxObject obj = Instantiate(cfg.prefab);
                    obj.gameObject.SetActive(false);
                    obj.transform.SetParent(transform);

                    var key = cfg.name;
                    if (pooledObjects.ContainsKey(key) == false)
                        pooledObjects[key] = new List<VfxObject>();
                    pooledObjects[key].Add(obj);
                }
            }
        }
    }

    private VfxPoolCfg GetVfxPoolCfg(string name)
    {
        if (vfxPoolCfgs == null || vfxPoolCfgs.Length == 0)
            return null;

        for(int i = 0; i < vfxPoolCfgs.Length; i++)
        {
            var cfg = vfxPoolCfgs[i];
            if (cfg.prefab == null)
            {
                Debug.LogError("vfx system exist null vfx prefab");
                continue;
            }

            if (cfg.name == name)
                return cfg;
        }
        Debug.LogError($"vfx system cant find cfg > {name}");
        return null;
    }

    public VfxObject GetVfx(VfxObject prefab)
    {
        if (prefab == null)
            return null;

        var name = prefab.name;
        List<VfxObject> pobjs = null;
        if (pooledObjects.TryGetValue(name, out pobjs) == false)
        {
            Debug.Log($"vfx system cant find any pooled obj > {name}");
            pobjs = new List<VfxObject>();
            pooledObjects[name] = pobjs;
        }

        for(int i = 0; i < pobjs.Count; i++)
        {
            if(pobjs[i].gameObject.activeInHierarchy == false)
            {
                var vobj = pobjs[i];
                vobj.Reset();
                return vobj;
            }
        }

        VfxObject obj = Instantiate(prefab);
        obj.gameObject.SetActive(false);
        pooledObjects[name].Add(obj);
        return obj;
    }

    public VfxObject GetVfx(string name)
    {
        List<VfxObject> pobjs = null;
        if (pooledObjects.TryGetValue(name, out pobjs) == false)
        {
            Debug.LogError($"vfx system cant find any pooled obj > {name}");
            return null;
        }

        for(int i = 0; i < pobjs.Count; i++)
        {
            if(pobjs[i].gameObject.activeInHierarchy == false)
            {
                var vobj = pobjs[i];
                vobj.Reset();
                return vobj;
            }
        }

        var cfg = GetVfxPoolCfg(name);
        if (cfg == null)
            return null;

        if(cfg.IsMaxLimit() && pobjs.Count >= cfg.maxCount)
        {
            Debug.LogWarning($"vfx {name} had out of {cfg.maxCount} limit");
            return null;
        }

        string key = cfg.name;
        VfxObject obj = Instantiate(cfg.prefab);
        obj.gameObject.SetActive(false);
        pooledObjects[key].Add(obj);
       
        return obj;
    }

    public void RecycleVfx(VfxObject obj)
    {
        obj.transform.SetParent(transform);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        obj.gameObject.SetActive(false);
    }
}