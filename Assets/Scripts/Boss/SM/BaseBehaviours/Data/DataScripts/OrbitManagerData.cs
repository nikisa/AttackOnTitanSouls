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
    [Tooltip("in sec. How long it takes the object to reach 0 angular speed")]
    public float AngularDecelerationTime;

    public List<OrbitData> orbitData;
    
    [HideInInspector]
    public HookPointController CenterRotation;

}
