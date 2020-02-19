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
    Vector3 targetDir;
    GameObject Target;


    public override void Enter()
    {
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
        base.Enter();
        setChaseRadius();
        Timer(chaseData);
        AccelerationTick();
        ChaseTick();
        //boss.Move();
        SetSpeed();
        SetCycleTimer();
    }

    public override void Exit()
    {
        ResetTimer(chaseData);
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
        animator.SetBool("ChaseOrbit", false);

        layerResult = 0;
        animator.SetInteger("Layer", layerResult);

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

        //boss.AccelerationVector = boss.OldPos - boss.transform.position;
        //boss.MoveSpeed = (chaseData.MaxSpeed / chaseData.TimeAcceleration * Time.deltaTime);
        //chaseData.DynamicDrag = (chaseData.MaxSpeed - boss.MoveSpeed) / chaseData.MaxSpeed;
        //boss.vectorAngle = Target.transform.position - boss.transform.position;
        //boss.OldPos = boss.transform.position;
        //boss.transform.position = boss.transform.position + boss.Inertia + boss.MoveSpeed * boss.vectorAngle.normalized * Time.deltaTime;
        //boss.Inertia = (boss.transform.position - boss.OldPos) * (chaseData.DynamicDrag);

        targetDir = Target.transform.position - boss.transform.position;
        boss.vectorAngle = Vector3.SignedAngle(Vector3.forward, targetDir, Vector3.up) * Mathf.Deg2Rad;
        boss.AccelerationModule = chaseData.MaxSpeed / chaseData.TimeAcceleration;
        boss.AccelerationVector = new Vector3(Mathf.Sin(boss.vectorAngle) * boss.AccelerationModule, 0, Mathf.Cos(boss.vectorAngle) * boss.AccelerationModule);
        //boss.MaxSpeedVector = new Vector3(Mathf.Cos(boss.vectorAngle) * chaseData.MaxSpeed, boss.AccelerationVector.y, Mathf.Sin(boss.vectorAngle) * chaseData.MaxSpeed);
        boss.Drag = boss.AccelerationModule / chaseData.MaxSpeed * Time.deltaTime;
        //boss.OldPos = boss.transform.position;
        boss.transform.localPosition += boss.VelocityVector * Time.deltaTime + 0.5f * boss.AccelerationVector * Mathf.Pow(Time.deltaTime, 2);
        boss.VelocityVector += boss.AccelerationVector * Time.deltaTime;
        boss.VelocityVector -= boss.VelocityVector * boss.Drag;


        if (Debugging) {
            Debug.DrawLine(boss.transform.position, boss.transform.position + boss.AccelerationVector, Color.red, .02f);
            Debug.DrawLine(boss.transform.position, boss.transform.position + boss.VelocityVector, Color.blue, .02f);
            //Debug.DrawLine(transform.position, Player.transform.position, Color.green, .02f);
            //Debug.DrawLine(boss.transform.position, boss.MaxSpeedVector, Color.green, .5f);
        }


        //if ((boss.VelocityVector * Time.deltaTime + 0.5f * boss.AccelerationVector * Mathf.Pow(Time.deltaTime, 2)).magnitude <= (boss.MaxSpeedVector * Time.deltaTime).magnitude) {

        //}

        //float MoveSpeed = ((chaseData.MaxSpeed / chaseData.TimeAcceleration) * Time.deltaTime);
        //MoveSpeed += boss.VelocityVector.magnitude;

        /*boss.transform.position + boss.VelocityVector.normalized / MoveSpeed*/
        Vector3 nextPosition = boss.transform.localPosition + (boss.VelocityVector * Time.deltaTime + 0.5f * boss.AccelerationVector * Mathf.Pow(Time.deltaTime, 2));
        layerResult = boss.MovingDetectPlayer(iterations);
        layerCollision = boss.MovingDetectCollision(iterations, nextPosition, boss.MoveSpeed);

        if (layerCollision == layerWall) {
            animator.SetInteger("Layer", layerCollision);
            Debug.Log("BOUNCING");
        }
        else {
            if (layerResult == layerPlayer) {
                if (!boss.Player.IsImmortal) {
                    PlayerController.DmgEvent();
                }
            }
        }
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
