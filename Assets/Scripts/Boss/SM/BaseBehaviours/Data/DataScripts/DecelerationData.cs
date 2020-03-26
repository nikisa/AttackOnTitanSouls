using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DecelerationData", menuName = "BossData/DecelerationData")]
public class DecelerationData : BaseData
{

    public AnimationCurve MovementDecelerationCurve;
    [Tooltip("Decreasing rate of entry speed, in m/sec^2.")]
    public float Deceleration;
    [Tooltip("Minimum reachable speed, in m/sec. Use negative value to reverse vector")]
    public float LowSpeed;

}
