using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMoveToState : FirstBossState
{
    public AccelerationData accelerationData;
    public MoveToData moveToData;
    public RotationMoveData rotationMoveData;
    public BossController.Targets targets;
    [HideInInspector]
    public GameObject Target;
    //private 
    Vector3 targetPosition;
    float startY;
    int range=1;
    float distance;
    float WallDistance;
    float timeStartAcceleration;
    RaycastHit hit;


    public override void Enter()
    {
        SetTarget();
        MoveToEnter();
        AccelerationEnter();
    }
    public override void Tick()
    {
        MoveToTick();
        AccelerationTick();
        RotationMoveTick();
        SetToCenter();
    }
    public override void Exit()
    {
        
    }
    public void ChargeAttack()
    { 

        targetPosition = new Vector3(Target.transform.position.x, startY, Target.transform.position.z);
        boss.RotateTarget(targetPosition);

    }

    public void MoveToEnter() {
        startY = boss.transform.position.y;
        ChargeAttack();
        hit = boss.RaycastCollision();
    }

    public void MoveToTick() {
        WallDistance = boss.CollisionDistance(hit.point);
        //Debug.Log(WallDistance);
        if (WallDistance <= 2) {
            Debug.Log(WallDistance + "OK__________" );
            animator.SetTrigger("Collision");
        }
        distance = Vector3.Distance(boss.transform.position, targetPosition);
        //newDistance = Vector3.Distance(boss.transform.position, Target.transform.position);
        if (moveToData.StopsAtTargetOvertaking) {
            if (boss.transform.position.x >= Target.transform.position.x - range && boss.transform.position.x <= Target.transform.position.x + range
                || boss.transform.position.z >= Target.transform.position.z - range && boss.transform.position.z <= Target.transform.position.z + range) {
                animator.SetTrigger(RECOVERY);
            }

        }
        if (distance <= 1 && !moveToData.StopsAtTargetOvertaking) {
            animator.SetTrigger(RECOVERY);
        }

        boss.Move();
    }

    public void AccelerationEnter() {
        timeStartAcceleration = Time.time;
    }

    public void AccelerationTick() {
        if (Time.time - timeStartAcceleration > accelerationData.WaitOnStart) {
            boss.Acceleration(accelerationData.TimeAcceleration, moveToData.MaxSpeed);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation(rotationMoveData.MaxSpeed);
    }

    public void SetToCenter() {
        if (boss.transform.position.x < -70 ||
            boss.transform.position.x > 70 ||
            boss.transform.position.z < -100 ||
            boss.transform.position.z > 120) {
            boss.transform.position = GameObject.FindGameObjectWithTag("Center").transform.position;
            animator.SetTrigger(IDLE);
        }
    }
    public void SetTarget()
    {
        Target=boss.SetTarget(targets);
    }
}
