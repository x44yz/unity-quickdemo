using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    public static ResSystem sRes => GameMgr.Inst.sRes;

    public AudioSource uiASource;
    public AudioSource tmpASource;
    // public GameAudioCfg audioCfg;

    [Header("RUNTIME")]
    public float volume;

    private Queue<AudioSource> asourcePool;
    private Dictionary<string, float> audioCooldown;

    public void Init()
    {
        asourcePool = new Queue<AudioSource>();
        asourcePool.Enqueue(tmpASource);

        audioCooldown = new Dictionary<string, float>();

        SetVolume(GameMgr.Inst.sSave.GetVolume());
    }

    public AudioSource GetAudioSource()
    {
        AudioSource targetASource = null;
        if (asourcePool.Count > 0)
        {
            targetASource = asourcePool.Dequeue();
        }
        else
        {
            targetASource = Instantiate(tmpASource);
            targetASource.transform.SetParent(tmpASource.transform.parent);
            targetASource.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        targetASource.volume = volume;
        return targetASource;
    }

    public void RecycleAudioSource(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            Debug.LogError($"cant add invalid audio source");
            return;
        }

        asourcePool.Enqueue(audioSource);
    }

    public void PlayAuido(string audioId, Vector3? wpos = null)
    {
        if (string.IsNullOrEmpty(audioId))
            return;

        var cfg = sRes.GetAudioSO(audioId);
        // check cooldown
        if (audioCooldown.ContainsKey(audioId) == false)
            audioCooldown[audioId] = 0;
        float t = audioCooldown[audioId];
        if (Time.time < t)
            return;
        audioCooldown[audioId] = Time.time + cfg.clip.length * 0.5f; // 0.5f 是因为音效一般都只有一半起作用  
        
        PlayAuido(cfg.clip, cfg.volumeScale, wpos);
    }

    public void PlayAuido(AudioClip audioClip, 
        float volumeScale = 1f,
        Vector3? wpos = null)
    {
        if (audioClip == null)
            return;

        var asource = GetAudioSource();
        if (wpos != null)
            asource.transform.position = wpos.Value;
        asource.PlayOneShot(audioClip, volumeScale);
        CoroutineUtil.Inst.DelaySeconds(audioClip.length, ()=>{
            RecycleAudioSource(asource);
        });
    }

    public void PlayUIAudio(string audioId, float? overrideVolumeScale = null)
    {
        if (string.IsNullOrEmpty(audioId))
            return;

        var cfg = sRes.GetAudioSO(audioId);
        float volumeScale = cfg.volumeScale;
        if (overrideVolumeScale != null)
            volumeScale = overrideVolumeScale.Value;
        PlayUIAudio(cfg.clip, volumeScale);
    }

    public void PlayUIAudio(AudioClip audioClip, float volumeScale = 1f)
    {
        if (audioClip == null)
            return;
        uiASource.PlayOneShot(audioClip, volumeScale);
    }
    
    public void SetVolume(float v)
    {
        volume = Mathf.Clamp01(v);
        uiASource.volume = volume;
    }
}