using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void Enter() {
        
    }

    public override void Tick() {
        if (player.newInput) {
            animator.SetTrigger(MOVEMENT);
        }
    }
}
