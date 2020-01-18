using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BounceData", menuName = "BossData/BounceData")]
public class BounceData : BaseData
{
    public float impactFreeze;
    public float shakeMagnitude;
    public float kinetikEnergyLoss;
    public float WaitOnStart;
    public float Deceleration;
    public float BounceAngle;
}
