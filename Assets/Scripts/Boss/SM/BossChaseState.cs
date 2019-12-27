using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : FirstBossState
{
    // data
    //public AccelerationData accelerationData;
    public ChaseData chaseData;


    public BossController.Targets targets;
    [HideInInspector]
    public GameObject Target;

    // private 
    float startY;
    float timeStartAcceleration;
    float timeStartChase;
    float AngularSpeed;
    float deltaAngle;


    public override void Enter()
    {

        if (/*animator.GetCurrentAnimatorStateInfo(0).IsName("Chase")*/ animator.GetCurrentAnimatorStateInfo(0).IsTag("0")) {
            Debug.Log("TAG");
            animator.SetBool("ChaseOrbit", true);
        }

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
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
        Debug.Log(bossOrbitManager.HookPointList.Count);
        animator.SetBool("ChaseOrbit", false);
    }
    public void AccelerationEnter()
    {
        timeStartAcceleration = Time.time;
    }
    public void ChaseEnter()
    {
     
        //chaseData.AngularSpeed = chaseData.MaxSpeed /accelerationData.TimeAcceleration / 10;
        //if (chaseData.HasTimer)
        //{
        
            timeStartChase = Time.time;
        //}
        //else
        //{
        //    timeStartChase = Mathf.Infinity;
        //}
        
        startY = boss.transform.position.y;
    }
    public void ChaseTick()
    {
        deltaAngle = Vector3.Angle(boss.transform.position, Target.transform.position);
        AngularSpeed = deltaAngle / chaseData.VectorRotationRate;
        if (Time.time - timeStartChase < chaseData.Time)
        {
            if (chaseData.HasVectorRotationRate)
            {
                boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, Quaternion.LookRotation(Target.transform.position - boss.transform.position), AngularSpeed * Time.deltaTime);
               
            }
            else
            {
                boss.RotateTarget(Target.transform.position);
            }

        }
        else
        {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
        if ((Target.transform.position-boss.transform.position).magnitude > chaseData.ChaseRadius)
        {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }
    public void AccelerationTick()
    {

         if (chaseData.HasAcceleration)
            {
                boss.Acceleration(chaseData.TimeAcceleration, chaseData.MaxSpeed);
            }
            else
            {
                boss.Acceleration(1, chaseData.MaxSpeed);
            }
           
        }
    
    public void SetTarget()
    {
        Target = boss.SetTarget(targets);
    }

    

}
