using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// game ui widget
public class GUIWidget : UIWidget
{
    public static GameMgr game => GameMgr.Inst;
    public static ResSystem sRes => GameMgr.Inst.sRes;
    public static UISystem sUI => GameMgr.Inst.sUI;
    public static GameContext gCtx => GameMgr.Inst.gCtx;
    public static EntitySystem sEntity => GameMgr.Inst.sEntity;
    public static LocalSystem sLocal => GameMgr.Inst.sLocal;
    public static SaveSystem sSave => GameMgr.Inst.sSave;
    public static AudioSystem sAudio => GameMgr.Inst.sAudio;
}
