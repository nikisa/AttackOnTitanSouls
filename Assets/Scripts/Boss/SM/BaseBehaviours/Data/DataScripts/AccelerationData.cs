using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AccelerationData", menuName = "BossData/AccelerationData")]
public class AccelerationData : BaseData
{
    
    [Tooltip("Increasing rate of entry speed, in m/sec^2")]
    public float TimeAcceleration;
    [Tooltip("On State Enter: wait X seconds before accelerating")]
    public float WaitOnStart;

    public bool HasAcceleration;
}
