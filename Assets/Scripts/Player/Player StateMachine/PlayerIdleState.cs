using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{

    //Private
    DataInput dataInput;
    Collider idleCollider;

    public override void Enter() {
        idleCollider = player.body.GetComponent<Collider>();
        idleCollider.enabled = true;
        player.MovementReset();

    }

    public override void Tick() {

        player.CharacterController.Move(player.VelocityVector);

        if (player.checkDeadZone()) {
            animator.SetTrigger(MOVEMENT);
        }
    }

    public override void Exit() {
        idleCollider.enabled = false;
    }
}
