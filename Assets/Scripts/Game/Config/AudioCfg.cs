using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using NaughtyAttributes;
#endif

public enum AudioId
{
    NONE = 0,
    UI_BTN_HOVER = 1,
    UI_BTN_CLICK = 2,
    UI_BUY_SELL = 3,
    UI_CLOSE_WINDOW = 4,
}

// [Serializable]
// public class AudioRes
// {
//     public AudioId audioId;
//     public AudioClip clip;
//     public float volumeScale = 1f;
// }

[CreateAssetMenu(fileName = "AC_ID", menuName = "CONFIG/Audio")]
public class AudioCfg : ScriptableObject
{
    public AudioId audioId;
    public AudioClip clip;
    public float volumeScale = 1f;

    // public AudioRes[] audioRes;
    // // NOTE: 使用 fake 音频是防止出错，另外有些回调是
    // // 在声音播放完成之后，简化逻辑 
    // public AudioClip silentClip;

    // public AudioRes GetAudioRes(AudioId id)
    // {
    //     foreach (var k in audioRes)
    //     {
    //         if (k.audioId == id)
    //             return k;
    //     }
    //     Debug.LogError($"cant find audio id > {id}");
    //     return null;
    // }

    // public AudioClip GetAudioClip(AudioId id)
    // {
    //     foreach (var k in audioRes)
    //     {
    //         if (k.audioId == id)
    //             return k.clip;
    //     }
    //     Debug.LogError($"cant find audio id > {id}");
    //     return silentClip;
    // }
#if UNITY_EDITOR
    [Button("Auto Name", EButtonEnableMode.Editor)]
    private void AutoName()
    {
        EditorUtils.RenameAsset(this, $"AC_{audioId}");
    }
#endif
}