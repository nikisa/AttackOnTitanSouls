using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSetup : BossBaseState
{

    void Setup() {
        boss.Data = boss.View.GetBossData();
        boss.Data.moveToInfo.Target = FindObjectOfType<PlayerController>();
        boss.Data.wallMoveToInfo.Target = boss.Data.moveToInfo.Target;
        boss.Data.bossInfo.Player = FindObjectOfType<PlayerController>();
        boss.Data.bossInfo.Graphics = GameObject.FindGameObjectWithTag("BossGraphics");
        boss.Data.orbit.CenterPoint = GameObject.FindGameObjectWithTag("CenterPoint");
        boss.Data.orbit.Tentacle = GameObject.FindGameObjectWithTag("Tentacle");
        boss.Data.orbit.Center = FindObjectOfType<BossController>();

    }

    public override void Enter() {
        Setup();
        TriggerExitState();
        Debug.Log("Enter");
    }


}
