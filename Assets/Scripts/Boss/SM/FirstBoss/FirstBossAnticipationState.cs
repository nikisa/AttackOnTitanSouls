using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAnticipationState : FirstBossState
{
    //Public 
    public AnticipationData anticipationData;

    public override void Enter()
    {
        base.Enter();
        boss.hasAwaken = true;
        ResetCycleTimer();
        animator.SetInteger("Layer", 0);

        OrbitTag(anticipationData);
        EnterAnticipation();
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void Exit()
    {
        animator.SetBool("AnticipationOrbit", false);
        CheckVulnerability();
        AnticipationExit();
    }

    public void EnterAnticipation() {
        --boss.loops;
        animator.SetInteger("Loops", boss.loops);

        if (anticipationData.InfinteLoops && boss.loops <= 0) {
            boss.loops = int.MaxValue;
        }
    }

    public void AnticipationExit()
    {
        boss.IsPrevStateReinitialize = false;
        animator.SetBool("Anticipation", false);
    }
}
