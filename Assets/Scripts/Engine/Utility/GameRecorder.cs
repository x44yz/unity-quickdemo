using System;
using System.IO;
using UnityEngine;
using UnityEditor.Recorder;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameRecorder : MonoBehaviour
{
#if UNITY_EDITOR
    private const string PREFKEY_AUTORECORD_TIME = "AutoRecordTime";

    public bool autoRecord = true;
    public int intervalSecs = 86400;

    [Header("RUNTIME")]
    public string storeKey;

    private void Awake() 
    {
        if (autoRecord == false)
            return;

        DontDestroyOnLoad(gameObject);

        storeKey = Application.productName + PREFKEY_AUTORECORD_TIME;
        int ts = EditorPrefs.GetInt(storeKey, 0);
        // Debug.Log($"[RECORD]last auto record timestamp > {ts}");
        var cd = intervalSecs - (((int)Utils.GetTimeStamp()) - ts);
        if (cd <= 0f)
        {
            Debug.Log($"[RECORD]start auto record > {cd}");
            EditorPrefs.SetInt(storeKey, ((int)Utils.GetTimeStamp()));

            var recorderWindow = (RecorderWindow)EditorWindow.GetWindow(typeof(RecorderWindow), false, "Recorder", false);
            if (!recorderWindow.IsRecording())
            {
                recorderWindow.StartRecording();
            }
            else
            {
                recorderWindow.StopRecording();
            }
        }
    }

    private void OnDestroy()
    {
        if (autoRecord == false)
            return;

        UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(RecorderWindow));
        EditorWindow editorWindow = ((array.Length != 0) ? ((EditorWindow)array[0]) : null);
        if (editorWindow)
        {
            editorWindow.Close();
        }
    }
#endif
}
