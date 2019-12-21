using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "OrbitData", menuName = "BossData/OrbitData")]
public class OrbitData : ScriptableObject
{
    //Inspector
    public float InitialRadius;
    public bool HasDeltaRadius;
    public float FinalRadius;
    public float Angle;
    public float OrbitSpeed;
    [HideInInspector]
    public float initialPosition;
    public bool HasPingPong;
}
