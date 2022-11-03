using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// https://docs.unity.cn/cn/2019.4/ScriptReference/SettingsProvider.html
namespace QuickDemo
{
    [FilePath("ProjectSettings/CaptureSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class CaptureSettings : ScriptableSingleton<CaptureSettings>
    {
        [SerializeField]
        private string _storeKey = "";
        [SerializeField]
        private string _captureFilePrefix = "Capture";

        public string capturePath
        {
            get 
            {
                return System.IO.Path.GetFullPath(Application.dataPath + "/../Captures/");
            }
        }

        public string storeKey
        {
            get
            {
                if (string.IsNullOrEmpty(_storeKey))
                    _storeKey = Application.productName;
                return _storeKey;
            }
        }

        public string captureFilePrefix
        {
            get { return _captureFilePrefix; }
        }

        void OnDisable()
        {
            Save();
        }

        public void Save()
        {
            Save(true);
        }

        internal SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(this);
        }
    }

    class CaptureProjectSettingsProvider : SettingsProvider
    {
        SerializedObject m_SerializedObject;
        SerializedProperty m_StoreKey;
        SerializedProperty m_CaptureFilePrefix;

        private class Styles
        {
            public static readonly GUIContent StoreKeyLabel = EditorGUIUtility.TrTextContent("Store Key");
            public static readonly GUIContent CaptureFilePrefixLabel = EditorGUIUtility.TrTextContent("CaptureFile Prefix");
        }

        public CaptureProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords) {}

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            CaptureSettings.instance.Save();
            m_SerializedObject = CaptureSettings.instance.GetSerializedSettings();
            m_StoreKey = m_SerializedObject.FindProperty("_storeKey");
            m_CaptureFilePrefix = m_SerializedObject.FindProperty("_captureFilePrefix");
        }

        public override void OnGUI(string searchContext)
        {
            m_SerializedObject.Update();
            EditorGUI.BeginChangeCheck();

            // EditorGUILayout.BeginHorizontal();
            // {
            //     m_CapturePath.stringValue = EditorGUILayout.TextField(Styles.CapturePathLabel, m_CapturePath.stringValue);
            //     if (GUILayout.Button("Brower", GUILayout.Width(70)))
            //     {
            //         var fullpath = EditorUtility.OpenFolderPanel("Choose Capture Save Folder", CaptureSettings.instance.fullPath, "Captures");
            //         m_CapturePath.stringValue = System.IO.Path.GetRelativePath();
            //     }
            // }
            // EditorGUILayout.EndHorizontal();

            var captureSettings = CaptureSettings.instance;
            m_StoreKey.stringValue = EditorGUILayout.TextField(Styles.StoreKeyLabel, captureSettings.storeKey);
            m_CaptureFilePrefix.stringValue = EditorGUILayout.TextField(Styles.CaptureFilePrefixLabel, captureSettings.captureFilePrefix);

            if (EditorGUI.EndChangeCheck())
            {
                m_SerializedObject.ApplyModifiedProperties();
                CaptureSettings.instance.Save();
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateCaptureProjectSettingProvider()
        {
            var provider = new CaptureProjectSettingsProvider("Project/Capture", SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<Styles>());
            return provider;
        }
    }
}
