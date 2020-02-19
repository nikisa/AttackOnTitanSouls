using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{

    //Private
    DataInput dataInput;

    public override void Enter() {
        player.VelocityVector = Vector3.zero;
    }

    public override void Tick() {
        if (Mathf.Pow(Input.GetAxis("Horizontal"), 2) + Mathf.Pow(Input.GetAxis("Vertical"), 2) >= Mathf.Pow(player.DeadZoneValue, 2)) {
            animator.SetTrigger(MOVEMENT);
        }
    }
}
