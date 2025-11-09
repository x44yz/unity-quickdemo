using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum GActionResult
{
    Success, // success + done
    Failure, // fail + done
    Running, // success + not done  
}

public abstract class GAction
{
    public static GameMgr game => GameMgr.Inst;
    public static ResSystem sRes => game.sRes;

    protected GActionResult result;
    protected Entity source;

    public bool IsDone() => result != GActionResult.Running;

    public void Bind(Entity et)
    {
        source = et;
    }

    public void Start()
    {
        result = GActionResult.Running;
        OnStart();
    }

    public virtual GActionResult Execute(float dt)
    {
        if (IsDone())
            return result;

        result = OnExecute(dt);
        return result;
    }

    public void End()
    {
        OnEnd();
    }

    protected virtual void OnStart() {}
    protected virtual void OnEnd() {}
    protected virtual GActionResult OnExecute(float dt) 
    { 
        return GActionResult.Success; 
    }
}
