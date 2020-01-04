using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossSetup : FirstBossState
{
    //Inspector
    public AnticipationData anticipationData;

    void Setup() {        
        boss.Player = FindObjectOfType<PlayerController>();
        boss.Graphics = GameObject.FindGameObjectWithTag("BossGraphics");
        boss.loops = anticipationData.Loops;

        //Anticipation Setup
        anticipationData.loops = anticipationData.Loops + 1;
    }

    public override void Enter() {
        Setup();
        TriggerExitState();
    }
}
