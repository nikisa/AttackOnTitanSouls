using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAnticipationState : FirstBossState
{
    //Public 
    public AnticipationData anticipationData;
    public RotationAccelerationData rotationAccelerationData;
    public GraphicsAnticipationData graphicsAnticipationData;
    public RotationMoveData rotationMoveData;
    
    //Private
    float timeStartAnticipation;
 
    public override void Enter()
    {
        OrbitTag(anticipationData);
        //Se Tag = 0 non reinizializza loops
        if (boss.IsPrevStateReinitialize) {
            anticipationData.loops = boss.loops;
      
        }
        EnterAnticipation();
        boss.View.ChangeMaterial(graphicsAnticipationData.AnticipationMat);
    }

    public override void Tick()
    {
        //RotationAccelerationTick();
        AnticipationTick();

    }
    public override void Exit()
    {
        animator.SetBool("AnticipationOrbit", false);
        CheckVulnerability();
        boss.View.ChangeMaterial(graphicsAnticipationData.NormalMat);
        AnticipationExit();
    }

    public void EnterAnticipation() {
        --anticipationData.loops; 
        boss.loops = anticipationData.loops; //Used for the Reinitialize State
        animator.SetInteger("Loops", anticipationData.loops);
        timeStartAnticipation = Time.time;
        if (anticipationData.InfinteLoops && anticipationData.loops <= 0) {
            anticipationData.loops = int.MaxValue;
        }
    }

    //Da refactorare con i timer come Parameters
    public void AnticipationTick() {
        if ((Time.time - timeStartAnticipation) > anticipationData.AnticipationTime) {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }


    public void AnticipationExit()
    {
        boss.IsPrevStateReinitialize = false;
        animator.SetBool("Anticipation", false);
    }
    
}
