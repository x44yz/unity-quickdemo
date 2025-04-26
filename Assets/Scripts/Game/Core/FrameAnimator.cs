using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
#if UNITY_EDITOR
using UnityEditor;
using NaughtyAttributes;
#endif

[Serializable]
public class FrameAniInfo
{
    public AnimationId id;
    public AnimationFlag flag;
    public int frameIdx;
    public int length;

    public int endFrameIdx => frameIdx + length;
}

// [RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class FrameAnimator : MonoBehaviour
{
    public SpriteRenderer sprRender;
    public FrameAniInfo[] aniInfos;
    public AnimationId defaultAni;
    public Texture tex;
    public float speed;

    [Header("RUNTIME")]
    public bool playing;
    public Vector2 frameSize;
    public Sprite[] sprites;
    private FrameAniInfo curAniInfo;
    private float frameTick;
    private int curFrameIdx;

    // public bool IsLastFrame()
    // {
    //     if (curAniInfo == null)
    //         return true;
    //     return curFrameIdx >= sprites.Length - 1;
    // }

    // private void Start() 
    // {
    //     Play(defaultAni);
    // }

    private void Update()
    {
        if (playing == false)
            return;

        if (curAniInfo != null && 
            curAniInfo.length > 1 &&
            CheckFlagEnd() == false)
        {
            frameTick += Time.deltaTime;
            if (frameTick >= speed)
            {
                frameTick -= speed;
                curFrameIdx = curFrameIdx + 1;
                if (curAniInfo.flag == AnimationFlag.REPEAT)
                {
                    if (curFrameIdx >= curAniInfo.endFrameIdx)
                        curFrameIdx = curAniInfo.frameIdx;
                    sprRender.sprite = GetSprite(curFrameIdx);
                }
                else if (curAniInfo.flag == AnimationFlag.NONE)
                {
                    if (curFrameIdx < curAniInfo.endFrameIdx)
                        sprRender.sprite = GetSprite(curFrameIdx);
                }
            }
        }
    }

    private Sprite GetSprite(int frameIdx)
    {
        if (frameIdx < 0 || frameIdx >= sprites.Length)
        {
            Debug.LogError($"{frameIdx} out of sprites range {sprites.Length}");
            frameIdx = 0;
        }
        return sprites[frameIdx];
    }

    private bool CheckFlagEnd()
    {
        if (curFrameIdx < curAniInfo.endFrameIdx)
            return false;

        if (curAniInfo.flag == AnimationFlag.NONE)
            return true;
        
        return false;
    }

    public bool HasAnimation(AnimationId id)
    {
        if (aniInfos != null && aniInfos.Length > 0)
        {
            for (int i = 0; i < aniInfos.Length; ++i)
            {
                if (aniInfos[i].id == id)
                    return true;
            }
        }
        return false;
    }

    public void Play(AnimationId id)
    {
        var aniInfo = GetFrameAniInfo(id);
        if (aniInfo == null)
        {
            Debug.LogError($"{name} cant play ani > {id}");
            return;
        }

        playing = true;
        curFrameIdx = aniInfo.frameIdx;
        frameTick = 0f;
        curAniInfo = aniInfo;
        sprRender.sprite = GetSprite(curFrameIdx);
    }

    private FrameAniInfo GetFrameAniInfo(AnimationId id)
    {
        if (aniInfos != null && aniInfos.Length > 0)
        {
            for (int i = 0; i < aniInfos.Length; ++i)
            {
                if (aniInfos[i].id == id)
                    return aniInfos[i];
            }
        }
        Debug.LogError($"{name} cant find tex ani info > {id}");
        return null;
    }

    public void StopPlay()
    {
        playing = false;
    }

    public void ResumePlay()
    {
        playing = true;
    }

#if UNITY_EDITOR
    [Button("Auto Sprites", EButtonEnableMode.Editor)]
    private void AutoSpritesFromTex()
    {
        if (tex == null)
        {
            Debug.LogError("tex is null");
            return;
        }

        var texturePath = AssetDatabase.GetAssetPath(tex);
        sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();
        frameSize = sprites[0].textureRect.size;
    }

    public AnimationId testAniId;
    [Button("Test Play", EButtonEnableMode.Editor)]
    private void TestPlay()
    {
        Play(testAniId);
    }
#endif
}
