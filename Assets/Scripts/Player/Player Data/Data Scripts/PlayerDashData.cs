using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Dash Data")]
public class PlayerDashData : ScriptableObject
{

    // Public
    public bool isHookTest;
    public float DashDistance;
    public float DashTimeFrames;
    public Ease DashEase;
    public float ResumeControl;
    public float DashTimeFreeze;
    public float FrameCombo;
    public float TimeFreeze;
    public float Rallenty;
}
