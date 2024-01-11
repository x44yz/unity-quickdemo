using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQueueLineTarget
{
    // bool ServeOpen();
    // bool ServeClose();
    bool CanServeActor();
    void ServeActor(Actor actor);
}
