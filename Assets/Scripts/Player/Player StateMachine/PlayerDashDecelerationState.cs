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
    float HorizontalDash;
    float VerticalDash;
    float Horizontal;
    float Vertical;

    public override void Enter() {

        Debug.Log("DECEL DASH");

        HorizontalDash = player.horizontalDash;
        VerticalDash = player.verticalDash;

        playerIdleData = player.playerIdleData;
        playerDashData = player.playerDashData;

    }

    public override void Tick() {

        player.DashDeceleration(HorizontalDash , VerticalDash , playerDashData.DashDecelerationTime , playerDashData.ActiveDashDistance , playerDashData.ActiveDashTime);

        if (player.dashMovementSpeed <= (playerDashData.ResumePlayerInput * playerIdleData.maxSpeed)) {

            Horizontal = player.horizontalDash;
            Vertical = player.verticalDash;

            if (Vertical != 0 || Horizontal != 0) {
                Debug.Log(player.dashMovementSpeed);
                animator.SetTrigger(IDLE);
            }

            Debug.Log("SPEED: " + player.dashMovementSpeed);
            
        }
    }

    public override void Exit() {
        
    }
}
