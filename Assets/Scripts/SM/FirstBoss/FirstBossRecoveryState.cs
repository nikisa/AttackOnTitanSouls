using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FirstBossRecoveryState : FirstBossState
{

    public RecoveryData recoveryData;
    public  RotationDecelerationData rotationDecelerationData;
    public DecelerationData decelerationData;
    public RotationMoveData rotationMoveData;

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
        if (distance <= 2) {
            Debug.Log("collisione");
            animator.SetTrigger("Collision");
        }

        if (decelerationData.LowSpeed >= 0) {
            boss.Move();
        }
        else {
            boss.NegativeMove();
        }
        

        if (boss.MoveSpeed <= decelerationData.LowSpeed && !stop) {
            stop = true;
            timeStartRecovery = Time.time;

        }
        if ((Time.time - timeStartRecovery) > recoveryData.RecoveryTime) {
            animator.SetTrigger("Anticipation");
        }
    }

    public void DecelerationEnter() {
        timeStartDeceleration = Time.time;
    }

    public void DecelerationTick() {

        if (Time.time - timeStartDeceleration > decelerationData.WaitOnStart) {
            boss.Deceleration(decelerationData.TimeDeceleration, decelerationData.LowSpeed);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation(rotationMoveData.MaxSpeed);
    }

    public void DecelerationRotationEnter() {

        timeStartRotationDeceleration = Time.time;
    }

    public void DecelerationRotationTick() {
        if (Time.time - timeStartRotationDeceleration > rotationDecelerationData.WaitOnStart) {
            boss.View.DecelerationRotation(rotationDecelerationData.DecelerationTime , rotationDecelerationData.LowSpeed);
        }
    }
}
