#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ImageRaycastGizmo : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        var img = GetComponent<Image>();
        if (img == null)
            return;

        var rt = GetComponent<RectTransform>();
        var cam = Camera.main;

        Handles.color = Color.red;
        Vector3 lb;
        // RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, rt.anchoredPosition, cam, out lb);
        lb = Camera.main.ScreenToWorldPoint(rt.position);
        Handles.DrawWireCube(lb, Vector3.one * 100);
    }
}
#endif