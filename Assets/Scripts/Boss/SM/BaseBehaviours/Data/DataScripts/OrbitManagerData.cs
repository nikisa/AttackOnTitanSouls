using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbitManagerData", menuName = "BossData/OrbitManagerData")]
public class OrbitManagerData : ScriptableObject
{
    [Tooltip("in sec. How long it takes the object to reach Angular Max Speed")]
    public float AngularAccelerationTime;
    [Tooltip("in degrees/sec. Positive values: orbit clockwise. Negative values: orbit anticlockwise")]
    public float AngularMaxSpeed;
    public bool hasAngularDeleceration;
    [Tooltip("in sec. How long it takes the object to reach 0 angular speed")]
    public float AngularDecelerationTime;
    [Tooltip("[in sec. Can’t be less then Angular Acceleration Time + Angular Deceleration Time")]
    public float OrbitTravelTime;

    public List<OrbitData> orbitData;
    [HideInInspector]
    public HookPointController CenterRotation;



}
