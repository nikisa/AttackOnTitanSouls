using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootedState : GrappleBaseState
{
    public override void Enter() {
        
    }

    public override void Tick() {

        grappleManager.UpdatePoints();
        grappleManager.UpdateHook();
        grappleManager.UpdateLinks();

        if (grappleManager.hook.shooted && !grappleManager.hook.isHooked) {
            grappleManager.target = grappleManager.FovPlayer.CorrectRayCast();
            target = grappleManager.target;

            //HookPoint hookPoint = hit.transform.GetComponent<HookPoint>();

            if (target != null) {
                if (target.transform.GetComponent<FirstBossMask>()) {
                    grappleManager.hookPoint = target.transform.GetComponent<FirstBossMask>();
                    grappleManager.hook.isHooked = true;
                    // SetHit();
                    // Debug.Log(hitTransform + "QUIIIIII");
                    animator.SetTrigger("Hooked");
                    return;
                }
                else {
                    //UpdateHook();
                    //Debug.Log("Missing Target");
                }
            }
        }


        if (grappleManager.hook.isHooked) {
            animator.SetTrigger("Hooked");
        }
        else if (!grappleManager.hook.isHooked && grappleManager.hook.ropeFinished) {
            animator.SetTrigger("Rewind");

        }
        
    }
    //public void SetHit()
    //{
    //    if (!grappleManager.IsSet)
    //    {
    //        //Debug.Log("UNOOO");
    //        hitTransform = grappleManager.hookPoint.GetComponent<Collider>().ClosestPoint(grappleManager.hook.transform.position);
    //        grappleManager.IsSet = true;
    //    }
    //}
}
