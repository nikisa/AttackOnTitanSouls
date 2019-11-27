using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MoveToData", menuName = "BossData/MoveToData")]
public class MoveToData : ScriptableObject
{

    [Tooltip("Maximum reachable speed, in m/sec.")]
    public float MaxSpeed;
    [Tooltip("Exits the state when it passes over the coordinates of the target")]
    public bool StopsAtTargetOvertaking; // per testing 
}
