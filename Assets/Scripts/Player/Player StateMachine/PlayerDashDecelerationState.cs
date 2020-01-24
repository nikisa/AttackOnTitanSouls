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
    float Horizontal;
    float Vertical;

    public override void Enter() {

        Horizontal = player.dataInput.Horizontal;
        Vertical = player.dataInput.Vertical;

        playerIdleData = player.playerIdleData;
        playerDashData = player.playerDashData;

    }

    public override void Tick() {

        player.DashDeceleration(Horizontal , Vertical , playerDashData.DashDecelerationTime , playerDashData.ActiveDashDistance , playerDashData.ActiveDashTime);

        if (player.dashMovementSpeed <= (playerDashData.ResumePlayerInput * playerIdleData.maxSpeed)) {
            if (Vertical != 0 || Horizontal != 0) {
                animator.SetTrigger(IDLE);
            }
        }
    }

    public override void Exit() {
        
    }
}
