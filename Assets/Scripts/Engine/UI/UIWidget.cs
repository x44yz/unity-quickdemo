using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidget : MonoBehaviour
{
    public bool isShow => gameObject.activeSelf;

    private RectTransform _rectTF;
    protected RectTransform rectTF
    {
        get {
            if (_rectTF == null)
                _rectTF = GetComponent<RectTransform>();
            return _rectTF;
        }
    }

    public Vector2 screenPos
    {
        get {
            // canves 是 overlay
            return transform.position;
            // canves 是 camera
            // return uicamera.WorldToScreenPoint(transform.position);
        }
    }

    public Vector2 anchorPos
    {
        get { return rectTF.anchoredPosition; }
        set { rectTF.anchoredPosition = value; }
    }

    public float anchorPosX
    {
        get { return anchorPos.x; }
        set { anchorPos = new Vector2(value, anchorPosY); }
    }

    public float anchorPosY
    {
        get { return anchorPos.y; }
        set { anchorPos = new Vector2(anchorPosX, value); }
    }

    // private void Awake()
    // {
    //     rectTF = GetComponent<RectTransform>();

    //     OnAwake();
    // }

    // private void Update()
    // {
    //     float dt = Time.deltaTime;
    //     OnUpdate(dt);
    // }

    // protected virtual void OnAwake()
    // {
    // }

    // protected virtual void OnUpdate(float dt)
    // {
    // }

    public virtual void Init()
    {
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
