using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossOrbitDecelerationState : FirstBossState
{

    public override void Tick() {
        bossOrbitManager.DecelerationMask();
        if (getMasksStopped() == bossOrbitManager.MasksList.Count) {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }


    public int getMasksStopped() {
        int count = 0;
        for (int i = 0; i < bossOrbitManager.MasksList.Count; i++) {
            if (bossOrbitManager.MasksList[i].AngularVelocity == 0) {
                count++;
            }
        }
        return count;
    }

}
