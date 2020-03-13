using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindState : GrappleBaseState
{

    public override void Enter() {
        
    }

    public override void Tick() {

        while (grappleManager.hook.shooted) {
            grappleManager.RewindPoints();
        }

        if (!grappleManager.hook.shooted) {
            animator.SetTrigger("RolledUp");
        }
    }

}
