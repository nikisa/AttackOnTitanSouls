using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindState : GrappleBaseState
{

    public override void Enter() {
        while (grappleManager.hook.shooted) {
            grappleManager.RewindPoints();
        }
    }

    public override void Tick() {
        if (!grappleManager.hook.shooted) {
            animator.SetTrigger("RolledUp");
        }
    }

}
