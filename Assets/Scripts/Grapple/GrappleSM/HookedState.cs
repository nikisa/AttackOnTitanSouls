using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookedState : GrappleBaseState
{
    public override void Enter() {

    }
               
    public override void Tick() {
        if (grappleManager.hook.isHooked) {
            if (!grappleManager.hookPoint.isHooked) {
                Debug.Log(hitTransform);
                grappleManager.hook.transform.position = hitTransform;
            
                grappleManager.hook.Inertia = Vector3.zero;
                grappleManager.hook.hitDistance = 0;
            }
            grappleManager.hookPoint.isHooked = true;

        }
        else {
            grappleManager.hookPoint.isHooked = false;
            grappleManager.hook.hitDistance = 1;
            animator.SetTrigger("Rewind");

        }
    }

}
