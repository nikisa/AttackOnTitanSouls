using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirtsBossDecelerationState : FirstBossState
{
    //Inspector
    public DecelerationData decelerationData;
    public bool Debugging;

    //Private
    int wallLayer;

    public override void Enter()
    {

        base.Enter();
        wallLayer = 10;
    }
    public override void Tick()
    {

        base.Tick();

        CollisionTick();
        setChaseRadius();
        Deceleration();
        SetCycleTimer();
    }

    public void CollisionTick()
    {

        Vector3 nextPosition = boss.transform.position + (boss.MoveSpeed * Time.deltaTime) * boss.transform.forward;
        if (boss.DetectCollision(nextPosition) == wallLayer)
        {
            animator.SetTrigger("Collision");
        }
    }


    public override void Exit()
    {
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
    }

    //Set speed parameter in the animator
    public void SetSpeed() {
        animator.SetFloat("Speed", boss.MoveSpeed);
    }

    public void Deceleration()
    {
        boss.DecelerationModule = decelerationData.Deceleration;

        if (boss.VelocityVector.magnitude > boss.DecelerationModule * Time.deltaTime)
        {
            boss.Deceleration();
        }
        else 
        {
            boss.VelocityVector = Vector3.zero;
            animator.SetTrigger(END_STATE_TRIGGER);
        }


        if (Debugging)
        {
            Debug.DrawLine(boss.transform.position, boss.transform.position + boss.AccelerationVector, Color.red, .02f);
            Debug.DrawLine(boss.transform.position, boss.transform.position + boss.VelocityVector, Color.blue, .02f);
            //Debug.DrawLine(transform.position, Player.transform.position, Color.green, .02f);
            //Debug.DrawLine(boss.transform.position, boss.MaxSpeedVector, Color.green, .5f);
        }

    }

    public void setChaseRadius()
    {
        float distance = (boss.Target.transform.position - boss.transform.position).magnitude;
        animator.SetFloat("ChaseRadius", distance);
    }


}
