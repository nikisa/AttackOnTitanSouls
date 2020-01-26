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
    bool IsTimerSet;
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

            if (!IsTimerSet) // da sitemare
            {
                
                PlayerController.TimerEvent();
                IsTimerSet = true;
            }
            Horizontal = player.dataInput.Horizontal; /*player.horizontalDash;*/
            Vertical = player.dataInput.Vertical; /*player.verticalDash;*/

            if (Vertical != 0 || Horizontal != 0) {
                Debug.Log(player.dashMovementSpeed + " --player.dashMovementSpeed-- ");
                animator.SetTrigger(IDLE);
            }


            
        }
    }

    public override void Exit() {
        
    }
}
