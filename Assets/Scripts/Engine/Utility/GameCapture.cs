using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("Tools/GameCapture")]
public class GameCapture : MonoBehaviour
{
#if UNITY_EDITOR
    private const string PREFKEY_AUTOCAPTURE_LASTTIME = "AutoCaptureLastTime";

    public bool autoCapture = true;
    public float intervalHours = 24;
    public float playDelayMinutes = 1;
    public bool logCapture = false;

    [Header("RUNTIME")]
    public string atuoCaptureStoreKey;
    public float autoCaptureCD;

    public int autoCaptureInterval
    {
        get {
            return Mathf.RoundToInt(intervalHours * 3600f);
        }
    }

    public int autoCaptureDelay
    {
        get {
            return Mathf.RoundToInt(playDelayMinutes * 60f);
        }
    }

    public static void StartCapture(bool log)
    {
        var captureSettings = CaptureSettings.instance;
        if (Directory.Exists(captureSettings.capturePath) == false)
            Directory.CreateDirectory(captureSettings.capturePath);

        DateTime dt = DateTime.Now;
        string filename = $"{captureSettings.capturePath}/{captureSettings.captureFilePrefix}_{dt.Year}-{dt.Month}-{dt.Day}_{dt.Hour}-{dt.Minute}-{dt.Second}.png";
        ScreenCapture.CaptureScreenshot(filename);
        if (log)
        {
            Debug.Log($"[CAPTURE]success capture at file > {filename}");
        }
    }

    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);

        var captureSettings = CaptureSettings.instance;
        atuoCaptureStoreKey = captureSettings.storeKey + PREFKEY_AUTOCAPTURE_LASTTIME;
        int ts = EditorPrefs.GetInt(atuoCaptureStoreKey, 0);
        Debug.Log($"[CAPTURE]last auto capture timestamp > {ts}");
        autoCaptureCD = autoCaptureInterval - (((int)Utils.GetTimeStamp()) - ts);
        autoCaptureCD = Mathf.Max(autoCaptureCD, 0f) + autoCaptureDelay;
    }

    private void Update()
    {
        if (autoCapture == false)
            return;

        autoCaptureCD -= Time.deltaTime;
        if (autoCaptureCD <= 0f)
        {
            autoCaptureCD = autoCaptureInterval;
            StartCapture(logCapture);
            EditorPrefs.SetInt(atuoCaptureStoreKey, ((int)Utils.GetTimeStamp()));
        }
    }
#endif
}

#if UNITY_EDITOR
public class GameCaptureEditor
{
    // % - CTRL on Windows / CMD on OSX
    // # - Shift
    // & -Alt
    // LEFT/RIGHT/UP/DOWN - Arrow keys
    // F1 … F2 - F keys
    // HOME,END,PGUP,PGDN
    // letter: - _ + letter, like "_g"
    [MenuItem("Tools/GameCapture _#q", false)]
    public static void StartCapture()
    {
        GameCapture.StartCapture(true);
    }
}
#endif