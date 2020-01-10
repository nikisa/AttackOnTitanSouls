using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "OrbitData", menuName = "BossData/OrbitData")]
public class OrbitData : ScriptableObject
{
    //Inspector
    public bool isSetup;
    [Tooltip("in meters. The distance from Center")]
    public float SetupRadius;
    [Tooltip("Variation of Radius in time")]
    public bool HasDeltaRadius;
    [Tooltip("in meters. The distance from Center at the end of the OrbitMoveTo")]
    public float FinalRadius;
    [Tooltip("in sec. How long it takes the object to go from Initial Radius value to Final one")]
    public float TravelTime;
    public Ease OrbitMoveToEase;
}
