using UnityEngine;

public enum AnimationId
{
    NONE = -1,
    IDLE = 0,
    ATTACK = 1,
    MOVE = 2,
}

public enum AnimationFlag
{
    NONE = 0,
    REPEAT = 1,
}

public static class ANIHashId
{
    public static readonly int PLAY = Animator.StringToHash("Play");
}