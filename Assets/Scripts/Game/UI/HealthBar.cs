using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
#endif

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer sprFront;

    public float sprFrontWidth;
    public float sprFrontOriginScaleX;

    private void Awake()
    {
        sprFrontWidth = sprFront.sprite.bounds.size.x;
        sprFrontOriginScaleX = sprFront.transform.localScale.x;
    }

    public void Set(int hp, int max)
    {
        if (max <= 0)
        {
            Debug.LogError($"cant set health becase max > {max}");
            return;
        }

        float p = hp * 1f / max;
        SetPercent(p);
    }

    public void SetPercent(float p)
    {
        p = Mathf.Clamp01(p);

        var lpos = sprFront.transform.localPosition;
        lpos.x = sprFront.sprite.pivot.x / sprFront.sprite.pixelsPerUnit  * sprFrontWidth * p - sprFrontWidth * 0.5f;
        sprFront.transform.localPosition = lpos;

        var s = sprFront.transform.localScale;
        s.x = sprFrontOriginScaleX * p;
        sprFront.transform.localScale = s;
    }

// #if UNITY_EDITOR
//     [Range(0f, 1f)]
//     public float testValue;
//     [Button(enabledMode: EButtonEnableMode.Editor)]
//     private void TestValue()
//     {
//         sprFrontWidth = sprFront.sprite.bounds.size.x;
//         sprFrontOriginScaleX = sprFront.transform.localScale.x;
//         Debug.Log($"pivot > {sprFront.sprite.pivot/sprFront.sprite.pixelsPerUnit}");
//         SetPercent(testValue);
//     }
// #endif
}
