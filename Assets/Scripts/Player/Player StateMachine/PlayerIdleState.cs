using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    // 
    public PlayerDashData playerDashData;

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
        if (Time.time - player.timerDash > playerDashData.EnableDashAt)
        {
            player.canDash = true;
        }

        if (Input.GetButtonDown("Dash") && player.canDash)
        {
            player.canDash = false;
            player.targetDir = player.body.transform.forward;
            animator.SetTrigger(DASH);
        }
    }

    public override void Exit() {
        idleCollider.enabled = false;
    }
}
