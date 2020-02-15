using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossPreFightState : FirstBossState
{

    //Private
    int HookPointsCount;

    public override void Enter() {

        HookPointsCount = bossOrbitManager.HookPointList.Count;
        animator.SetInteger(MASKS_COUNT , HookPointsCount);
    }

    public override void Tick() {

        //When the Boss loses a Mask then the current state ends
        if (bossOrbitManager.HookPointList.Count != HookPointsCount) {

            animator.SetTrigger(END_STATE_TRIGGER);
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
        SetCycleTimer();
    }

    public override void Exit() {
        boss.IsPrevStateReinitialize = false;
    }

}
