using System;

// 注意，有些状态是有优先级，有些是互斥的，
// 有些是可以共存的，比如燃烧和中毒

// [Flags]
// public enum Status
// {
//     None = 0,
//     // live status
//     Alive = 1 << 0,
//     Dead = 1 << 1,
//     Ghost = 1 << 2,
//     // move status
//     Rooted = 1 << 3, // 定身
//     Slowed = 1 << 4, // 减速
//     Hasted = 1 << 5, // 加速
//     Stunned = 1 << 6, // 眩晕
//     Frozen = 1 << 7, // 冰冻
//     // action status 
//     Charmed = 1 << 8, // 魅惑，攻击友方
//     Feared = 1 << 9, // 恐惧，随机移动
//     Confused = 1 << 10, // 混乱，随机攻击目标
//     Silenced = 1 << 11, // 沉默，无法使用技能
//     Disarmed = 1 << 12, // 缴械，无法普通攻击
//     // resist status
//     Invincible = 1 << 13, // 无敌，免疫所有伤害
//     Ethereal = 1 << 14, // 虚无，免疫物理伤害
//     PhysicalImmune = 1 << 15, // 物理免疫  
//     MagicImmune = 1 << 16, // 魔法免疫
//     DamageReduction = 1 << 17, // 减伤
//     DamageAmplification = 1 << 18, // 易伤
//     // element status
//     Buring = 1 << 19, // 燃烧
//     Poisoned = 1 << 20, // 中毒
// }

public enum Status
{
    None = 0,
    // LiveStatusBegin = Alive,
    // LiveStatusEnd = Ghost,
    // move status
    Rooted = 4, // 定身
    Slowed = 5, // 减速
    Hasted = 6, // 加速
    Stunned = 7, // 眩晕
    Frozen = 8, // 冰冻
    // MoveStatusBegin = Rooted,
    // MoveStatusEnd = Frozen,
    // action status 
    Charmed = 9, // 魅惑，攻击友方
    Feared = 10, // 恐惧，随机移动
    Confused = 11, // 混乱，随机攻击目标
    Silenced = 12, // 沉默，无法使用技能
    Disarmed = 13, // 缴械，无法普通攻击
    // ActionStatusBegin = Charmed,
    // ActionStatusEnd = Disarmed,
    // resist status
    Invincible = 14, // 无敌，免疫所有伤害
    Ethereal = 15, // 虚无，免疫物理伤害
    PhysicalImmune = 16, // 物理免疫  
    MagicImmune = 17, // 魔法免疫
    DamageReduction = 18, // 减伤
    DamageAmplification = 19, // 易伤
    // element status
    Buring = 20, // 燃烧
    Poisoned = 21, // 中毒
}

public static class StatusUtils
{
    // public static bool IsLiveStatus(this Status st)
    // {
    //     return st == Status.Alive ||
    //         st == Status.Dead ||
    //         st == Status.Ghost;
    // }
}