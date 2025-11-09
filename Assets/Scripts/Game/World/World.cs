using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using NaughtyAttributes;
#endif

public class World : GameBehaviour
{
    public enum State
    {
        Running = 0,
        Pause = 1,
    }
    [HorizontalLine(color: EColor.Red)]
    public State state;
    
}