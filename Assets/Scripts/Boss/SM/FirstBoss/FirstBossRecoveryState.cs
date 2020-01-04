using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FirstBossRecoveryState : FirstBossState
{
    //Inspector
    public RecoveryData recoveryData;
    public DecelerationData decelerationData;

    //Private
    float timeStartRecovery;

    public override void Enter()
    {
        RecoveryInfoEnter();
    }
    public override void Tick()
    {
        RecoveryInfoTick();
        DecelerationTick();
    }
    public override void Exit()
    {
        animator.SetBool("RecoveryOrbit", false);
        CheckVulnerability();
        boss.IsPrevStateReinitialize = false;
    }

    public void RecoveryInfoEnter() {
        timeStartRecovery = Time.time;
    }
   
    public void RecoveryInfoTick() {
        //Ends state when the timer has finished
        if ((Time.time - timeStartRecovery) > recoveryData.Time) {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }

    //Does a deceleration when finishing the movement
    public void DecelerationTick() {
        boss.Deceleration(decelerationData.TimeDeceleration, decelerationData.LowSpeed , boss.MaxSpeed);
    }

}
