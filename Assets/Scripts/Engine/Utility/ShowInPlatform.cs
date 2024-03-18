using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum PlatformType
{
    WIN = 1 << 0,
    ANDROID = 1 << 1,
} 

public class ShowInPlatform : MonoBehaviour
{
    public PlatformType type; 

    private void Awake()   
    {
#if UNITY_STANDALONE_WIN
        gameObject.SetActive(type.HasFlag(PlatformType.WIN));
#endif
#if UNITY_ANDROID
        gameObject.SetActive(type.HasFlag(PlatformType.ANDROID));
#endif
    }
}
