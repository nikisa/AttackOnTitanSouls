using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChaseData", menuName = "BossData/ChaseData")]
public class ChaseData : BaseData
{ 
    [Tooltip("Maximum reachable speed, in m/sec.")]
    public float MaxSpeed;
    public bool HasVectorRotationRate;
    [Tooltip("in degrees / sec.")]
    public float VectorRotationRate;
    [Tooltip("n m. If the target distance is major this value, the object leaves the state")]
    public float ChaseRadius;
    [Tooltip("Increasing rate of entry speed, in m/sec^2")]
    public float TimeAcceleration;
    public bool HasAcceleration;

}
