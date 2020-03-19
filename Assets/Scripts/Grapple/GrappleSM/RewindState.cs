using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindState : GrappleBaseState
{

    public override void Enter() {
        
    }

    public override void Tick() {

        if(grappleManager.hook.shooted)
            grappleManager.RewindPoints();

        if (Input.GetAxis("Rewind") < 0.9f) {
            Debug.Log("STOP REWINDING");
            animator.SetTrigger("Hooked");
        }


        if (!grappleManager.hook.shooted) {
            animator.SetTrigger("RolledUp");
        }
    }

}
