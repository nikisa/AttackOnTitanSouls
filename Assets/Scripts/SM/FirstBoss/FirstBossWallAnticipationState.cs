using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossWallAnticipationState : FirstBossState
{
    //private
    float timeStartAnticipation;
    float timeStartRotation;
    float loops;

    
    public override void Enter()
    {
        loopsInit();
        EnterAnticipation();
        boss.View.ChangeMaterial(boss.Data.wallGraphicsAnticipationInfo.AnticipationMat);
        EnterRotationAcceleration();
    }

    public override void Tick()
    {
        RotationAccelerationTick();
        AnticipationTick();
        RotationMoveTick();

    }
    public override void Exit()
    {
       boss.View.ChangeMaterial(boss.Data.wallGraphicsAnticipationInfo.NormalMat);
    }

    public void loopsInit() {
        loops = boss.Data.wallAnticipationInfo.Loops;
    }

    public void EnterAnticipation() {

        //boss.MoveSpeed = 0; è da fare? 
        loops--; // 
        timeStartAnticipation = Time.time;
        if (boss.Data.wallAnticipationInfo.InfinteLoops && loops <= 0) {
            loops = 999999;
            //animator.SetTrigger(IDLE);
        }
        if (loops <= 0) {
            Debug.Log("hhh");
            animator.SetTrigger(IDLE);
        }
    }

    public void EnterRotationAcceleration() {
        timeStartRotation = Time.time;
    }

    public void RotationAccelerationTick() {
        if (Time.time - timeStartRotation > boss.Data.wallRotationAccelerationInfo.WaitOnStart) {
            boss.View.AccelerationRotation(boss.Data.wallRotationAccelerationInfo.AccelerationTime, boss.Data.wallRotationAccelerationInfo.MaxSpeed);
        }
    }

    public void AnticipationTick() {
        if ((Time.time - timeStartAnticipation) > boss.Data.wallAnticipationInfo.AnticipationTime) {
            animator.SetTrigger(MOVETO);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation();
    }

}
