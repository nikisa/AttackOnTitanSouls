using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstBossMoveToState : FirstBossState
{
    //Inspector
    public MoveToData moveToData;
    public bool Debugging;


    //Private 
    GameObject Target;
    Vector3 targetPosition;
    float startY;
    float timeStartAcceleration;
    float timeStartTrail;
    float timeStartMoveTo;
    float timeMoveTo;
    int iterations;
    int layerResult;
    int layerCollision;

    int layerWall;
    int layerPlayer;
    float reinitSphereCastTimer;
    Vector3 targetDir;


    

    public override void Enter()
    {

        base.Enter();

        iterations = 30;
        layerWall = 10;
        layerPlayer = 11;
        reinitSphereCastTimer = 0.05f;

        
        Target = moveToData.Target.instance;
        boss.Target = Target;
        OrbitTag(moveToData);
        MoveToEnter();
        targetDir = targetPosition - boss.transform.position;
        AccelerationEnter();



    }
    public override void Tick()
    {
        Debug.DrawRay(boss.transform.position + new Vector3(0, 6, 0), boss.AccelerationVector * 10, Color.blue, .1f);
        Timer(moveToData);
        MoveToTick();
        AccelerationTick();
        SetSpeed();
        SetCycleTimer();
    }


    public override void Exit()
    {
        ResetTimer(moveToData);
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
        animator.SetBool("MoveToOrbit", false);
        animator.SetInteger("Layer", 0);
    }


    //Set direction and position of the Target
    public void ChargeAttack()
    { 
        targetPosition = new Vector3(Target.transform.position.x, startY, Target.transform.position.z);
        boss.AccelerationVector = targetPosition - boss.transform.position;
        boss.RotateTarget(targetPosition);
    }

    public void MoveToEnter() {
        boss.moveToData = moveToData;
        boss.MaxSpeed = moveToData.MaxSpeed;
        boss.MoveSpeed += moveToData.AddToVelocity;
        startY = boss.transform.position.y;
        ChargeAttack();
        timeStartMoveTo = Time.time;
    }


    public void MoveToTick() {

        layerResult = boss.MovingDetectPlayer(iterations);

        Vector3 nextPosition = boss.transform.position + (boss.MoveSpeed * Time.deltaTime) * boss.transform.forward;
        layerCollision = boss.MovingDetectCollision(iterations, nextPosition, boss.MoveSpeed);

        if (layerCollision == layerWall)
        {
            animator.SetInteger("Layer", layerCollision);
        }
        else {

            //boss.Move();

            //__________________________

            
            boss.vectorAngle = Vector3.SignedAngle(Vector3.forward, targetDir, Vector3.up) * Mathf.Deg2Rad;
            boss.AccelerationModule = moveToData.MaxSpeed / moveToData.TimeAcceleration;
            boss.AccelerationVector = new Vector3(Mathf.Sin(boss.vectorAngle) * boss.AccelerationModule, 0, Mathf.Cos(boss.vectorAngle) * boss.AccelerationModule);
            //boss.MaxSpeedVector = new Vector3(Mathf.Cos(boss.vectorAngle) * chaseData.MaxSpeed, boss.AccelerationVector.y, Mathf.Sin(boss.vectorAngle) * chaseData.MaxSpeed);
            boss.Drag = boss.AccelerationModule / moveToData.MaxSpeed * Time.deltaTime;
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



            //__________________________

            if (layerResult == layerPlayer) {
                if (!boss.Player.IsImmortal)
                {
                    PlayerController.DmgEvent();
                }
                
            }
        }
    }

    public void AccelerationEnter() {
        timeStartAcceleration = Time.time;
    }

    public void AccelerationTick() {
        boss.Acceleration(moveToData.TimeAcceleration, moveToData.MaxSpeed);
    }

    //Set speed parameter in the animator
    public void SetSpeed() {
        animator.SetFloat("Speed", boss.MoveSpeed);
    }

}
