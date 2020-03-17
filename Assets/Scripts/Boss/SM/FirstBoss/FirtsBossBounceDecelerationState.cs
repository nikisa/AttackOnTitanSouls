using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirtsBossBounceDecelerationState : FirstBossState
{
    //Inspector
    public DecelerationData decelerationData;
    public BounceDecelerationData bounceDecelerationData;

    //Private
    int wallLayer;

    public override void Enter() {

        base.Enter();
        wallLayer = 10;
    }
    public override void Tick() {

        base.Tick();

        CollisionTick();
        setChaseRadius();
        Deceleration();
        BounceDeceleration();
        SetCycleTimer();
    }

    public void CollisionTick() {

        Vector3 nextPosition = boss.transform.position + (boss.MoveSpeed * Time.deltaTime) * boss.transform.forward;

        if (boss.DetectCollision(nextPosition) == wallLayer) {
            animator.SetTrigger("Collision");
        }

    }


    public override void Exit() {
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
    }

    //Set speed parameter in the animator
    public void SetSpeed() {
        animator.SetFloat("Speed", boss.MoveSpeed);
    }

    public void Deceleration() {
        boss.DecelerationModule = decelerationData.Deceleration;

        if (boss.VelocityVector.magnitude > boss.DecelerationModule * Time.deltaTime) {
            boss.Deceleration();

        }
        else {
            boss.VelocityVector = Vector3.zero;
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }

    public void BounceDeceleration() {
        boss.DecelerationModule = bounceDecelerationData.ImpulseDeceleration;
        
        if (boss.BounceVector.magnitude > boss.DecelerationModule * Time.deltaTime) {
            Debug.Log("BV: " + boss.BounceVector.magnitude);
            boss.BounceDeceleration();
        }
        else {
            boss.BounceVector = Vector3.zero;
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }

    public void setChaseRadius() {
        float distance = (boss.Target.transform.position - boss.transform.position).magnitude;
        animator.SetFloat("ChaseRadius", distance);
    }


}
