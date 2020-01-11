using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RecoveryData", menuName = "BossData/RecoveryData")]
public class RecoveryData : BaseData
{

    [Tooltip("On State Enter: wait X seconds before decelerating")]
     float WaitOnStart;
    [Tooltip("Minimum reachable speed, in m/sec. Use negative value to reverse vector")]
    public float LowSpeed;
}
