using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public float scale = 1.2f;
    private Vector3 originScale = Vector3.one;

    private void Start()
    {
        originScale = gameObject.transform.localScale;
    }

    private void OnMouseEnter() 
    {
        gameObject.transform.localScale = originScale * scale;
    }

    private void OnMouseExit() 
    {
        gameObject.transform.localScale = originScale;
    }
}
