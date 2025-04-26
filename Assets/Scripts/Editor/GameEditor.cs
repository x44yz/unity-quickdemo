using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GameEditor
{
    // [MenuItem("Tools/SnapToNearestGrid")]
    // public static void SnapToNearestGrid()
    // {
    //     var obj = Selection.activeGameObject;
    //     if (obj == null)
    //     {
    //         Debug.LogWarning("select obj is null");
    //         return;
    //     }
        
    //     var board = GameObject.FindObjectOfType<Board>();
    //     if (board == null)
    //     {
    //         Debug.LogWarning("cant find any board");
    //         return;
    //     }

    //     obj.transform.position = board.GetGridPos(obj.transform.position);
    // }
}
