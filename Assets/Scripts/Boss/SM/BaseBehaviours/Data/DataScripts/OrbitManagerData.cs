using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbitManagerData", menuName = "BossData/OrbitManagerData")]
public class OrbitManagerData : BaseData
{
    public float speedRadius;
    public float MaxSpeed;
    public float TimeAcceleration;
    public List<OrbitData> orbitData;
    [HideInInspector]
    public HookPointController CenterRotation;
}
