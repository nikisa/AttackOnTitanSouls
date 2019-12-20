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
    int loops;

     public void Awake()   /// dove metterla?
     {
       
        loops = anticipationData.Loops +1;
        //animator.SetInteger("Loops", anticipationData.Loops);

    }
    public override void Enter()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Anticipation")) {
            animator.SetBool("AnticipationOrbit", true);
        }


        //loopsInit();
        EnterAnticipation();
        boss.View.ChangeMaterial(graphicsAnticipationData.AnticipationMat);
        EnterRotationAcceleration();
    }

    public override void Tick()
    {
        RotationAccelerationTick();
        AnticipationTick();
        //RotationMoveTick();

    }
    public override void Exit()
    {
        animator.SetBool("AnticipationOrbit", false);
        CheckVulnerability();
        boss.View.ChangeMaterial(graphicsAnticipationData.NormalMat);
        AnticipationExit();
    }

    public void loopsInit() {
        loops = anticipationData.Loops;
    }

    public void EnterAnticipation() {

        //boss.MoveSpeed = 0; è da fare? 
        loops--; // 
        animator.SetInteger("Loops", loops);
        timeStartAnticipation = Time.time;
        if (anticipationData.InfinteLoops && loops <= 0) {
            loops = 999999;
            //animator.SetTrigger(IDLE);
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
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation(rotationMoveData.MaxSpeed);
    }
    public void AnticipationExit()
    {
        if (loops <= 0)
        {
            Debug.Log("fine ciclo");
            loops = anticipationData.Loops +1;
           
        }
        animator.SetBool("Anticipation", false);
    }

}
