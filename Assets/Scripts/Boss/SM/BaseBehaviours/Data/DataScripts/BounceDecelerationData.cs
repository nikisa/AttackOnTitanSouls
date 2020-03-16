using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BounceDecelerationData", menuName = "BossData/BounceDecelerationData")]
public class BounceDecelerationData : BaseData
{
    [Tooltip("Decreasing rate of entry speed, in m/sec^2.")]
    public float ImpulseDeceleration;

}
