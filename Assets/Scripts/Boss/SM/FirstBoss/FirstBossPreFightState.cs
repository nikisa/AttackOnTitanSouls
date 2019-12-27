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
        //bossOrbitManager.SetHookPoints(); //Da spostare in SetUp
        HookPointsCount = bossOrbitManager.HookPointList.Count;
    }

    public override void Tick() {


        if (bossOrbitManager.HookPointList.Count != HookPointsCount) {
            Debug.Log("Enter");
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }

    public override void Exit() {
        boss.IsPrevStateReinitialize = false;
    }

}
