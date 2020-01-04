using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossPreFightState : FirstBossState
{

    //Private
    BossOrbitManager bossOrbitManager;
    float HookPointsCount;

    public override void Enter() {
        bossOrbitManager = FindObjectOfType<BossOrbitManager>();
        HookPointsCount = bossOrbitManager.HookPointList.Count;
    }

    public override void Tick() {
        //When the Boss loses a Mask then the current state ends
        if (bossOrbitManager.HookPointList.Count != HookPointsCount) {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }

    public override void Exit() {
        boss.IsPrevStateReinitialize = false;
    }

}
