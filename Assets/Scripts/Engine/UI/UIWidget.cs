using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidget : MonoBehaviour
{
    public static GameMgr game => GameMgr.Inst;
    public static ResSystem sRes => GameMgr.Inst.sRes;
    public static InputSystem sInput => GameMgr.Inst.sInput;
    public static Player plr => GameMgr.Inst.plr;
    public static TimeSystem sTime => GameMgr.Inst.sTime;
    public static GameContext gCtx => GameMgr.Inst.gCtx;

    public bool isShow => gameObject.activeSelf;

    private void Awake()
    {
        OnAwake();
    }

    private void Update()
    {
        if (game.paused)
            return;

        float dt = Time.deltaTime;
        OnUpdate(dt);
    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void OnUpdate(float dt)
    {

    }

    public virtual void Init()
    {

    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
