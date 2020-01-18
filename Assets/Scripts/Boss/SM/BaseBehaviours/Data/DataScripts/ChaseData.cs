using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChaseData", menuName = "BossData/ChaseData")]
public class ChaseData : BaseData
{

    public TargetType Target;
    [Tooltip("in sec. How long it takes the object to reach Max Speed.")]
    //[AbsoluteValue()]
    public float TimeAcceleration;
    [Tooltip("Maximum reachable speed, in m/sec.")]
    public float MaxSpeed;
    [HideInInspector]
    public float DynamicDrag;


}
