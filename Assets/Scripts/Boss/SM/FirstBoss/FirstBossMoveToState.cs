﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstBossMoveToState : FirstBossState
{
    //Inspector
    public MoveToData moveToData;
    public RotationMoveData rotationMoveData;
    public BossController.Targets targets;

    //Public
    [HideInInspector]
    public GameObject Target;

    //Private 
    Vector3 targetPosition;
    float startY;
    float timeStartAcceleration;
    float timeStartTrail;
    float timeStartMoveTo;
    float timeMoveTo;
    int iterations;

    public override void Enter()
    {
        OrbitTag(moveToData);
        iterations = 30;
        RotationEnter();
        SetTarget();
        MoveToEnter();
        AccelerationEnter();

    }
    public override void Tick()
    {
        MoveToTick();
        AccelerationTick();
    }


    public override void Exit()
    {
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
        animator.SetBool("MoveToOrbit", false);
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

    // TO REFACTOR
    public void MoveToTick() {
   
        if (boss.MovingDetectCollision(iterations) == 1 && Time.time - timeStartMoveTo > .5f) // BUG HERE!!! Probabilmente causato dal Timer
        {
            Debug.Log("collisione");
              animator.SetTrigger("Collision");
        }
        else {

            boss.Move();

            if (boss.MovingDetectCollision(iterations) == 2) {
                SceneManager.LoadScene(2);
            }
        }
     

        if (Time.time - timeStartMoveTo > timeMoveTo)
        {
            animator.SetTrigger(DECELERATION);
        }
    }

    public void AccelerationEnter() {
        timeStartAcceleration = Time.time;
    }

    public void AccelerationTick() {
        boss.Acceleration(moveToData.TimeAcceleration, moveToData.MaxSpeed);
    }


    public void SetTarget()
    {
        Target=boss.SetTarget(targets);
    }

}
