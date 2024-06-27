using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Inst;

    public ResSystem sRes;
    public InputSystem sInput;
    public TimeSystem sTime;
    public CameraSystem sCamera;
    public Player plr;
    public GameContext gCtx;

    [Header("UI")]
    public PageHUD hud;

    [Header("RUNTIME")]
    public bool paused;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        // sRes.Init();
        // sInput.Init();
        // sCamera.Init();
        // sTime.Init();
        // sEntity.Init();
        
        // gCtx.Init();

        hud.transform.parent.gameObject.SetActive(true);
        hud.Init();
        hud.Show();
    }

    public void SetPaused(bool v)
    {
        paused = v;
        hud.SetPaused(paused);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPaused(!paused);
        }
        if (paused)
            return;

        // sTime.Tick(Time.deltaTime);
        // float dt = sTime.deltaMins;
        // sInput.Tick(dt);
        // sCamera.Tick(dt);
        // sEntity.Tick(dt);
    }

    public void Restart()
    {
        sTime.Reset();
        gCtx.Reset();
        SetPaused(false);
    }
}
