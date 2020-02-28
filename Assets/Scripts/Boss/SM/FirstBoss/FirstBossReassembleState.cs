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
        


    }
}
