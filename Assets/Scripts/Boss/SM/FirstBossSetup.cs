using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossSetup : FirstBossState
{
    //Inspector
    public List<AnticipationData> everyAnticipationData;
    public List<BaseData> DataToResetTime;
    void Setup() {        
        boss.loops = everyAnticipationData[0].Loops;

        //Anticipation Setup
        for (int i = 0; i < everyAnticipationData.Count; i++) {
            everyAnticipationData[i].Time = 0;
        }  
        for (int i = 0; i < DataToResetTime.Count; i++) {
            DataToResetTime[i].Time = 0;
        }

    }

    public override void Enter() {
        Setup();
        TriggerExitState();
    }
}
