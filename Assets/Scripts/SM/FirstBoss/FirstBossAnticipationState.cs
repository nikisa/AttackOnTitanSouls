using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAnticipationState : FirstBossState
{
    public AnticipationData anticipationData;
    public RotationAccelerationData rotationAccelerationData;
    public GraphicsAnticipationData graphicsAnticipationData;
    public RotationMoveData rotationMoveData;
    //private
    float timeStartAnticipation;
    float timeStartRotation;
    float loops;

    
    public override void Enter()
    {
        loopsInit();
        EnterAnticipation();
        boss.View.ChangeMaterial(graphicsAnticipationData.AnticipationMat);
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
       boss.View.ChangeMaterial(graphicsAnticipationData.NormalMat);
    }

    public void loopsInit() {
        loops = anticipationData.Loops;
    }

    public void EnterAnticipation() {

        //boss.MoveSpeed = 0; è da fare? 
        loops--; // 
        timeStartAnticipation = Time.time;
        if (anticipationData.InfinteLoops && loops <= 0) {
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
        if (Time.time - timeStartRotation > rotationAccelerationData.WaitOnStart) {
            boss.View.AccelerationRotation(rotationAccelerationData.AccelerationTime, rotationMoveData.MaxSpeed);
        }
    }

    public void AnticipationTick() {
        if ((Time.time - timeStartAnticipation) > anticipationData.AnticipationTime) {
            animator.SetTrigger(MOVETO);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation(rotationMoveData.MaxSpeed);
    }

}
