using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookedState : GrappleBaseState
{
    public override void Enter() {
        grappleManager.hook.transform.position = grappleManager.hookPoint.transform.position;
    }
               
    public override void Tick() {

        if (grappleManager.hook.isHooked) {
            grappleManager.UpdatePoints();
            grappleManager.UpdateHook();
            grappleManager.UpdateLinks();
            grappleManager.hookPoint.isHooked = true;
            grappleManager.hookPoint.Inertia = Vector3.zero;
            grappleManager.hook.Inertia = Vector3.zero;
            

            if (Input.GetButtonDown("UnhookXBOX")) {
                grappleManager.hookPoint.isHooked = false;
                grappleManager.hook.isHooked = false;
                grappleManager.hookPoint.transform.position = grappleManager.hookPoint.HookPointPivot.transform.position;
                grappleManager.hook.hitDistance = 1;
                grappleManager.hook.Inertia = Vector3.zero;
            }

            if (!grappleManager.hookPoint.isHooked) {
                grappleManager.hook.Inertia = Vector3.zero;
                grappleManager.hook.hitDistance = 0;
                animator.SetTrigger("Rewind");
            }

            if (Input.GetAxis("Rewind") >= 0.5f) {
                Debug.Log("REWINDING");
                animator.SetTrigger("Rewind");
            }

        }
        else {
            //grappleManager.hookPoint.isHooked = false;
            grappleManager.hook.hitDistance = 1;
            grappleManager.hook.Inertia = Vector3.zero;
            animator.SetTrigger("Rewind");

        }
    }

}
