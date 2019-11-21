using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FirstBossWallRecoveryState : FirstBossState
{
 
    //private
    bool stop;
    float timeStartRecovery;
    float timeStartDeceleration;
    float timeStartRotationDeceleration;
    float distance;
    RaycastHit hit;

    public override void Enter()
    {
        RecoveryInfoEnter();
        DecelerationEnter();
        DecelerationRotationEnter();
    }
    public override void Tick()
    {
        RecoveryInfoTick();
        DecelerationTick();
        RotationMoveTick();
        DecelerationRotationTick();
    }
    public override void Exit()
    {
       
    }

    public void RecoveryInfoEnter() {
        hit = boss.RaycastCollision();
        timeStartRecovery = 9999;
        stop = false;
    }
   
    public void RecoveryInfoTick() {
        distance = boss.CollisionDistance(hit.point);
        if (distance <= 5) {
            Debug.Log("collisione");
            animator.SetTrigger("Collision");
        }
        boss.Move();
        if (boss.Data.bossInfo.MoveSpeed <= boss.Data.wallDecelerationInfo.LowSpeed && !stop) {
            stop = true;
            timeStartRecovery = Time.time;

        }
        if ((Time.time - timeStartRecovery) > boss.Data.wallRecoveryInfo.RecoveryTime) {

            animator.SetTrigger("Anticipation");
        }
    }

    public void DecelerationEnter() {
        timeStartDeceleration = Time.time;
    }

    public void DecelerationTick() {

        if (Time.time - timeStartDeceleration > boss.Data.wallDecelerationInfo.WaitOnStart) {
            boss.Deceleration(boss.Data.wallDecelerationInfo.TimeDeceleration, boss.Data.wallDecelerationInfo.LowSpeed);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation();
    }

    public void DecelerationRotationEnter() {

        timeStartRotationDeceleration = Time.time;
    }

    public void DecelerationRotationTick() {
        if (Time.time - timeStartRotationDeceleration > boss.Data.wallRotationDecelerationInfo.WaitOnStart) {
            boss.View.DecelerationRotation(boss.Data.wallRotationDecelerationInfo.DecelerationTime);
        }
    }
}
