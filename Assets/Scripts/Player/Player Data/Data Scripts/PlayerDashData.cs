using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Dash Data")]
public class PlayerDashData : ScriptableObject
{

    // Public
    public float DashDistance;
    public float DashTimeFrames;
    public float ResumeControl;
    public float TimeFreeze;
    public float SetTimeScale;
}
