using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public virtual void SetEnabled(bool v)
    {
        enabled = v;
    }

    public virtual bool isMoving => false;

    public virtual void SetDestination(Vector3 pos, Quaternion rot)
    {
    }
}
