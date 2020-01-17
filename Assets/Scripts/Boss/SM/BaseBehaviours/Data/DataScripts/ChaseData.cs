using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChaseData", menuName = "BossData/ChaseData")]
public class ChaseData : BaseData
{

    public TargetType Target;
    public bool HasAcceleration;
    [Tooltip("Increasing rate of entry speed, in m/sec^2")]
    public float TimeAcceleration;
    [Tooltip("Maximum reachable speed, in m/sec.")]
    public float MaxSpeed;
    public bool HasVectorRotationRate;
    [Tooltip("in degrees / sec.")]
    public float VectorRotationRate;
    public float DynamicDrag;


}
