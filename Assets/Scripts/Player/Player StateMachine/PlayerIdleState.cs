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
        if (player.checkDeadZone()) {
            animator.SetTrigger(MOVEMENT);
        }
    }
}
