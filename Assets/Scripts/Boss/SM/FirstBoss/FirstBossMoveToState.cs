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

    int layerWall;
    int layerPlayer;
    float accelerationModule;
    Vector3 targetDir;


    public override void Enter()
    {
        base.Enter();

        iterations = 30;
        layerWall = 10;
        layerPlayer = 11;
        
        Target = moveToData.Target.instance;
        boss.Target = Target;
        accelerationModule = moveToData.MaxSpeed / moveToData.TimeAcceleration;

        boss.MovementReset();
        OrbitTag(moveToData);
        MoveToEnter();

    }
    public override void Tick()
    {
        base.Tick();

        Debug.DrawRay(boss.transform.position + new Vector3(0, 6, 0), boss.AccelerationVector * 10, Color.blue, .1f);
        MoveToTick();
        SetSpeed();
        SetCycleTimer();
    }


    public override void Exit()
    {
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
        animator.SetBool("MoveToOrbit", false);
        animator.SetInteger("Layer", 0);
    }


    //Set direction and position of the Target
    public void ChargeAttack()
    { 
        targetPosition = new Vector3(Target.transform.position.x, startY, Target.transform.position.z);
        targetDir = targetPosition - boss.transform.position;
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

        //layerResult = boss.MovingDetectPlayer(iterations);

        layerResult = boss.DetectCollision(boss.nextPosition);
        Debug.Log("layer: " + layerResult);

        if (layerResult == layerPlayer) {
            if (!boss.Player.IsImmortal) {
                PlayerController.DmgEvent();
            }

        }

        if (layerResult == layerWall && boss.VelocityVector.magnitude > 20)
        {
            animator.SetInteger("Layer", layerResult);
        }
        else {

            boss.Movement(targetDir , moveToData.MaxSpeed , accelerationModule);
        }
    }

    //Set speed parameter in the animator
    public void SetSpeed() {
        animator.SetFloat("Speed", boss.MoveSpeed);
    }

}
