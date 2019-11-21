using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMoveToState : FirstBossState
{
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
        MoveToEnter();
        AccelerationEnter();
    }
    public override void Tick()
    {
        MoveToTick();
        AccelerationTick();
        RotationMoveTick();
    }
    public override void Exit()
    {
        
    }
    public void ChargeAttack()
    { 

        targetPosition = new Vector3(boss.Data.moveToInfo.Target.transform.position.x, startY, boss.Data.moveToInfo.Target.transform.position.z);
        boss.RotateTarget(targetPosition);

    }

    public void MoveToEnter() {
        startY = boss.transform.position.y;
        ChargeAttack();
        hit = boss.RaycastCollision();
    }

    public void MoveToTick() {
        WallDistance = boss.CollisionDistance(hit.point);
        if (WallDistance <= 2) {
            Debug.Log("ROSSO");
            animator.SetTrigger("Collision");
        }
        distance = Vector3.Distance(boss.transform.position, targetPosition);
        //newDistance = Vector3.Distance(boss.transform.position, Target.transform.position);
        if (boss.Data.moveToInfo.StopsAtTargetOvertaking) {
            if (boss.transform.position.x >= boss.Data.moveToInfo.Target.transform.position.x - range && boss.transform.position.x <= boss.Data.moveToInfo.Target.transform.position.x + range
                || boss.transform.position.z >= boss.Data.moveToInfo.Target.transform.position.z - range && boss.transform.position.z <= boss.Data.moveToInfo.Target.transform.position.z + range) {
                animator.SetTrigger(RECOVERY);
            }

        }
        if (distance <= 1 && !boss.Data.moveToInfo.StopsAtTargetOvertaking) {
            animator.SetTrigger(RECOVERY);
        }

        boss.Move();
    }

    public void AccelerationEnter() {
        timeStartAcceleration = Time.time;
    }

    public void AccelerationTick() {
        if (Time.time - timeStartAcceleration > boss.Data.accelerationInfo.WaitOnStart) {
            boss.Acceleration(boss.Data.accelerationInfo.TimeAcceleration, boss.Data.accelerationInfo.MaxSpeed);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation();
    }
}
