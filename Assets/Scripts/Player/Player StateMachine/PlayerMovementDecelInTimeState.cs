using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDecelInTimeState : PlayerBaseState
{
    
    //Inspector
    public PlayerDecelInTimeData playerDecelInTimeData;

    //Private
    PlayerMovementData playerMovementData; 


    public override void Enter() {

        playerMovementData = player.playerMovementData;
        player.DecelerationModule = (playerMovementData.maxSpeed) / (playerDecelInTimeData.DecelerationTime);
    }

    public override void Tick() {

        if (player.VelocityVector.magnitude > player.DecelerationModule * Time.deltaTime) {
            player.Deceleration();
        }
        else {
            player.VelocityVector = Vector3.zero;
            animator.SetTrigger(IDLE);
        }

        if (Mathf.Pow(Input.GetAxis("Horizontal"), 2) + Mathf.Pow(Input.GetAxis("Vertical"), 2) > Mathf.Pow(player.DeadZoneValue, 2)) {
            animator.SetTrigger(MOVEMENT);
        }

    }

    public override void Exit() {
        player.VelocityVector = Vector3.zero;
    }

}
