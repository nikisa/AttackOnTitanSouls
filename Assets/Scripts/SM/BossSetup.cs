using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSetup : BossBaseState
{

    void Setup() {
        boss.Data = boss.View.GetBossData();
        boss.Data.moveToInfo.Target=SetTarget(BossData.Targets.Player);
        boss.Data.wallMoveToInfo.Target = boss.Data.moveToInfo.Target;

        boss.Data.orbit.CenterPoint = SetTargetOrbit(BossData.TargetsOrbit.CenterPoint);
        boss.Data.orbit.Tentacle = SetTargetOrbit(BossData.TargetsOrbit.Tentacle);
        boss.Data.orbit.Center = SetTargetOrbit(BossData.TargetsOrbit.Boss);
        boss.Data.bossInfo.Player = FindObjectOfType<PlayerController>();
        boss.Data.bossInfo.Graphics = GameObject.FindGameObjectWithTag("BossGraphics");

    }

    public override void Enter() {
        Setup();
        
        TriggerExitState();
        Debug.Log("Enter");
    }
    public GameObject SetTarget(BossData.Targets _enumTarget) {
        GameObject result =null;
        switch (_enumTarget)
        {
            case BossData.Targets.Player:
                result = FindObjectOfType<PlayerController>().gameObject;
                break;
                
        }
        
        return result;



    }
    public GameObject SetTargetOrbit(BossData.TargetsOrbit _enumTarget)
    {
        GameObject result = null;
        switch (_enumTarget)
        {
            case BossData.TargetsOrbit.Boss:
                result = FindObjectOfType<BossController>().gameObject;
                break;
            case BossData.TargetsOrbit.CenterPoint:
                result = GameObject.FindGameObjectWithTag("CenterPoint");
                break;
            case BossData.TargetsOrbit.Tentacle:
                result = GameObject.FindGameObjectWithTag("Tentacle");
                break;

        }

        return result;



    }



}
