using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DecelerationData", menuName = "BossData/DecelerationData")]
public class DecelerationData : BaseData
{
    [Tooltip("Decreasing rate of entry speed, in m/sec^2.")]
    public float TimeDeceleration;
    [Tooltip("Minimum reachable speed, in m/sec. Use negative value to reverse vector")]
    public float LowSpeed;

}
