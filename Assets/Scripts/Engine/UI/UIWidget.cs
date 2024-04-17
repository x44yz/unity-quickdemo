using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidget : MonoBehaviour
{
    public bool isShow => gameObject.activeSelf;

    private void Awake()
    {
        OnAwake();
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        OnUpdate(dt);
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnUpdate(float dt)
    {
    }

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
