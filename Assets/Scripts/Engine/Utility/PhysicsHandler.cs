using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
#endif

public class PhysicsHandler : MonoBehaviour
{
    public bool log;
#if UNITY_EDITOR
    private static string[] collideTags = TagDef.All;
    [Dropdown("collideTags")]
#endif
    public string collideTag;

    public delegate void TriggerFunc(GameObject obj);
    public TriggerFunc onTriggerEnter;
    public TriggerFunc onTriggerExit;
    public TriggerFunc onTriggerStay;

    private void OnTriggerEnter(Collider other)
    {
        if (log)
            Debug.Log($"{name} on trigger enter > {other.name} - {other.tag}");
        if (string.IsNullOrEmpty(collideTag) == false &&
            other.CompareTag(collideTag) == false)
            return;
        onTriggerEnter?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (log)
            Debug.Log($"{name} on trigger exit > {other.name} - {other.tag}");
        if (string.IsNullOrEmpty(collideTag) == false &&
            other.CompareTag(collideTag) == false)
            return;
        onTriggerExit?.Invoke(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (log)
            Debug.Log($"{name} on trigger exit > {other.name} - {other.tag}");
        if (string.IsNullOrEmpty(collideTag) == false &&
            other.CompareTag(collideTag) == false)
            return;
        onTriggerStay?.Invoke(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (log)
            Debug.Log($"{name} on trigger enter 2d > {other.name} - {other.tag}");
        if (string.IsNullOrEmpty(collideTag) == false &&
            other.CompareTag(collideTag) == false)
            return;
        onTriggerEnter?.Invoke(other.gameObject);
    }
}
