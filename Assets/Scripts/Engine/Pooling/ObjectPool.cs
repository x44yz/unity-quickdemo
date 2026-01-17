using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int initAmount;
    public bool isLimitMax;
    [ShowIf("isLimitMax")]
    public int maxAmount;

    private bool isInited => pooledObjects != null;
    private List<GameObject> pooledObjects;

    public static ObjectPool Create(string name)
    {
        var obj = new GameObject();
        obj.transform.ResetTransformation();
        var objPool = obj.AddComponent<ObjectPool>();
        objPool.name = $"ObjectPool_{name}";
        return objPool;
    }

    private void Start()
    {
        if (!isInited)
            CreatePool();
    }

    public void Init(GameObject prefab, int initAmount, int? maxAmount = null)
    {
        this.prefab = prefab;
        this.initAmount = initAmount;
        this.isLimitMax = maxAmount != null;
        if (maxAmount != null)
            this.maxAmount = maxAmount.Value;

        CreatePool();
    }

    private void CreatePool()
    {
        if (isInited)
        {
            Debug.LogError($"object pool {name} had inited");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError($"object pool {name} invalid prefab");
            return;
        }

        if (isLimitMax && initAmount > maxAmount)
        {
            Debug.LogError($"object pool {name} invalid max amount");
            maxAmount = initAmount;
        }

        pooledObjects = new List<GameObject>();

        for(int i = 0; i < initAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if(!isLimitMax || pooledObjects.Count < maxAmount)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }
       
        Debug.LogWarning($"object pool {name} out of limit > {pooledObjects.Count}/{maxAmount}");
        return null;
    }
}