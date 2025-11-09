using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

#if UNITY_EDITOR
using NaughtyAttributes;
#endif

public class VfxObject : GameBehaviour
{
    private static int NAME_ONSTOP = Shader.PropertyToID("OnStop");

    public enum RecycleType
    {
        Particle = 0, // 粒子生命周期，大部分
        Manual = 1, // 设定时间  
        None = 2, // 不回收，玩家自己处理  
        VisualEffect = 3,
    }

    [OnValueChanged("OnRecycleTypeChanged")]
    public RecycleType recycleType;
    [ShowIf("recycleType", RecycleType.Manual)]
    public float life = 1f; // 手动设置回收时间

    private float tick = 0f;
    private ParticleSystem particleSys;
    private VisualEffect visualEff;

    private void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
        visualEff = GetComponent<VisualEffect>();

        if (recycleType == RecycleType.Particle)
        {
            if (particleSys == null)
                Debug.LogError($"vfx {dbgName} cant find any particle system");
        }
        else if (recycleType == RecycleType.VisualEffect)
        {
            if (visualEff == null)
                Debug.LogError($"vfx {dbgName} cant find any visual effect");
        }
    }

    private void Update() 
    {
        if (recycleType == RecycleType.Particle)
        {
            if (particleSys && particleSys.IsAlive() == false)
                Stop();
        }
        else if (recycleType == RecycleType.Manual)
        {
            tick += Time.deltaTime;
            if (tick >= life)
                Stop();
        }
        else if (recycleType == RecycleType.VisualEffect)
        {
            // 检测结束方式是 aliveParticleCount 或者 HasAnySystemAwake
            // 但是 HasAnySystemAwake 需要确保在下一帧检测，否则范围 false
            // aliveParticleCount 只能使用 == 0, <= 0 直接 stop，怀疑默认是负数
            if (visualEff && visualEff.aliveParticleCount == 0)
                Stop();
        }
    }

    public virtual void Play()
    {
        // Debug.Log($"vfx {dbgName} play");
        gameObject.SetActive(true);

        if (particleSys != null)
            particleSys.Play();
        if (visualEff != null)
            visualEff.Play();
    }

    public virtual void Stop()
    {
        // Debug.Log($"vfx {dbgName} stop");
        gameObject.SetActive(false);

        // 停止粒子系统并清空已生成的粒子
        if (particleSys != null)
            particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (visualEff != null)
            visualEff.Stop();

        sVfx.RecycleVfx(this);
    }

    // 从 pool 取出播放时候
    public virtual void Reset()
    {
        tick = 0f;
    }

#if UNITY_EDITOR
    private void OnRecycleTypeChanged()
    {
        if (recycleType == RecycleType.Particle)
        {
            var psys = GetComponent<ParticleSystem>();
            if (psys == null)
            {
                Debug.LogError("failed set ParticleLife because cant find any ParticleSystem");
                recycleType = RecycleType.Manual;
                return;
            }
        }
        else if (recycleType == RecycleType.VisualEffect)
        {
            var veff = GetComponent<VisualEffect>();
            if (veff == null)
            {
                Debug.LogError("failed set VisualEffect because cant find any VisualEffect");
                recycleType = RecycleType.Manual;
                return;
            }
        }
    }
#endif
}