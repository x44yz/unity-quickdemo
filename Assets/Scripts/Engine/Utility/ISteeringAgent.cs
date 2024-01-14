using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISteeringAgent
{
    float steeringMass { get; }
    Vector3 steeringPos { get; }
    Vector3 steeringVelocity { get; }
    float steeringMaxSpeed { get; }
    float steeringMaxForce { get; }
    float steeringCollisionRadius { get; }
    // Arrival
    float steeringSlowRadius { get; }
    float steeringStopRadius { get; }
    // Avoidance
    bool steeringNeedAvoid { get; }
    float steeringAvoidAHead { get; }
    float steeringAvoidForce { get; }
    // Separation
    float steeringSeparationRadius { get; }
    float steeringSeparationForce { get; }
}