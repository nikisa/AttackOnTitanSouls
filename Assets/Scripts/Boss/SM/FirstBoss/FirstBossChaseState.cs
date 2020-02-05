using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossChaseState : FirstBossState
{
    //Inspector
    public ChaseData chaseData;

    //Private 
    float maxSpeed;
    float startY;
    float timeStartAcceleration;
    float timeStartChase;
    float AngularSpeed;
    float deltaAngle;
    GameObject Target;


    public override void Enter()
    {
        Target = chaseData.Target.instance;
        OrbitTag(chaseData);
        AccelerationEnter();
        ChaseEnter();
    }

    public override void Tick()
    {
        base.Enter();
        setChaseRadius();
        Timer(chaseData);
        AccelerationTick();
        ChaseTick();
        //boss.Move();
        SetSpeed();
    }

    public override void Exit()
    {
        ResetTimer(chaseData);
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
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
        //deltaAngle = Vector3.Angle(boss.transform.position, Target.transform.position);
        //AngularSpeed = deltaAngle / chaseData.VectorRotationRate;
        //    if (chaseData.HasVectorRotationRate)
        //    {
        //        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, Quaternion.LookRotation(Target.transform.position - boss.transform.position), AngularSpeed * Time.deltaTime);
        //    }
        //    else
        //    {
        //        boss.RotateTarget(Target.transform.position);
        //    }
        //_______________________________________________________________________________________________________________________________________________________________________________________________

        // MaxSpeed / Acceleration = 1/DD
        // Acceleration = MaxSpeed / AccelerationTime
        // MaxSpeed / (MaxSpeed / AccelerationTime) = 1/DD
        // DD = 1/AccelerationTime

        boss.MoveSpeed = (chaseData.MaxSpeed / chaseData.TimeAcceleration * Time.deltaTime);
        chaseData.DynamicDrag = (chaseData.MaxSpeed - boss.MoveSpeed) / chaseData.MaxSpeed;
        boss.vectorAngle = Target.transform.position - boss.transform.position;
        boss.OldPos = boss.transform.position;
        boss.transform.position = boss.transform.position + boss.Inertia + boss.MoveSpeed * boss.vectorAngle.normalized * Time.deltaTime;
        boss.Inertia = (boss.transform.position - boss.OldPos) * (chaseData.DynamicDrag);
    }

    //Does an acceleration when starts chasing the target
    public void AccelerationTick()
    {
        boss.Acceleration(chaseData.TimeAcceleration, chaseData.MaxSpeed);         
    }

    public void setChaseRadius() {
        float distance = (Target.transform.position - boss.transform.position).magnitude;
        animator.SetFloat("ChaseRadius", distance);
    }

    //Set speed parameter in the animator
    public void SetSpeed() {
        animator.SetFloat("Speed" , boss.MoveSpeed);
    }
}
