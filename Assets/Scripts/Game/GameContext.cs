using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static Player plr => GameMgr.Inst.plr;
    public static ResSystem sRes => GameMgr.Inst.sRes;
    public static TimeSystem sTime => GameMgr.Inst.sTime;

    public ItemCount[] initItems;

    public void Init()
    {
        Reset();
    }

    public void Reset()
    {
        if (initItems != null)
        {
            plr.GetInventory().AddItems(initItems);
        }

    }
}
