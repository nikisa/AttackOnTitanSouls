using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleBaseState : BaseState
{


    protected GrappleManager grappleManager;


    protected RaycastHit hit;
    protected Vector3 hitTransform;

    public void SetContext(object context, Animator animator , GrappleManager grappleManager , Hook hook) {
        grappleManager.Player = context as PlayerController;
        this.animator = animator;
        this.grappleManager = grappleManager;
        grappleManager.hook = hook;
    }
}
