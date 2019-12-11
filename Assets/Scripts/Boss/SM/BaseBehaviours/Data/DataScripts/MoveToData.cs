﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MoveToData", menuName = "BossData/MoveToData")]
public class MoveToData : BaseData
{

    [Tooltip("Maximum reachable speed, in m/sec.")]
    public float MaxSpeed;
    [Tooltip("Increasing rate of entry speed, in m/sec^2")]
    public float TimeAcceleration;
    [Tooltip("On State Enter: wait X seconds before accelerating")]
    float WaitOnStart;
    public bool HasAcceleration;
    [Tooltip("Exits the state when it passes over the coordinates of the target")] [HideInInspector]
    public bool StopsAtTargetOvertaking; // per testing 
    [Tooltip("Decreasing rate of entry speed, in m/sec^2.")]
    public float TimeDeceleration;
    [Tooltip("Minimum reachable speed, in m/sec. Use negative value to reverse vector")]
    public float LowSpeed;
    //[Tooltip("in sec")]
    //public float MoveToDuration;
    [Tooltip ("If true, the object leaves the Move To State on collision enter")]
    public bool StopOnSolid;
    [Tooltip ("The value to add (or subtract, if negative) to the entry velocity parameter.")]
    public float AddToVelocity;
    // scia
    public GameObject TrailOb;
    public float TrailDelay;
}
