﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FirstBossRecoveryState : FirstBossState
{
    //Inspector
    public RecoveryData recoveryData;
    public DecelerationData decelerationData;

    //Private
    float timeStartRecovery;
    int iterations;
    int layerResult;
    int layerWall;
    int layerPlayer;

    public override void Enter()
    {

        base.Enter();

        iterations = 1;
        layerWall = 10;
        layerPlayer = 11;

        RecoveryInfoEnter();
    }
    public override void Tick()
    {
        base.Tick();
        SetCycleTimer();
        detectCollsion();

    }
    public override void Exit()
    {
        animator.SetBool("RecoveryOrbit", false);
        CheckVulnerability();
        boss.IsPrevStateReinitialize = false;
    }

    public void RecoveryInfoEnter() {
        timeStartRecovery = Time.time;
        OrbitTag(recoveryData);
    }
   
    void detectCollsion() {
        layerResult = boss.DetectCollision(boss.nextPosition);
        animator.SetInteger("Layer", 0);
    }

}
