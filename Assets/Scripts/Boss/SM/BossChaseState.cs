using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BossBaseState
{
    // data
    public AccelerationData accelerationData;
    public ChaseData chaseData;


    public BossController.Targets targets;
    [HideInInspector]
    public GameObject Target;
    // private 
    float startY;
    float timeStartAcceleration;
    float timeStartChase;
    //float AngularSpeed;
    public override void Enter()
    {
        SetTarget();
        AccelerationEnter();
        ChaseEnter();
    }
    public override void Tick()
    {
        AccelerationTick();
        ChaseTick();
        boss.Move();
        //AngularSpeed = boss.MoveSpeed / 5;
    }
    public override void Exit()
    {
      
    }
    public void AccelerationEnter()
    {
        timeStartAcceleration = Time.time;
    }
    public void ChaseEnter()
    {
        //chaseData.AngularSpeed = chaseData.MaxSpeed /accelerationData.TimeAcceleration / 10;
        float timeStartChase = Time.time;
        startY = boss.transform.position.y;
    }
    public void ChaseTick()
    {
        if (Time.time - timeStartChase < chaseData.TimeChase)
        {
            if (chaseData.HasAngularSpeed)
            {
                boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, Quaternion.LookRotation(Target.transform.position - boss.transform.position), chaseData.AngularSpeed * Time.deltaTime);
               
            }
            else
            {
                boss.RotateTarget(Target.transform.position);
            }

        }
        else
        {
            animator.SetTrigger(IDLE);
        }
    }
    public void AccelerationTick()
    {

        if (Time.time - timeStartAcceleration > accelerationData.WaitOnStart)
        {
            if (accelerationData.HasAcceleration)
            {
                boss.Acceleration(accelerationData.TimeAcceleration, chaseData.MaxSpeed);
            }
            else
            {
                boss.Acceleration(1, chaseData.MaxSpeed);
            }
           
        }
    }
    public void SetTarget()
    {
        Target = boss.SetTarget(targets);
    }
}
