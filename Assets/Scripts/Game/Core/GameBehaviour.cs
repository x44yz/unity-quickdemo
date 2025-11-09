using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    public static GameMgr game
    {
        get {
            if (Application.isPlaying)
                return GameMgr.Inst;
            return GameObject.FindObjectOfType<GameMgr>();
        }
    }
    public static ResSystem sRes => game.sRes;
    public static GameContext gCtx => game.gCtx;
    public static AudioSystem sAudio => game.sAudio;
    public static EntitySystem sEntity => game.sEntity;
    public static UISystem sUI => game.sUI;
    public static VfxSystem sVfx => game.sVfx;

    // public static PageHUD hud => GameMgr.Inst.sUI.GetPage<PageHUD>();

    public bool dbgTransform;

    public virtual Vector3 pos
    {
        get { return transform.position; }
        set { 
            transform.position = value;
            if (dbgTransform)
                Debug.Log($"{dbgName} set pos > {value}");
        }
    }

    public Vector3 fwd 
    {
        get { return transform.forward; }
        set { 
            transform.forward = value; 
            if (dbgTransform)
                Debug.Log($"{dbgName} set fwd > {value}");
        }
    }

    public Vector3 right 
    {
        get { return transform.right; }
        set { 
            transform.right = value; 
            if (dbgTransform)
                Debug.Log($"{dbgName} set right > {value}");
        }
    }

    public Vector3 up
    {
        get { return transform.up; }
        set { 
            transform.up = value; 
            if (dbgTransform)
                Debug.Log($"{dbgName} set up > {value}");
        }
    }

    public string hihyPath => Utils.GetHierarchyPath(transform);
    public string dbgName => name;

    public bool isVisible => gameObject.activeSelf;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}