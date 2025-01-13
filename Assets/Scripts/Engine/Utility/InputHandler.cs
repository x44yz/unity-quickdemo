using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour, IPointerClickHandler,
    IPointerDownHandler, IPointerUpHandler, IBeginDragHandler,
    IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> onClick;
    public Action<PointerEventData> onClickDown;
    public Action<PointerEventData> onClickUp;
    public Action<PointerEventData> onBeginDrag;
    public Action<PointerEventData> onDrag;
    public Action<PointerEventData> onEndDrag;
    public bool trackLog;

    // click 表示按下和释放都在同一目标内完成才算一次 click
    public void OnPointerClick(PointerEventData eventData)
    {
        if (trackLog)
            Debug.Log($"{Utils.GetHierarchyPath(transform)} on pointer click");
        onClick?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (trackLog)
            Debug.Log($"{Utils.GetHierarchyPath(transform)} on pointer down");
        onClickDown?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (trackLog)
            Debug.Log($"{Utils.GetHierarchyPath(transform)} on pointer up");
        onClickUp?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (trackLog)
            Debug.Log($"{Utils.GetHierarchyPath(transform)} on begin drag");
        onBeginDrag?.Invoke(eventData);
    }

    // 检测 drag 时候这个是必须有的，否则 begin 和 end 无法触发
    public void OnDrag(PointerEventData eventData)
    {
        if (trackLog)
            Debug.Log($"{Utils.GetHierarchyPath(transform)} on drag");
        onDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (trackLog)
            Debug.Log($"{Utils.GetHierarchyPath(transform)} on end drag");
        onEndDrag?.Invoke(eventData);
    }
}
