using UnityEngine;

public class CallbackAction : GAction
{
    private System.Action callback;

    public CallbackAction(System.Action cb)
    {
        callback = cb;
    }

    protected override GActionResult OnExecute(float dt)
    {
        callback.Invoke();
        return GActionResult.Success;
    }
}