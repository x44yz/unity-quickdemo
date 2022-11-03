using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// https://docs.unity.cn/cn/2019.4/ScriptReference/SettingsProvider.html
namespace QuickDemo
{
    public class CaptureSettings : ScriptableObject
    {
        public const string k_CaptureSettingsPath = "Assets/Editor/CaptureSettings.asset";

        public string capturePath;

        public static CaptureSettings GetOrCreateSettings()
        {
            string path = Application.dataPath + "/Editor";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var settings = AssetDatabase.LoadAssetAtPath<CaptureSettings>(k_CaptureSettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<CaptureSettings>();
                settings.capturePath = System.IO.Path.GetFullPath(Application.dataPath + "/../Captures/");
                AssetDatabase.CreateAsset(settings, k_CaptureSettingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    // Register a SettingsProvider using IMGUI for the drawing framework:
    public static class CaptureSettingsIMGUIRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateCaptureSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Project/CaptureSettings", SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "Capture",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = CaptureSettings.GetSerializedSettings();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PropertyField(settings.FindProperty("capturePath"), new GUIContent("Capture Path"));
                        if (GUILayout.Button("Brower", GUILayout.Width(70)))
                        {
                            settings.FindProperty("capturePath").stringValue = EditorUtility.OpenFolderPanel("Choose Capture Save Folder", settings.FindProperty("capturePath").stringValue, "Captures");
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    settings.ApplyModifiedProperties();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Capture" })
            };

            return provider;
        }
    }
}
