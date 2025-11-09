using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Defs
{
    public const int INVALID_UID = -1;

    public const int ONE_DAY_HOUR = 24;
    public const int ONE_DAY_MINS = ONE_DAY_HOUR * 60;
    public const int ONE_HOUR_MINS = 60;
}

public enum EntityType
{
    INVALID = -1,
    ITEM = 0,
    PLAYER = 1,
    NPC = 2,
}

public static class TagDef
{
    public const string PLAYER = "Player";

    public static readonly string[] All = new string[]
    {
        PLAYER,
    };
}

[Flags]
public enum ItemType
{
    NONE = 0,
    MATERIAL = 1 << 0,
    FOOD = 1 << 1,
    DRINK = 1 << 2,
    TOOL = 1 << 3,
}

public enum ItemId
{
    NONE = -1,
    // MATERIAL
    CASH = 0,
    POWER = 1,
    // FRUIT
    BANANA = 100,
    APPLE = 101,
    ORANGE = 102,
    GRAPE = 103,
    LEMON = 104,
    MANGO = 105,
    STRAW_BERRY = 106,
    PINEAPPLE = 107,
    PEACH = 108,
    PEAR = 109,
    WATERMELON = 110,
    CHERRY = 111,
    KIWI = 112,
    AVOCADO = 113,
    // DRINK
    COLA = 200,
    // FASTFOOD 
    SANDWICH = 300,
    // TOOL
    TRASH_CAN = 2000,
}

public enum BuffKey
{
    MOVE_SPD = 0,
}

public enum ValueType
{
    Constant = 0,
    Percent = 1,
}
public enum Operator
{
    EQUAL = 0,
    NOTEQUAL = 1,
    GREATER = 2,
    GREATER_OR_EQUAL = 3,
    LESS = 4,
    LESS_OR_EQUAL = 5,
}

public enum Direction
{
    NONE = -1,
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 3,
}
public enum ResolutionType
{
    FULLSCREEN = 0,
    R2560X1440 = 1,
    R1920X1080 = 2,
    R1366X768 = 3,
}

public static class LayerDef
{
    public const string MONSTER = "Monster";

    public static readonly int IDX_MONSTER = LayerMask.NameToLayer(MONSTER);

    public static readonly int MASK_MONSTER = LayerMask.GetMask(MONSTER);
}

public enum Stat
{
    None = -1,
    Hp = 0,
    MoveSpeed = 1,
    Damage = 2,
    AttackSpeed = 3,
    MaxHp = 4,
    AttackRange = 5,
    ProjectileCount = 6,
    EffectRange = 7,
    EffectDuration = 8,
    MatchSlot = 9,
    ProjectileSpeed = 10,
    PickRange = 11,
    LifeSteal = 12,
    Knockback = 13, // 击退  
}

public enum ModifyValueType
{
    Bonus = 0,
    Scale = 1,
}

public enum TargetScope
{
    Hero = 0,
    Enemy = 1,
}

public enum RuneType
{
    Weapon = 0,
    StatModifier = 1,
}

public enum StatModifyTarget
{
    Hero = 0,
    AllWeapon = 1,
    RandWeapon = 2,
}

public static class VFXName
{
    public const string MonsterHit = "MonsterHitVFX";
}