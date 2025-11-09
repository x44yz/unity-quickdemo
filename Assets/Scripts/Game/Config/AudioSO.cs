using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using NaughtyAttributes;
#endif

[CreateAssetMenu(fileName = "Audio", menuName = "CONFIG/Audio")]
public class AudioSO : ScriptableObject, IIdGenTarget
{
    [OnValueChanged("OnIdChanged")]
    public string id;
    public AudioClip clip;
    public float volumeScale = 1f;

#region IIdGenTarget
    public string Id() => id;
    public string Name() => id.ToTitleCase().Replace("_", "");
    public bool IsExport() => true;
#endregion // IIdGenTarget

#if UNITY_EDITOR
    private void OnIdChanged()
    {
        id = id.Trim().Replace(' ', '_');
        id = id.ToLower();
    }

    [Button("Auto Name", EButtonEnableMode.Editor)]
    private void AutoName()
    {
        ConfigUtils.RenameAssetName(this, id);
    }
#endif
}