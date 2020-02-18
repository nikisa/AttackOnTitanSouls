using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDecelerationState : PlayerBaseState
{

    public PlayerDecelerationData playerDecelerationData;

    //Public 
    [HideInInspector]
    public PlayerMovementData playerMovementData;
    [HideInInspector]
    public PlayerDashData playerDashData;

    //Private
    float decelRatePerSec;

    public override void Enter() {

        playerMovementData = player.playerMovementData;
        playerDashData = player.playerDashData;

        //decelRatePerSec = -playerMovementData.maxSpeed / (playerMovementData.TimeDeceleration);
        //player.DeceleratioModule = decelerationData.Deceleration;
    }

    public override void Tick() {
        player.PlayerInclination();
        player.Deceleration(decelRatePerSec);

        if (player.newInput || player.movementVelocity == Vector3.zero) {
            animator.SetTrigger(IDLE);
        }

    }

}
