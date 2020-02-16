using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstBossMoveToState : FirstBossState
{
    //Inspector
    public MoveToData moveToData;

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


    

    public override void Enter()
    {

        base.Enter();

        iterations = 30;
        layerWall = 10;
        layerPlayer = 11;
        reinitSphereCastTimer = 0.05f;

        Target = moveToData.Target.instance;
        OrbitTag(moveToData);
        MoveToEnter();
        AccelerationEnter();

    }
    public override void Tick()
    {
        Debug.DrawRay(boss.transform.position + new Vector3(0, 6, 0), boss.AccelerationVector * 10, Color.blue, .1f);
        Timer(moveToData);
        MoveToTick();
        AccelerationTick();
        SetSpeed();
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

            boss.Move();

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
