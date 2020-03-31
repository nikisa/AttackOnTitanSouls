using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : BossController
{
    //Inspector
    public BossOrbitManager bossOrbitManager;   
    public float ActiveMaskCollisionTime;


    //Public
    [HideInInspector]
    public float timerMaskCollision;
    [HideInInspector]
    public float vectorAngle;
    [HideInInspector]
    public FirstBossMask firstBossMask;
    [HideInInspector]
    public ControllerColliderHit hit;
    [HideInInspector]
    public float actualMaxSpeed;
    [HideInInspector]
    public bool hasAwaken = false; //Temporary


    //[HideInInspector]
    public int loops;
    [HideInInspector]
    public bool IsPrevStateReinitialize;

    protected override void Start() {
        base.Start();
        foreach (var item in animator.GetBehaviours<FirstBossState>()) {
            item.SetContext(this, firstBossMask , animator , bossOrbitManager);
        }

    }

    protected override void Update() {
        RotateOnTarget();
    }

    //Bounce del Boss (no maschere)
    private void OnControllerColliderHit(ControllerColliderHit _hit) {

        hit = _hit;
        
        if ((_hit.collider.GetComponent<MovementBase>() || _hit.collider.GetComponent<PlayerView>()) && !_hit.collider.GetComponent<BossController>()) {
            animator.SetInteger("Layer", 11);
            Debug.Log("DDDD");
            Player.GetDamage();
            bossOrbitManager.ObjHit = 2;
        }

        if (_hit.collider.tag == "Walls") {
            animator.SetInteger("Layer", 10);
            bossOrbitManager.ObjHit = 1;
        }
    }

    public void RotateOnTarget() {
        if (hasAwaken) {
            if (bossOrbitManager.MasksList[0].VelocityVector != Vector3.zero)
                Graphics.transform.LookAt(Player.transform.position);
            //else 
                //transform.parent.LookAt(-Player.transform.position);
            
        }
    }

}