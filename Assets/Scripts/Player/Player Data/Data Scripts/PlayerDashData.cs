using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Dash Data")]
public class PlayerDashData : ScriptableObject
{

    // Public
    public float PreDashFreeze;
    public float ActiveDashDistance;
    public float ActiveDashTime;
    public float DashDecelerationTime;
    public float ResumeControl;
    public float EnableDashAt;
}
