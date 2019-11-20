using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAnticipationState : FirstBossState
{
    //private
    float timeStartAnticipation;
    float timeStartRotation;
    float loops;

    public void Awake()
    {
        loops = boss.Data.anticipationInfo.Loops;
    }

    public override void Enter()
    {
        EnterAnticipation();
        boss.View.ChangeMaterial(boss.Data.graphicsAnticipationInfo.AnticipationMat);
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
        boss.View.ChangeMaterial(boss.Data.graphicsAnticipationInfo.NormalMat);
    }

    public void EnterAnticipation() {

        //boss.MoveSpeed = 0; è da fare? 
        loops--; // 
        timeStartAnticipation = Time.time;
        if (boss.Data.anticipationInfo.InfinteLoops && loops <= 0) {
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
        if (Time.time - timeStartRotation > boss.Data.rotationAccelerationInfo.WaitOnStart) {
            boss.View.AccelerationRotation(boss.Data.rotationAccelerationInfo.AccelerationTime, boss.Data.rotationAccelerationInfo.MaxSpeed);
        }
    }

    public void AnticipationTick() {
        if ((Time.time - timeStartAnticipation) > boss.Data.anticipationInfo.AnticipationTime) {
            animator.SetTrigger(MOVETO);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation();
    }

}
