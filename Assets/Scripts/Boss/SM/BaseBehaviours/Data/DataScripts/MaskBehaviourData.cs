using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "MaskBehaviourData", menuName = "BossData/MaskBehaviourData")]
public class MaskBehaviourData : BaseData
{
    //Inspector

    public int ID;

    #region AngularMovement info
    [Tooltip("in sec. How long it takes the object to reach Angular Max Speed")]
    public float AngularAccelerationTime;
    [Tooltip("in degrees/sec. Positive values: orbit clockwise. Negative values: orbit anticlockwise")]
    public float AngularMaxSpeed;
    [Tooltip("in sec. How long it takes the object to reach 0 angular speed")]
    public float AngularDecelerationTime;
    #endregion

    #region RadiusMovement info
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
    #endregion
}