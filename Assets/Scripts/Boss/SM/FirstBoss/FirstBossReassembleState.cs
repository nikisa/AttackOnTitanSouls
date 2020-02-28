using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossReassembleState : FirstBossState
{

    //Private
    private List<MaskBehaviourData> masksBehaviourList;

    public override void Enter() {
        masksBehaviourList = bossOrbitManager.MasksBehaviourList;


    }

    public override void Tick() {

        for (int i = 0; i < bossOrbitManager.MasksList.Count; i++) {
            if (!bossOrbitManager.checkCorrectPosition(i)) {
                bossOrbitManager.MasksList[i].RotateAroud(180 , 2);
            }
        }

    }
}
