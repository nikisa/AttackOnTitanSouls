﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Idle Data")]
public class PlayerIdleData : ScriptableObject {
    //Inspector
    public bool isHookTest;
    public float maxSpeed;
    public float framesZeroToMax;
    public float framesMaxToZero;
    public float resumeControl;
}
