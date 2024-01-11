using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum InputClickState
// {
//     DOWN,
//     UP,
// }

public enum InputClickKey
{
    LEFT = 0,
    RIGHT = 1,
    MIDDLE = 2,
}

public class InputHandler : MonoBehaviour
{
    public Action<InputClickKey, Vector3> onClickDown;
    public Action<InputClickKey, Vector3> onClickUp;
    public Action onMouseEnter;
    public Action onMouseExit;
    public bool trackLog;

    public void ClickDown(InputClickKey key, Vector3 wp)
    {
        if (trackLog)
            Debug.Log($"{name} get click down > {key}");
        onClickDown?.Invoke(key, wp);
    }

    public void ClickUp(InputClickKey key, Vector3 wp)
    {
        if (trackLog)
            Debug.Log($"{name} get click up > {key}");
        onClickUp?.Invoke(key, wp);
    }

    public void MouseEnter()
    {
        if (trackLog)
            Debug.Log($"{name} mouse enter");
        onMouseEnter?.Invoke();
    }

    public void MouseExit()
    {
        if (trackLog)
            Debug.Log($"{name} mouse exit");
        onMouseExit?.Invoke();
    }
}
