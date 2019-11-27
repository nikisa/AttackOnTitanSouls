using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RotationDecelerationData", menuName = "BossData/RotationDecelerationData")]
public class RotationDecelerationData : ScriptableObject
{
    public float LowSpeed;
    [Tooltip("Decrese rate of entry rotation speed , in degrees/sec^2.")]
    public float DecelerationTime;
    [Tooltip("On State Enter: wait X seconds before accelerating")]
    public float WaitOnStart;
}
