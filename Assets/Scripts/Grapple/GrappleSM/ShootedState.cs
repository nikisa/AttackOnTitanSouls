﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootedState : GrappleBaseState
{
    public override void Enter() {
        
    }

    public override void Tick() {
        if (!Input.GetKeyDown(KeyCode.Mouse1) || !Input.GetButtonDown("ShootXBOX")) {
            grappleManager.UpdatePoints();
            grappleManager.UpdateHook();
            grappleManager.UpdateLinks();
        }

        if (grappleManager.hook.shooted && !grappleManager.hook.isHooked) {
            hit = grappleManager.hook.RaycastCollsion();

            if (hit.transform != null && hit.transform.GetComponent<HookPoint>()) {
                grappleManager.hookPoint = hit.transform.GetComponent<HookPoint>();
                grappleManager.hook.isHooked = true;
            }
            else {
                //UpdateHook();
                //Debug.Log("Missing Target");
            }
        }


        if (grappleManager.hook.isHooked) {
            animator.SetTrigger("Hooked");
        }
        else if (!grappleManager.hook.isHooked && grappleManager.hook.ropeFinished) {
            animator.SetTrigger("Rewind");
        }
    }
}