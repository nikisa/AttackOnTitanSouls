using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RecoveryData", menuName = "BossData/RecoveryData")]
public class RecoveryData : BaseData
{
    // Start is called before the first frame update
    //public float RecoveryTime;
    [Tooltip("Decreasing rate of entry speed, in m/sec^2.")]
    public float TimeDeceleration;
    [Tooltip("On State Enter: wait X seconds before decelerating")]
     float WaitOnStart;
    [Tooltip("Minimum reachable speed, in m/sec. Use negative value to reverse vector")]
    public float LowSpeed;
}
