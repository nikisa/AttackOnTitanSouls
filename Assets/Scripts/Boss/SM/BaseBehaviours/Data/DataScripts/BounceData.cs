using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BounceData", menuName = "BossData/BounceData")]
public class BounceData : BaseData
{
    public float impactFreeze;
    public float shakeMagnitude;
    [Range(0,1)]
    public float kinetikEnergyLoss;
    [Range(0,1)]
    public float surfaceFriction;

    //public float WaitOnStart;
    //public float Deceleration;
    //public float BounceAngle;
}
