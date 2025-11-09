using UnityEngine;

public class MoveAction : GAction
{
    private Vector3 moveDir;

    public MoveAction(Vector3 moveDir)
    {
        this.moveDir = moveDir.normalized;
    }

    protected override GActionResult OnExecute(float dt) 
    {
        Vector3 velocity = moveDir * source.GetMoveSpd();
        var dist = velocity * dt;
        source.pos += dist;
        
        return GActionResult.Running; 
    }
}