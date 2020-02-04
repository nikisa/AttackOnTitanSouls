using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolledUpState : GrappleBaseState
{

    public override void Enter() {
        
    }

    public override void Tick() {
        if (Input.GetKeyDown(KeyCode.Mouse0) /*|| Input.GetButtonDown("ShootPS4")*/ || Input.GetButtonDown("ShootXBOX")){
            grappleManager.HookShooting();
            if (!grappleManager.hook.shooted) {
                grappleManager.InstantiateRope();
                animator.SetTrigger("Shooted");
            }  
        }
    }
}
