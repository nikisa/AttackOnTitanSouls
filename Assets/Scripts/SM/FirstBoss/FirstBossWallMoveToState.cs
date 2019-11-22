using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossWallMoveToState : FirstBossState
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
        SetToCenter();
    }
    public override void Exit()
    {
        
    }
    public void ChargeAttack()
    { 

        targetPosition = new Vector3(boss.Data.wallMoveToInfo.Target.transform.position.x, startY, boss.Data.wallMoveToInfo.Target.transform.position.z);
        boss.RotateTarget(targetPosition);

    }

    public void MoveToEnter() {
        
        startY = boss.transform.position.y;
        ChargeAttack();
        hit = boss.RaycastCollision();
    }

    public void MoveToTick() {
        WallDistance = boss.CollisionDistance(hit.point);
<<<<<<< HEAD
        if (WallDistance <= boss.transform.localScale.x/2) {
=======
>>>>>>> 2c74e38b58270d25b1ac8072e86271849498c9ae

        if (WallDistance <= boss.transform.localScale.x/2) {
            Debug.Log(WallDistance);
            animator.SetTrigger("Collision");
        }
        else {
            boss.Move();
        }
        distance = Vector3.Distance(boss.transform.position, targetPosition);
        if (boss.Data.moveToInfo.StopsAtTargetOvertaking) {
            if (boss.transform.position.x >= boss.Data.wallMoveToInfo.Target.transform.position.x - range && boss.transform.position.x <= boss.Data.wallMoveToInfo.Target.transform.position.x + range
                || boss.transform.position.z >= boss.Data.wallMoveToInfo.Target.transform.position.z - range && boss.transform.position.z <= boss.Data.wallMoveToInfo.Target.transform.position.z + range) {
                animator.SetTrigger(RECOVERY);
            }

        }
        if (distance <= 1 && !boss.Data.wallMoveToInfo.StopsAtTargetOvertaking) {
            animator.SetTrigger(RECOVERY);
        }
        else {
            boss.Move();
        }

        
    }

    public void AccelerationEnter() {
        timeStartAcceleration = Time.time;
    }

    public void AccelerationTick() {
        if (Time.time - timeStartAcceleration > boss.Data.wallAccelerationInfo.WaitOnStart) {
            boss.Acceleration(boss.Data.wallAccelerationInfo.TimeAcceleration, boss.Data.wallAccelerationInfo.MaxSpeed);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation();
    }
    public void SetToCenter()
    {
        if (boss.transform.position.x < -70 ||
            boss.transform.position.x > 70  ||
            boss.transform.position.z < -100 ||
            boss.transform.position.z > 120 )
        {
            boss.transform.position = GameObject.FindGameObjectWithTag("Center").transform.position;
            animator.SetTrigger("Collision");
        }
    }
}
