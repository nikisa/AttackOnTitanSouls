using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossChaseState : FirstBossState
{
    //Inspector
    public bool Debugging;
    public ChaseData chaseData;

    //Private 
    int iterations;
    int layerResult;
    int layerWall;
    int layerCollision;
    int layerPlayer;
    float maxSpeed;
    float startY;
    float timeStartAcceleration;
    float timeStartChase;
    float AngularSpeed;
    float deltaAngle;
    float accelerationModule;
    Vector3 targetDir;
    GameObject Target;


    public override void Enter()
    {
        base.Enter();

        iterations = 30;
        layerWall = 10;
        layerPlayer = 11;

        Target = chaseData.Target.instance;
        boss.Target = Target;
        OrbitTag(chaseData);
        AccelerationEnter();
        ChaseEnter();
    }

    public override void Tick()
    {
        base.Tick();

        setChaseRadius();
        ChaseTick();
        SetSpeed();
        SetCycleTimer();
    }

    public override void Exit()
    {

        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
        animator.SetBool("ChaseOrbit", false);
        animator.SetInteger("Layer", layerResult);

    }

    public void AccelerationEnter()
    {
        timeStartAcceleration = Time.time;
        //boss.VelocityVector = Vector3.zero;
        boss.MovementReset();
    }

    public void ChaseEnter()
    {
        timeStartChase = Time.time;
        startY = boss.transform.position.y; //Keep costant the Y axes of the Boss position
        accelerationModule = chaseData.MaxSpeed / chaseData.TimeAcceleration;
    }

    //Chase the target
    public void ChaseTick()
    {
        layerCollision = boss.DetectCollision(boss.nextPosition);
        targetDir = Target.transform.position - boss.transform.position;

        if (layerCollision == layerWall && boss.VelocityVector.magnitude > 5) {
            animator.SetInteger("Layer", layerCollision);
        }
        else {
            boss.Movement(targetDir, chaseData.MaxSpeed, accelerationModule);
        }
    }


    public void setChaseRadius() {
        float distance = (Target.transform.position - boss.transform.position).magnitude;
        animator.SetFloat("ChaseRadius", distance);
    }

    //Set speed parameter in the animator
    public void SetSpeed() {
        animator.SetFloat("Speed" , boss.VelocityVector.magnitude);
    }

    
}
