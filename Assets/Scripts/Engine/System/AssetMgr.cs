using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetMgr : MonoSingleton<AssetMgr>
{
    public bool showLog = true;

    private Dictionary<string, AsyncOperationHandle> operationDictionary = new Dictionary<string, AsyncOperationHandle>();

    private void OnDestroy()
    {
        foreach (var item in operationDictionary)
        {
            Addressables.Release(item.Value);
        }
    }

    public void LoadAssetAsync<T>(string key, Action<T> callback, bool cache = true) where T : UnityEngine.Object
    {
        AsyncOperationHandle op;
        if (operationDictionary.TryGetValue(key, out op))
        {
            if (showLog)
            {
                Debug.Log($"[ASSET]LoadAssetAsync from cache successed > {key}");
            }
            callback?.Invoke((T)op.Result);
            return;
        }

        StartCoroutine(_LoadAssetAsync<T>(key, (opHandle) =>
        {
            if (opHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"[ASSET]LoadAssetAsync failed > {key}");
                Addressables.Release(opHandle);
                callback?.Invoke(default(T));
                return;
            }

            if (showLog)
            {
                Debug.Log($"[ASSET]LoadAssetAsync successed > {key}");
            }

            if (cache)
            {
                operationDictionary[key] = opHandle;
            }
            callback?.Invoke(opHandle.Result);
        }));
    }

    private IEnumerator _LoadAssetAsync<T>(string key, Action<AsyncOperationHandle<T>> callback)
    {
        var opHandle = Addressables.LoadAssetAsync<T>(key);
        yield return opHandle;
        callback?.Invoke(opHandle);
    }

    public T LoadAsset<T>(string key, bool cache = true) where T : UnityEngine.Object
    {
        AsyncOperationHandle opHandle;
        if (operationDictionary.TryGetValue(key, out opHandle))
        {
            if (showLog)
            {
                Debug.Log($"[ASSET]LoadAsset from cache successed > {key}");
            }
            return (T)opHandle.Result;
        }

        opHandle = Addressables.LoadAssetAsync<T>(key);
        opHandle.WaitForCompletion();

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            if (showLog)
            {
                Debug.Log($"[ASSET]LoadAsset successed > {key}");
            }

            if (cache)
            {
                operationDictionary[key] = opHandle;
            }
            return (T)opHandle.Result;
        }
        else
        {
            Addressables.Release(opHandle);
            return default(T);
        }
    }

    public List<T> LoadAssets<T>(string key, bool cache = true) where T : UnityEngine.Object
    {
        AsyncOperationHandle opHandle;
        if (operationDictionary.TryGetValue(key, out opHandle))
        {
            if (showLog)
            {
                Debug.Log($"[ASSET]LoadAsset from cache successed > {key}");
            }
            return Utils.ObjToList<T>(opHandle.Result);
        }

        opHandle = Addressables.LoadAssets<T>(key, null);
        opHandle.WaitForCompletion();

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            if (showLog)
            {
                Debug.Log($"[ASSET]LoadAsset successed > {key}");
            }

            if (cache)
            {
                operationDictionary[key] = opHandle;
            }
            return Utils.ObjToList<T>(opHandle.Result);
        }
        else
        {
            Addressables.Release(opHandle);
            return new List<T>() { default(T) };
        }
    }

    public void InstGameObjectAsync(string key, Action<GameObject> callback)
    {
        LoadAssetAsync<GameObject>(key, (x) =>
        {
            if (x != null)
            {
                var obj = GameObject.Instantiate(x);
                callback?.Invoke(obj);
            }
            else
            {
                Debug.Log("[ASSET]failed inst obj > " + key);
                callback?.Invoke(null);
            }
        });
    }

    public GameObject InstGameObject(string key)
    {
        var asset = LoadAsset<GameObject>(key);
        if (asset != null)
        {
            return GameObject.Instantiate(asset);
        }
        Debug.Log("[ASSET]failed inst obj > " + key);
        return null;
    }

    public Sprite LoadSprite(string key)
    {
        if (key.Contains("["))
        {
            var keys = key.Split('[');
            if (keys.Length != 2)
            {
                Debug.LogError($"[ASSET]not support format > {key}");
                return null;
            }

            var kk = LoadAssets<Sprite>(keys[0]);
            if (kk == null)
            {
                Debug.LogError($"[ASSET]failed load assets > {keys[0]}");
                return null;
            }

            var matchName = keys[1].Split(']')[0];
            foreach (var k in kk)
            {
                if (k.name == matchName)
                    return k;
            }
            Debug.LogError($"[ASSET]failed load assets > {key}");
            return null;
        }
        // default
        return LoadAsset<Sprite>(key);
    }
}