using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    public int id;
    public string buffName;
    public Sprite spr;
    public int rarity;

    public abstract string Desc();
    public abstract System.Type InstType();
    public abstract void ApplyToActor(Actor actor);
}
