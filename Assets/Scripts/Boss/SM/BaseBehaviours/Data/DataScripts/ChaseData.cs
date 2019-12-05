using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChaseData", menuName = "BossData/ChaseData")]
public class ChaseData : ScriptableObject
{ 
    [Tooltip("Maximum reachable speed, in m/sec.")]
    public float MaxSpeed;
    public bool HasAngularSpeed;
    [Tooltip("in degrees / sec.")]
    public float AngularSpeed;
    
    public float TimeChase;
}
