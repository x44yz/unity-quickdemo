using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJson;
using TMPro;
using UnityEngine;

public static class SaveKey
{
    public const string VERSION = "version";
    public const string VOLUME = "volume";
    public const string LANGUAGE = "language";
    public const string RESOLUTION = "resolution";
}

public class SaveSystem : MonoBehaviour
{
    public GameContext gCtx => GameMgr.Inst.gCtx;

    public float autoSaveSecs;
    
    [Header("RUNTIME")]
    public int version;
    public bool hasArchive = false;
    public bool isDirty;
    public float autoSaveTick;

    public void Init()
    {
        version = PlayerPrefs.GetInt(SaveKey.VERSION, 0);
        if (version == 0)
        {
            hasArchive = false;
            version = 1;
            PlayerPrefs.SetInt(SaveKey.VERSION, version);
        }
        else
        {
            hasArchive = true;
        }
    }

    public void Tick(float dt)
    {
        if (!isDirty)
            return;

        autoSaveTick += dt;
        if (autoSaveTick >= autoSaveSecs)
        {
            isDirty = false;
            autoSaveTick = 0f;
            PlayerPrefs.Save();
            Debug.Log($"[SAVE]auto save at {Utils.GetTimeStamp()}");
        }
    }

    public Lang GetLang(Lang defaultLang)
    {
        return (Lang)PlayerPrefs.GetInt(SaveKey.LANGUAGE, (int)defaultLang);
    }

    public void SetLang(Lang lang)
    {
        isDirty = true;
        PlayerPrefs.SetInt(SaveKey.LANGUAGE, (int)lang);
    }

    public float GetVolume(float defaultVal = 0.5f)
    {
        return PlayerPrefs.GetFloat(SaveKey.VOLUME, defaultVal);
    }

    public void SetVolume(float v)
    {
        isDirty = true;
        PlayerPrefs.SetFloat(SaveKey.VOLUME, v);
    }

    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public ResolutionType GetResolutionType()
    {
        return (ResolutionType)PlayerPrefs.GetInt(SaveKey.RESOLUTION);
    }

    public void SaveResolutionType(ResolutionType resolutionType)
    {
        isDirty = true;
        PlayerPrefs.SetInt(SaveKey.RESOLUTION, (int)resolutionType);
    }
}