using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossReassembleState : FirstBossState
{

    //Private
    private List<MaskBehaviourData> masksBehaviourList;
    private int maxReassembleSpeed = 720; 

    public override void Enter() {
        masksBehaviourList = bossOrbitManager.MasksBehaviourList;
    }

    public override void Tick() {

        for (int i = 0; i < bossOrbitManager.MasksList.Count; i++) {
            if (!bossOrbitManager.checkCorrectPosition(i , bossOrbitManager.MasksList[i].MaskID)) {
                if (bossOrbitManager.MasksBehaviourList[i].AngularMaxSpeed > maxReassembleSpeed) {
                    bossOrbitManager.MasksList[i].RotateAroud(bossOrbitManager.MasksBehaviourList[i].AngularMaxSpeed/4, bossOrbitManager.MasksBehaviourList[i].AngularAccelerationTime);
                }
                else {
                    bossOrbitManager.MasksList[i].RotateAroud(bossOrbitManager.MasksBehaviourList[i].AngularMaxSpeed, bossOrbitManager.MasksBehaviourList[i].AngularAccelerationTime);
                }
                
            }
        }

    }

    public override void Exit() {
        bossOrbitManager.ResetVelocity();
    }

}
