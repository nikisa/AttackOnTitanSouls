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
    [Tooltip("in sec")]
    public float MoveToDuration;
    [Tooltip ("If true, the object leaves the Move To State on collision enter")]
    public bool StopOnSolid;
    [Tooltip ("The value to add (or subtract, if negative) to the entry velocity parameter.")]
    public float AddToVelocity;
    // scia
    public GameObject TrailOb;
    public float TrailDelay;
}
