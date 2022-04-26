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

        public void LoadAsset<T>(string key, Action<T> callback, bool cache = true) where T : UnityEngine.Object
        {
            AsyncOperationHandle op;
            if (operationDictionary.TryGetValue(key, out op))
            {
                callback?.Invoke((T)op.Result);
                return;
            }

            StartCoroutine(_LoadAssetAsync<T>(key, (opHandle)=>{
                if (opHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError($"[ASSET]LoadAssetAsync failed > {key}");
                    callback?.Invoke(default(T));
                    return;
                }
                
                Debug.Log($"[ASSET]LoadAssetAsync successed > {key}");
                if (cache)
                {
                    operationDictionary[key] = opHandle;
                }
                else
                {
                    Addressables.Release(opHandle);
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

        public static void InstGameObjectAsync(string key, Action<GameObject> callback)
        {
            AssetMgr.Inst.LoadAsset<GameObject>(key, (x)=>{
                if (x != null)
                {
                    var obj = GameObject.Instantiate(x);
                    callback?.Invoke(obj);
                }
                else
                {
                    callback?.Invoke(null);
                }
            });
        }
    }
}