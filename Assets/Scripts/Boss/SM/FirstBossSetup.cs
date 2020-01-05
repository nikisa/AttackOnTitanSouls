using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossSetup : FirstBossState
{
    //Inspector
    public List<AnticipationData> everyAnticipationData;

    void Setup() {        
        boss.loops = everyAnticipationData[0].Loops;

        //Anticipation Setup
        for (int i = 0; i < everyAnticipationData.Count; i++) {
            everyAnticipationData[i].loops = everyAnticipationData[i].Loops + 1;
        }
    }

    public override void Enter() {
        Setup();
        TriggerExitState();
    }
}
