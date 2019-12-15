using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashFramePerfectState : PlayerBaseState {

    [HideInInspector]
    public PlayerDashData playerDashData;
    public float frames;

    //Private
    float startTime;

    public override void Enter() {
        playerDashData = player.playerDashData;
        startTime = Time.time;
        
    }
    public override void Tick() {
        if ((Time.time - startTime) < frames / 60) {
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

   
}
