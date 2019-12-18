using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResumeDashState : PlayerBaseState
{
    //Private
    float timeStart;

    public override void Enter() {
        timeStart = Time.time;
    }

    public override void Tick() {
        if (Time.time - timeStart > player.playerDashData.ResumeControl) {
            animator.SetTrigger(DASH_FRAME_PERFECT);
        }
        if (player.dataInput.Dash) {
            animator.SetTrigger(IDLE);
        }
    }

    public override void Exit() {

    }

}
