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
        player.DeceleratioModule = (playerMovementData.maxSpeed) / (playerDecelInTimeData.DecelerationTime);
        
    }

    public override void Tick() {

        if (player.VelocityVector.magnitude > player.DeceleratioModule * Time.fixedDeltaTime) {
            player.newDeceleration();
        }
        else {
            player.VelocityVector = Vector3.zero;
            animator.SetTrigger(IDLE);
        }

        if (player.newInput) {
            animator.SetTrigger(MOVEMENT);
        }


    }

}
