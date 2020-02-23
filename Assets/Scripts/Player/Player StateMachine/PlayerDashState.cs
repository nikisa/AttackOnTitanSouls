using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{

    //Inspector
    public PlayerDashData playerDashData;

    //Private
    float timeStart;

    public override void Enter() {

        player.targetDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        timeStart = Time.time;

        player.SetDashVelocity(playerDashData.ActiveDashDistance, playerDashData.ActiveDashTime);

    }

    public override void Tick() {

        if (Time.time - timeStart <  playerDashData.ActiveDashTime) {
            player.Dash(player.dashVelocityModule , player.targetDir);
        }
        else {
            animator.SetTrigger(DASH_DECELERATION);
        }
        
    }

    public override void Exit() {
        
    }

}
