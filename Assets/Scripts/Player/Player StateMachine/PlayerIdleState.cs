using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{

    //Private
    DataInput dataInput;

    public override void Enter() {
        player.MovementReset();
    }

    public override void Tick() {

        player.CharacterController.Move(player.VelocityVector/10);

        if (player.checkDeadZone()) {
            animator.SetTrigger(MOVEMENT);
        }
    }
}
