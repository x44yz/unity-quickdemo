using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace QuickDemo
{
    [AddComponentMenu("QuickDemo/AssetMgr")]
    public class AssetMgr : MonoBehaviour
    {
        public static AssetMgr Inst;

        public bool showLog = true;

        private Dictionary<string, AsyncOperationHandle> operationDictionary = new Dictionary<string, AsyncOperationHandle>();

        private void Awake()
        {
            Inst = this;
        }

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

            StartCoroutine(_LoadAssetAsync<T>(key, (opHandle)=>{
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

        public static void InstGameObjectAsync(string key, Action<GameObject> callback)
        {
            AssetMgr.Inst.LoadAssetAsync<GameObject>(key, (x)=>{
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

        public static GameObject InstGameObject(string key)
        {
            var asset = AssetMgr.Inst.LoadAsset<GameObject>(key);
            if (asset != null)
            {
                return GameObject.Instantiate(asset);
            }
            Debug.Log("[ASSET]failed inst obj > " + key);
            return null;
        }
    }
}