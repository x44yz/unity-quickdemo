using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace QuickDemo.Editor
{
    [CustomEditor(typeof(UnityEngine.Transform))]
    [CanEditMultipleObjects]
    public class TransformExEditor : UnityEditor.Editor
    {
        //Unity's built-in editor
        UnityEditor.Editor defaultEditor;
        Transform transform;

        void OnEnable()
        {
            //When this inspector is created, also create the built-in inspector
            defaultEditor = UnityEditor.Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
            transform = target as Transform;
        }

        void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable
            MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null)
                disableMethod.Invoke(defaultEditor,null);
            DestroyImmediate(defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            defaultEditor.OnInspectorGUI();

            //Show World Space Transform
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("World Space", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("Position", transform.position);
            EditorGUILayout.Vector3Field("Rotation", transform.rotation.eulerAngles);
            EditorGUILayout.Vector3Field("Scale", transform.lossyScale);
            EditorGUI.EndDisabledGroup();
        }
    }
}