using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashFramePerfectState : PlayerBaseState {

    [HideInInspector]
    public PlayerDashData playerDashData;
    //public float frames;

    //Private
    float startTime;

    PlayerIdleData playerIdleData;
    float InitialVelocity;
    float DashTimeFrames;
    float timeDeceleration;

    public override void Enter() {
        playerDashData = player.playerDashData;
        startTime = Time.time;

        DashTimeFrames = playerDashData.DashTimeFrames;
        playerDashData = player.playerDashData;
        playerIdleData = player.playerIdleData;
        timeDeceleration = InitialVelocity / DashTimeFrames;

    }
    public override void Tick() {

        Deceleration();

        if ((Time.time - startTime) < DashTimeFrames / 60) {
            if (player.dataInput.Dash) {
                Debug.Log("PERFECT TIME");
                animator.SetTrigger(DASH);
            }
        }
        else {
            animator.SetTrigger(IDLE);
        }
    }

    public override void Exit() {

    }

    void Deceleration() {

        InitialVelocity -= timeDeceleration * Time.deltaTime;
        player.transform.Translate(InitialVelocity * player.dashDirection * Time.deltaTime);
    }
    
}
