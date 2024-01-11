using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    private Transform mainCameraTF;
    private float mainCameraPosZ;

    [Header("PLAYER-RUNTIME")]
    public Inventory dailyBag;

    public override float walkSpd
    {
        get {
            return stat.FixWithBuff(0.5f, BuffKey.MOVE_SPD, 0.01f);
        }
    }

    public override void Init(int id)
    {
        base.Init(id);

        entityType = EntityType.PLAYER;

        mainCameraTF = Camera.main.transform;
        mainCameraPosZ = mainCameraTF.position.z;
    }

    public override void Reset()
    {
        base.Reset();

        bag.ClearAll();
    }

    public override void Tick(float dt)
    {
        base.Tick(dt);

        mainCameraTF.transform.position = new Vector3(pos.x, pos.y, mainCameraPosZ);
    }

    public Inventory GetInventory()
    {
        return GetComponent<Inventory>();
    }
}
