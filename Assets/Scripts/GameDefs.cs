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

public enum Stat
{
    HUNGER = 0,
    COUNT,
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
    CONSTANT = 0,
    PERCENT = 1,
}