using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Inst
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogWarning($"cant find a gameobject of instance {typeof(T)}");

                    _instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    if (_instance == null)
                    {
                        Debug.LogError($"failed create instace {typeof(T)}");
                    }
                }

                if (_instance != null)
                {
                    _instance.Init();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            _instance.Init();

            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance.gameObject != this.gameObject)
        {
            Debug.LogWarning($"destroy duplicated instance {typeof(T)}");
            Destroy(this.gameObject);
        }
    }

    protected virtual void Init()
    {
    }

    protected virtual void OnApplicationQuit()
    {
        _instance = (T)((object)null);
    }
}