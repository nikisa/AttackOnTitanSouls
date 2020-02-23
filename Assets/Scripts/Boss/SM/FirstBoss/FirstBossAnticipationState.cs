using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossAnticipationState : FirstBossState
{
    //Public 
    public AnticipationData anticipationData;

    //Private
    int layerResult;
    int layerWall;
    int layerPlayer;

    public override void Enter()
    {

        base.Enter();

        ResetCycleTimer();
        animator.SetInteger("Layer", 0);
        layerWall = 10;
        layerPlayer = 11;

        OrbitTag(anticipationData);
        
        if (boss.IsPrevStateReinitialize) {
            anticipationData.loops = boss.loops;
        }

        EnterAnticipation();
    }

    public override void Tick()
    {
        base.Tick();
        detectCollsion();
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
        animator.SetInteger("Loops", anticipationData.loops);

        if (anticipationData.InfinteLoops && anticipationData.loops <= 0) {
            anticipationData.loops = int.MaxValue;
        }
    }

    public void AnticipationExit()
    {
        boss.IsPrevStateReinitialize = false;
        animator.SetBool("Anticipation", false);
    }

    void detectCollsion() {
        if (layerResult == layerPlayer) {
            if (!boss.Player.IsImmortal) {
                PlayerController.DmgEvent();
            }
        }
    }

}
