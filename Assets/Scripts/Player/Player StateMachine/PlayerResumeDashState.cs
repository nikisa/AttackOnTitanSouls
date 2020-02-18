using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerResumeDashState : PlayerBaseState
{
    //Private
    float timeStart;

    PlayerMovementData playerMovementData;
    PlayerDashData playerDashData;
    float DashTimeFrames;
    float timeDeceleration;


    public override void Enter() {
        //timeStart = Time.time;
        //playerDashData = player.playerDashData;
        //playerIdleData = player.playerIdleData;
        //player.InitialDashVelocity = playerDashData.DashDistance / (playerDashData.DashTimeFrames / 60);
        //Debug.Log("InitialDashVelocity: " + player.InitialDashVelocity);
        //DashTimeFrames = playerDashData.DashTimeFrames;
        //timeDeceleration = player.InitialDashVelocity / playerDashData.DashTimeFrames;
    }

    public override void Tick() {
        //Deceleration();

        Debug.Log("SPEED: " + player.dashMovementSpeed);

        if (Time.time - timeStart > player.playerDashData.ResumePlayerInput) {
            animator.SetTrigger(DASH_FRAME_PERFECT);
        }
        if (player.dataInput.Dash) {
            animator.SetTrigger(IDLE);
        }
    }

    public override void Exit() {

    }

    void Deceleration() {
        Debug.Log("PDD: " + player.InitialDashVelocity);
        player.InitialDashVelocity -= timeDeceleration * Time.deltaTime;
        player.transform.Translate(player.InitialDashVelocity * player.dashDirection * Time.deltaTime);
    }

}
