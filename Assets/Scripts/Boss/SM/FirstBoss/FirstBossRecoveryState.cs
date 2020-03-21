using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FirstBossRecoveryState : FirstBossState
{
    //Inspector
    public RecoveryData recoveryData;
    public DecelerationData decelerationData;

    public override void Enter()
    {
        base.Enter();
        RecoveryInfoEnter();
    }
    public override void Tick()
    {

        base.Tick();
        SetCycleTimer();
    }
    public override void Exit()
    {
        animator.SetBool("RecoveryOrbit", false);
        CheckVulnerability();
        boss.IsPrevStateReinitialize = false;
    }

    public void RecoveryInfoEnter() {
        OrbitTag(recoveryData);
    }
   

}
