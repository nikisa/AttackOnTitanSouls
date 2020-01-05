using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossChaseState : FirstBossState
{
    //Inspector
    public ChaseData chaseData;
    public BossController.Targets targets;

    //Private 
    float startY;
    float timeStartAcceleration;
    float timeStartChase;
    float AngularSpeed;
    float deltaAngle;
    public GameObject Target;


    public override void Enter()
    {
        Target = chaseData.Target.instance;
        OrbitTag(chaseData);
        AccelerationEnter();
        ChaseEnter();
    }

    public override void Tick()
    {
        AccelerationTick();
        ChaseTick();
        boss.Move();
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
        timeStartChase = Time.time;
        startY = boss.transform.position.y; //Keep costant the Y axes of the Boss position
    }

    //Chase the target
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

    //Does an acceleration when starts chasing the target
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
}
