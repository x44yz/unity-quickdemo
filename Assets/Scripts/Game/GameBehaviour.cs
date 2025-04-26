using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    // public PanelHUD hud => GameMgr.Inst.hud;
    // public TimeSystem sTime => GameMgr.Inst.sTime;
    public static ResSystem sRes => GameMgr.Inst?.sRes;
    // public EvtSystem sEvent => GameMgr.Inst.sEvent;
    // public PanelStage panelStage => GameMgr.Inst.panelStage;
    public static GameContext gCtx => GameMgr.Inst?.gCtx;
    public static AudioSystem sAudio => GameMgr.Inst.sAudio;
    public static EntitySystem sEntity => GameMgr.Inst.sEntity;
    public static UISystem sUI => GameMgr.Inst.sUI;

    // public static Player plr => GameMgr.Inst.plr;
    public static GameMgr game => GameMgr.Inst;
    // public static PanelEvent panelEvent => GameMgr.Inst.panelEvent;
    // public static ConfigSystem sConfig => GameMgr.Inst?.sConfig;
    // public static Board board => GameMgr.Inst?.board;
    // public static Pathfinding path => GameMgr.Inst.path;

    public static PageHUD hud => GameMgr.Inst.sUI.GetPage<PageHUD>();

    public virtual Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    // public Vector3 fwd 
    // {
    //     get { return transform.forward; }
    //     set { transform.forward = value; }
    // }

    public Vector3 right 
    {
        get { return transform.right; }
        set { transform.right = value; }
    }
}