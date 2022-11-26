using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;
using QuickDemo.FSM;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Unit : MonoBehaviour, IStateMachineOwner
{
    public const float ATTACK_PRETIME = 0.7f;

    public static readonly int HASH_WALK = Animator.StringToHash("Walk");
    public static readonly int HASH_ATTACK = Animator.StringToHash("Attack");
    public static readonly int HASH_DEATH = Animator.StringToHash("Death");
    public static readonly int HASH_WORK = Animator.StringToHash("Work");

    public GameObject model;
    public Transform lhandContainerTF;
    public Transform rhandContainerTF;
    public bool debugFSM;
    public float bodyRadius;

    [Header("RUNTIME")]
    public bool isSelected = false;
    public Animator animator = null;
    public StateMachine<Unit> motionFSM;
    public bool hasMoveTarget = false;
    public Vector3 moveTargetPos;
    public Vector3 velocity;

    public GameConfig gCfgs { get { return GameConfig.Inst; } }
    public virtual bool IsFSMDebug { get { return debugFSM; }  }
    public virtual string FSMDebugLogPrefix { get { return name; } }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector3 forward 
    { 
        get { return transform.forward; }
        set { transform.forward = value; }
    }

    public bool visible
    {
        get { return model.activeSelf; }
        set { model.SetActive(value); }
    }

    public virtual bool isDead { get { return false; } }
    public virtual bool isAttackable { get { return !isDead; } }
    public virtual float moveSpeed { get { return 4f; } }
    public virtual float stopRadius { get { return 0.7f; } }
    public bool isMoving { get { return motionFSM != null && motionFSM.curState is USMove; } }

    void Awake()
    {
        forward = Vector3.back;
        animator = GetComponentInChildren<Animator>();

        motionFSM = new StateMachine<Unit>(this);
        USIdle idle = new USIdle(this);
        USMove move = new USMove(this);

        motionFSM.Register(idle);
        motionFSM.Register(move);

        motionFSM.AddTransition(new Transition(idle, move, idle.IsTranslateToMove));
        motionFSM.AddTransition(new Transition(move, idle, move.IsTranslateToIdle));

        motionFSM.Translate(typeof(USIdle));

        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    void Update()
    {
        float dt = Time.deltaTime;
        motionFSM.Update(dt);

        // swap
        if (curCommand == null && todoCommand != null)
        {
            curCommand = todoCommand;
            todoCommand = null;
        }

        OnUpdate(dt);
    }

    protected virtual void OnUpdate(float dt)
    {
    }

    public void SetMoveTargetPos(Vector3 pos)
    {
        hasMoveTarget = true;
        moveTargetPos = pos;
    }

    // 2d - Y
    public bool IsTargetAtRange(Vector3 point, float minDist = 0.1f)
    {
        if (point == null)
            return false;
        float dist = (pos - point).ZeroYSquardLen();
        // Debug.Log("xx-- dist > " + dist);
        return dist < minDist;
    }

    public virtual void LockedByAttacker(Unit attacker)
    {
    }

    public virtual void UnlockedByAttacker(Unit attacker)
    {
    }

    public virtual void TakeAttack(Unit attacker, float atk)
    {
        Debug.LogError("not implement take attack");
    }

    private Command curCommand = null;
    private Command todoCommand = null;

    public virtual bool CanTakeCommand(CommandId commandId)
    {
        return true;
    }

    public void TakeCommand(CommandId commandId)
    {
        var cmd = new Command();
        cmd.id = commandId;
        TakeCommand(cmd);
    }

    public void TakeCommand(Command cmd)
    {
        todoCommand = cmd;
    }

    public bool IsCommandTaken(CommandId commandId)
    {
        return curCommand != null && curCommand.id == commandId;
    }

    public bool IsNewCommandTaken()
    {
        return curCommand != null || todoCommand != null;
    }

    public Command RunCommand(CommandId commandId)
    {
        if (IsCommandTaken(commandId))
        {
            var cmd = curCommand;
            curCommand = null;
            return cmd;
        }
        Debug.LogError($"[CMD]cant runcommand because command not taken > {commandId}");
        return null;
    }

    public List<ISteeringAgent> avoidAgents = null;
    public void SetSteeringAvoidAgents(List<ISteeringAgent> sepAgents)
    {
        avoidAgents = sepAgents;
    }
    
    public virtual List<ISteeringAgent> GetSteeringAvoidAgents()
    {
        return avoidAgents;
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos() 
    {
        Vector3 center = pos + Vector3.up * 0.01f;

        Handles.color = Color.red;
        Handles.DrawWireDisc(center, Vector3.up, bodyRadius);

        // Gizmos.color = Color.blue;
        // Gizmos.DrawLine(center, velocity.normalized * 1f);

        if (hasMoveTarget)
        {
            Gizmos.color = Color.green;
            Vector3 target = new Vector3(moveTargetPos.x, center.y, moveTargetPos.z);
            Gizmos.DrawLine(center, target);
            Gizmos.DrawSphere(target, 0.1f);
        }
    }
#endif
}

public class UnitState : State
{
    protected Unit owner;

    public UnitState(Unit owner)
    {
        this.owner = owner;
    }
}

public class USIdle : UnitState
{

    public float idleTick = 0f;

    public USIdle(Unit owner) : base(owner)
    {
    }

    public override void OnEnter(State from)
    {
        idleTick = Utils.Rand(1f, 2f);

        owner.animator.SetBool(Unit.HASH_WALK, false);
    }

    public override void OnExit(State to)
    {
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
    }

    public bool IsTranslateToMove()
    {
        return owner.hasMoveTarget;
    }
}

public class USMove : UnitState
{
    public USMove(Unit owner) : base(owner)
    {
    }

    public override void OnEnter(State from)
    {
        owner.animator.SetBool(Unit.HASH_WALK, true);
    }

    public override void OnExit(State to)
    {
        owner.animator.SetBool(Unit.HASH_WALK, false);
        owner.hasMoveTarget = false;
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);

        Vector3 dist = (owner.moveTargetPos - owner.pos).ZeroY();
        Vector3 dir = dist.normalized;
        // if (moveDir.sqrMagnitude > 0.01f)
        if (Utils.IsZero(dir) == false)
        {
            // Debug.Log($"xx-- {owner.name} move dir > {moveDir.sqrMagnitude}");
            owner.forward = Vector3.Lerp(owner.forward, dir, 0.7f);
        }

        Vector3 deltaDist = dir * owner.moveSpeed * dt;
        if (deltaDist.sqrMagnitude >= dist.sqrMagnitude)
        {
            deltaDist = dist;
            owner.hasMoveTarget = false;
        }
        owner.pos += deltaDist;
    }

    public bool IsTranslateToIdle()
    {
        return owner.hasMoveTarget == false;
    }
}
