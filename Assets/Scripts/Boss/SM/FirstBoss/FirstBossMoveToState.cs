using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstBossMoveToState : FirstBossState
{
    //Inspector
    public MoveToData moveToData;
    public RotationMoveData rotationMoveData;
    //public BossController.Targets targets;

    //Public
    //[HideInInspector]
    ////public GameObject Target;

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
        RotationEnter();
        MoveToEnter();
        AccelerationEnter();

    }
    public override void Tick()
    {
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
        boss.RotateTarget(targetPosition);
    }

    public void MoveToEnter() {
        boss.moveToData = moveToData;
        boss.MaxSpeed = moveToData.MaxSpeed;
        boss.MoveSpeed += moveToData.AddToVelocity;
        startY = boss.transform.position.y;
        timeMoveTo = moveToData.Time - moveToData.TimeDeceleration;
        ChargeAttack();
        timeStartMoveTo = Time.time;
    }

    public void RotationEnter()
    {
        boss.RotationSpeed += rotationMoveData.AddToRotationSpeed;
    }

    public void MoveToTick() {

        layerResult = boss.MovingDetectCollision(iterations);

        if (layerResult == layerWall && Time.time - timeStartMoveTo > reinitSphereCastTimer)
        {
            animator.SetInteger("Layer", layerResult);
        }
        else {

            boss.Move();

            if (layerResult == layerPlayer) {
                PlayerController.DeathEvent();
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
