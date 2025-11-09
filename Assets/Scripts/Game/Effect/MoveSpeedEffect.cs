using UnityEngine;

public class MoveSpeedEffect : TimeEffect
{
    public override Stat GetEffectStat() => Stat.MoveSpeed;

    public ModifyValueType valueType;
    public float value;

    public override void Init(EffectSO cfg)
    {
        var scfg = cfg as MoveSpeedEffectSO;
        if (scfg == null)
        {
            Debug.LogError("not slow effect cfg");
            return;
        }

        duration = scfg.duration;
        valueType = scfg.valueType;
        value = scfg.value;
    }

    protected override void OnActivate()
    {
        owner.AddStatus(Status.Slowed);
        owner.ModifyStat(Stat.MoveSpeed, valueType, value, true);
    }

    protected override void OnDeactivate()
    {
        owner.RemoveStatus(Status.Slowed);
        owner.ModifyStat(Stat.MoveSpeed, valueType, value, false);
    }
}