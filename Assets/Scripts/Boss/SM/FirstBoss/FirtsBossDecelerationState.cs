using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirtsBossDecelerationState : FirstBossState
{
    //Inspector
    public DecelerationData decelerationData;

    //Private
    float timer;
    float finalDeltaTime;
    int iterations;


    public override void Enter()
    {
        iterations = 1;
        base.Enter();
        animator.SetInteger("Layer", 0);
        timer = 0;
        boss.DecelerationModule = decelerationData.Deceleration;
        setMovementDecelerationCurve();


    }
    public override void Tick()
    {
        timer += Time.deltaTime;
        base.Tick();

        //CollisionTick();
        setChaseRadius();
        Deceleration();
        SetCycleTimer();
        SetSpeed();
    }

 

    public override void Exit()
    {
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();

        
    }

    //Set speed parameter in the animator
    public void SetSpeed() {
        animator.SetFloat("Speed", boss.VelocityVector.magnitude);
    }

    public void Deceleration()
    {

        if (timer <= finalDeltaTime)
        {
            boss.Deceleration(decelerationData.MovementDecelerationCurve , timer - Time.deltaTime , timer , iterations);
        }
        else 
        {
            boss.Deceleration(decelerationData.MovementDecelerationCurve, timer - Time.deltaTime, finalDeltaTime, iterations);
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }

    public void setChaseRadius()
    {
        float distance = (boss.Target.transform.position - boss.transform.position).magnitude;
        animator.SetFloat("ChaseRadius", distance);
    }

    void setMovementDecelerationCurve() {

        decelerationData.MovementDecelerationCurve.keys = null;
        finalDeltaTime = boss.VelocityVector.magnitude / boss.DecelerationModule;

        decelerationData.MovementDecelerationCurve.AddKey(0, boss.VelocityVector.magnitude);
        decelerationData.MovementDecelerationCurve.AddKey(finalDeltaTime , 0);

    }

    


}
