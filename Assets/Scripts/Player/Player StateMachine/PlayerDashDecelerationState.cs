using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashDecelerationState : PlayerBaseState
{

    //Private
    PlayerIdleData playerIdleData;
    PlayerDashData playerDashData;
    float InitialVelocity;
    float DashTimeFrames;
    float timeDeceleration;

    public override void Enter() {
        DashTimeFrames = playerDashData.DashTimeFrames;
        playerDashData = player.playerDashData;
        playerIdleData = player.playerIdleData;
        InitialVelocity = playerDashData.DashDistance / DashTimeFrames;
        timeDeceleration = InitialVelocity / DashTimeFrames;
    }

    public override void Tick() {
        Deceleration();
    }

    public override void Exit() {
        
    }

    void Deceleration() {
        
        InitialVelocity -= timeDeceleration * Time.deltaTime;
        if (InitialVelocity <= playerIdleData.maxSpeed) {

            animator.SetTrigger(DASH_RESUME);
        }
    }

}
