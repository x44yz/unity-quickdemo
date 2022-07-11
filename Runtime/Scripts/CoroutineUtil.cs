using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtil : MonoBehaviour
{
    private static CoroutineUtil _inst = null;

    public static CoroutineUtil Inst
    {
        get
        {
            if (_inst == null)
            {
                var obj = new GameObject("CoroutineUtil");
                _inst = obj.AddComponent<CoroutineUtil>();
            }
            return _inst;
        }
    }

    private void Awake() 
    {
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void DelayFrame(int frameCount, Action callback)
    {
        StartCoroutine(_DelayFrame(frameCount, callback));
    }

    public IEnumerator _DelayFrame(int frameCount, Action callback)
    {
        int frameTick = 0;
        while (frameTick < frameCount)
        {
            frameTick += 1;
            yield return null;
        }

        callback.Invoke();
    }

    public void DelayTime(float seconds, Action callback)
    {
        StartCoroutine(_DelayTime(seconds, callback));
    }

    public IEnumerator _DelayTime(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        
        callback.Invoke();
    }
}
