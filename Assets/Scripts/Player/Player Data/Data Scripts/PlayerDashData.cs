using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Dash Data")]
public class PlayerDashData : ScriptableObject
{

    // Public
    public AnimationCurve DashCurve;
    public float PreDashFreeze;
    public float ActiveDashDistance;
    public float ActiveDashTime;
    public float DashDecelerationTime;
    public AnimationCurve DashDecelerationCurve;
    [Tooltip("[in %. Enable player’s input during deceleration when the PC reaches a velocity beetween zero (0) and MaxSpeed (1).")]
    [Range(0,1)]
    public float ResumePlayerInput;
    public float EnableDashAt;

    [HideInInspector]
    public float frame = 1f / 60f;

}
