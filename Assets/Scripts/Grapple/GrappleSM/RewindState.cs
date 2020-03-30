using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindState : GrappleBaseState
{

    public override void Enter() {
        
    }

    public override void Tick() {

        grappleManager.UpdatePoints();
        grappleManager.UpdateHook();
        grappleManager.UpdateLinks();

        if (grappleManager.hook.shooted) {
            if (!grappleManager.hook.isHooked) {
                grappleManager.RewindPoints();
            }
            else {
                if (Input.GetAxis("Rewind") >= 0.5f) {
                    grappleManager.RewindPoints();
                }
                else {
                    Debug.Log("STOP REWINDING");
                    animator.SetTrigger("Hooked");
                }
            }
        }        

        if (!grappleManager.hook.shooted) {
            animator.SetTrigger("RolledUp");
        }
    }

}
