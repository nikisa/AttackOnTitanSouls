using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerResumeDashState : PlayerBaseState
{
    //Private
    float timeStart;

    PlayerIdleData playerIdleData;
    PlayerDashData playerDashData;
    float DashTimeFrames;
    float timeDeceleration;


    public override void Enter() {
        

        timeStart = Time.time;

        DashTimeFrames = playerDashData.DashTimeFrames;
        playerDashData = player.playerDashData;
        playerIdleData = player.playerIdleData;
        player.InitialDashVelocity = 1 + 1;//playerDashData.DashDistance / (DashTimeFrames / 60);
        
        timeDeceleration = player.InitialDashVelocity / DashTimeFrames;
        
    }

    public override void Tick() {

        //Deceleration();

        if (Time.time - timeStart > player.playerDashData.ResumeControl) {
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
