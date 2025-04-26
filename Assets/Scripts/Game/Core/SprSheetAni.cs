using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SprSheetAni : MonoBehaviour
{
    public Sprite[] sprites;
    public AnimationFlag flag;
    public float speed = 0.1f;
    public int startFrameIdx;

    private SpriteRenderer sprRender;
    private float frameTick;
    private int curFrameIdx;

    private void Start() 
    {
        sprRender = GetComponent<SpriteRenderer>();
        Play();
    }

    private void Update()
    {
        if (flag != AnimationFlag.REPEAT && curFrameIdx >= sprites.Length)
            return;

        frameTick += Time.deltaTime;
        if (frameTick >= speed)
        {
            frameTick -= speed;
            curFrameIdx = curFrameIdx + 1;
            if (curFrameIdx >= sprites.Length)
                curFrameIdx = 0;

            sprRender.sprite = sprites[curFrameIdx];
        }
    }

    public void Play()
    {
        curFrameIdx = startFrameIdx;
        frameTick = 0f;
        sprRender.sprite = sprites[0];
    }
}
