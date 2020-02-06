using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDecelerationState : PlayerBaseState
{

    //Public 
    [HideInInspector]
    public PlayerIdleData playerIdleData;
    [HideInInspector]
    public PlayerDashData playerDashData;

    //Private
    float decelRatePerSec;

    public override void Enter() {

        playerIdleData = player.playerIdleData;
        playerDashData = player.playerDashData;

        decelRatePerSec = -playerIdleData.maxSpeed / (playerIdleData.framesMaxToZero / 60);

    }

    public override void Tick() {

        player.Deceleration(decelRatePerSec);

        if (player.newInput || player.movementVelocity == Vector3.zero) {
            animator.SetTrigger(IDLE);
        }

    }

}
