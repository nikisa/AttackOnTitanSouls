using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RotationAccelerationData", menuName = "BossData/RotationAccelerationData")]
public class RotationAccelerationData : ScriptableObject
{
 
    [Tooltip("Increasing rate of entry rotation speed , in degrees/sec^2.")]
    public float AccelerationTime;
    [Tooltip("On State Enter: wait X seconds before accelerating")]
    public float WaitOnStart;
}
