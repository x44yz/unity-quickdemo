using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharClickToMove : CharController, IInteractSource
{
    private readonly int hashSpeedPara = Animator.StringToHash("Speed");
    private readonly int hashLocomotionTag = Animator.StringToHash("Locomotion");
    public const string startingPositionKey = "starting position";
    private const float stopDistanceProportion = 0.1f;
    private const float navMeshSampleDistance = 4f;

    public Animator animator;
    public NavMeshAgent agent;
    // public SaveData playerSaveData;
    public float turnSmoothing = 15f;
    public float speedDampTime = 0.1f;
    public float slowingSpeed = 0.175f;
    public float turnSpeedThreshold = 0.5f;
    public float inputHoldDelay = 0.5f;

    private ICharInteractObj curInteractTarget;
    private Vector3 destPos;
    private bool handleInput = true;
    private WaitForSeconds inputHoldWait;

    public event System.Action onStop;

    public override bool isMoving
    {
        get
        {
            if (agent.enabled == false) return false;
            // if (agent.isStopped) return false; // Slowing 的时候也会停止  
            // if (agent.pathPending) return true;
            if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion) return false;
            return true;
        }
    }

    private void Start()
    {
        agent.updateRotation = false;

        inputHoldWait = new WaitForSeconds(inputHoldDelay);

        // Load the starting position from the save data and find the transform from the starting position's name.
        // string startingPositionName = "";
        // playerSaveData.Load(startingPositionKey, ref startingPositionName);
        // Transform startingPosition = StartingPosition.FindStartingPosition(startingPositionName);

        // transform.position = startingPosition.position;
        // transform.rotation = startingPosition.rotation;

        destPos = transform.position;
    }

    private void OnAnimatorMove()
    {
        agent.velocity = animator.deltaPosition / Time.deltaTime;
    }

    public override void SetEnabled(bool v)
    {
        base.SetEnabled(v);
        agent.enabled = v;
    }

    private void Update()
    {
        if (agent.pathPending)
            return;

        float speed = agent.desiredVelocity.magnitude;

        if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
            Stopping(out speed);
        else if (agent.remainingDistance <= agent.stoppingDistance)
            Slowing(out speed, agent.remainingDistance);
        else if (speed > turnSpeedThreshold)
            Moving();

        animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
    }

    private void Stopping(out float speed)
    {
        agent.isStopped = true;
        transform.position = destPos;
        speed = 0f;

        if (curInteractTarget != null)
        {
            transform.rotation = curInteractTarget.GetInteractRot();

            curInteractTarget.Interact(this);
            curInteractTarget = null;

            StartCoroutine(WaitForInteraction());
        }

        if (onStop != null)
        {
            onStop.Invoke();
        }
    }

    private void Slowing(out float speed, float distanceToDestination)
    {
        agent.isStopped = true;
        float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;

        Quaternion targetRotation = curInteractTarget != null ? curInteractTarget.GetInteractRot() : transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionalDistance);
        transform.position = Vector3.MoveTowards(transform.position, destPos, slowingSpeed * Time.deltaTime);

        speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);
    }

    private void Moving()
    {
        Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
    }

    public void OnGroundClick(BaseEventData data)
    {
        if (enabled == false)
            return;
        if (!handleInput)
            return;

        curInteractTarget = null;
        PointerEventData pData = (PointerEventData)data;

        // Try and find a point on the nav mesh nearest to the world position of the click and set the destination to that.
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
            destPos = hit.position;
        else
            // In the event that the nearest position cannot be found, set the position as the world position of the click.
            destPos = pData.pointerCurrentRaycast.worldPosition;

        agent.SetDestination(destPos);
        agent.isStopped = false;
    }

    public void OnInteractTargetClick(ICharInteractObj t)
    {
        if (enabled == false)
            return;
        if (!handleInput)
            return;

        curInteractTarget = t;
        destPos = curInteractTarget.GetInteractPos();

        agent.SetDestination(destPos);
        agent.isStopped = false;
    }

    private IEnumerator WaitForInteraction()
    {
        handleInput = false;
        yield return inputHoldWait;

        while (animator.GetCurrentAnimatorStateInfo(0).tagHash != hashLocomotionTag)
        {
            yield return null;
        }

        handleInput = true;
    }

    public override void SetDestination(Vector3 pos, Quaternion rot)
    {
        destPos = pos;

        agent.SetDestination(pos);
        agent.isStopped = false;
    }
}
