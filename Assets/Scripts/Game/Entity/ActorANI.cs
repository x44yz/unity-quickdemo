using System;
using System.Collections.Generic;
using UnityEngine;

// public enum AnimId
// {
//     IDLE = 0,
//     WALK = 1,
//     RUN = 2,
//     ATTACK = 3,
//     HIT = 4,
//     SHOOT = 5,
//     BUILD = 6,
//     EAT = 7,
//     NOACTION = 8,
//     SLEEP = 9,
//     NOACTION2 = 10,
//     DEAD = 11,
//     RELOAD = 12,
//     AIM = 13,
//     GATHER = 14,
// }

[DisallowMultipleComponent]
public class ActorANI : ActorComp
{
    public static readonly int _animIDSpeed = Animator.StringToHash("Speed");
    public static readonly int _animIDGrounded = Animator.StringToHash("Grounded");
    public static readonly int _animIDJump = Animator.StringToHash("Jump");
    public static readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
    public static readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

    private Animator animator;
    // public List<ANICfg> aniCfgs;
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    public Vector3 audioOffset;

    public override void Init(Actor actor)
    {
        base.Init(actor);

        animator = GetComponentInChildren<Animator>();
        // if (string.IsNullOrEmpty(actor.aniAssetLabel))
        // {
        //     Debug.LogError($"{actor.name} ani asset is null");
        //     return;
        // }
        // aniCfgs = sRes.LoadAssets<ANICfg>(actor.aniAssetLabel);
    }

    public override void Tick(float dt)
    {
    }

    // public float PlayANI(AnimId id)
    // {
    //     // Debug.Log($"{name} play ani > {id}");
    //     var cfg = GetActorANICfg(id);
    //     if (cfg == null)
    //         return 0f;

    //     if (cfg.fixedTrans)
    //         ani.CrossFadeInFixedTime(cfg.stateHash, cfg.transDuration, (int)cfg.layerIndex);
    //     else
    //         ani.CrossFade(cfg.stateHash, cfg.transDuration, (int)cfg.layerIndex);
    //     return cfg.time;
    // }

    // public ANICfg GetActorANICfg(AnimId id)
    // {
    //     foreach (var d in aniCfgs)
    //     {
    //         if (d.id == id)
    //             return d;
    //     }
    //     Debug.LogError($"cant get actor ani cfg > {id}");
    //     return null;
    // }

    // public bool HasANI(AnimId id)
    // {
    //     foreach (var d in aniCfgs)
    //     {
    //         if (d.id == id)
    //             return true;
    //     }
    //     return false;
    // }

    // public void SetANISpeed(AnimId id, float v)
    // {
    //     var cfg = GetActorANICfg(id);
    //     if (string.IsNullOrEmpty(cfg.speedParam))
    //     {
    //         Debug.LogError($"cant find {id} cfg speed param");
    //         return;
    //     }
    //     ani.SetFloat(cfg.speedParam, v);
    // }

    public void SetBool(int id, bool v)
    {
        animator.SetBool(id, v);
    }

    public void SetFloat(int id, float v)
    {
        animator.SetFloat(id, v);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(audioOffset), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(audioOffset), FootstepAudioVolume);
        }
    }
}