using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GameState
{
    MAINMENU,
    BATTLE,
    UPGRADE,
    OVER,
    TOWER_REMOVE,
    DEFEAT,
}

public class GameMgr : MonoBehaviour
{
    public static GameMgr Inst;

    public ResSystem sRes;
    public AudioSystem sAudio;
    public GameContext gCtx;
    public EntitySystem sEntity;
    public LocalSystem sLocal;
    public UISystem sUI;
    public SaveSystem sSave;
    public TimeSystem sTime;
    public InputSystem sInput;
    public CameraSystem sCamera;

    public PageHUD hud;

    [Header("RUNTIME")]
    public bool paused;
    public GameState gameState;
    public GameState lastGameState;
    private GameObject ingameDebugger;
    private int ingameDebugClickCount;

    void Awake()
    {
        Inst = this;
        Application.targetFrameRate = 30;
    }

    void Start()
    {
        // sSave.Init();
        sRes.Init();
        gCtx.Init();
        sAudio.Init();
        sEntity.Init();
        // sLocal.Init();
        sUI.Init();

        // if (sSave.hasArchive == false)
        //     sSave.SaveResolutionType(sRes.miscCfg.initResolutionType);
        // var rt = sSave.GetResolutionType();
        // SetResolution(rt);

        SetGameState(GameState.MAINMENU);
    }

    public void SetResolution(ResolutionType resolutionType)
    {
        if (resolutionType == ResolutionType.FULLSCREEN)
            Screen.SetResolution(1920, 1080, true);
        else if (resolutionType == ResolutionType.R2560X1440)
            Screen.SetResolution(2560, 1440, false);
        else if (resolutionType == ResolutionType.R1920X1080)
            Screen.SetResolution(1920, 1080, false);
        else if (resolutionType == ResolutionType.R1366X768)
            Screen.SetResolution(1366, 768, false);
        else
            Debug.LogError($"not implement resolution > {resolutionType}");
        sSave.SaveResolutionType(resolutionType);
    }

    public void SetPaused(bool v)
    {
        paused = v;
        hud.SetPaused(paused);
    }

    public void SetGameState(GameState gs)
    {
        lastGameState = gameState;
        gameState = gs;
    }

    public void Update()
    {
        if (ingameDebugger == null && Input.GetKeyDown(KeyCode.Alpha7))
        {
            ingameDebugClickCount += 1;
            if (ingameDebugClickCount == 7)
            {
                ingameDebugger = Instantiate(sRes.ingameDebugPRB);
                ingameDebugger.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPaused(!paused);
        }
        if (paused)
            return;
#endif

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
