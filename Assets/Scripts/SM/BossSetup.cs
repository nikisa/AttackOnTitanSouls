using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSetup : BossBaseState
{

    void Setup() {
        
        //boss.Data.moveToInfo.Target=SetTarget(BossData.Targets.Player);
        //boss.Data.wallMoveToInfo.Target = boss.Data.moveToInfo.Target;

        //boss.Data.orbit.CenterPoint = SetTargetOrbit(BossData.TargetsOrbit.CenterPoint);
        //boss.Data.orbit.Tentacle = SetTargetOrbit(BossData.TargetsOrbit.Tentacle);
        //boss.Data.orbit.Center = SetTargetOrbit(BossData.TargetsOrbit.Boss);
        boss.Player = FindObjectOfType<PlayerController>();
        boss.Graphics = GameObject.FindGameObjectWithTag("BossGraphics");

    }

    public override void Enter() {
        Setup();
        
        TriggerExitState();
        Debug.Log("Enter");
    }
   
   



}
