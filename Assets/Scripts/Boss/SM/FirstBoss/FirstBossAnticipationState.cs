using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAnticipationState : FirstBossState
{
    //Public 
    public AnticipationData anticipationData;

    //Private
    int layerResult;

    public override void Enter()
    {
        base.Enter();

        ResetCycleTimer();
        animator.SetInteger("Layer", 0);

        OrbitTag(anticipationData);

        if (boss.IsPrevStateReinitialize) {
            anticipationData.loops = boss.loops;
        }

        EnterAnticipation();
    }

    public override void Tick()
    {
        base.Tick();
        boss.loops = anticipationData.loops; //Used for the Reinitialize State (Prima era in Enter)
        //pezza del dio boia

    }

    public override void Exit()
    {
        animator.SetBool("AnticipationOrbit", false);
        CheckVulnerability();
        AnticipationExit();
    }

    public void EnterAnticipation() {
        --anticipationData.loops;
        boss.loops = anticipationData.loops; //Used for the Reinitialize State
        
        animator.SetInteger("Loops", boss.loops);

        if (anticipationData.InfinteLoops && anticipationData.loops <= 0) {
            anticipationData.loops = int.MaxValue;
        }
    }

    public void AnticipationExit()
    {
        boss.IsPrevStateReinitialize = false;
        animator.SetBool("Anticipation", false);
        
    }

}
