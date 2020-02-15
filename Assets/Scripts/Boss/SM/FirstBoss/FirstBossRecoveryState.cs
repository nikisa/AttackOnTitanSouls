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
    int iterations;
    int layerResult;
    int layerWall;
    int layerPlayer;

    public override void Enter()
    {

        iterations = 1;
        layerWall = 10;
        layerPlayer = 11;

        RecoveryInfoEnter();
    }
    public override void Tick()
    {
        Timer(recoveryData);
        //RecoveryInfoTick();
      //  DecelerationTick();
        detectCollsion();
    }
    public override void Exit()
    {
        animator.SetBool("RecoveryOrbit", false);
        CheckVulnerability();
        boss.IsPrevStateReinitialize = false;
        ResetTimer(recoveryData);
    }

    public void RecoveryInfoEnter() {
        timeStartRecovery = Time.time;
        OrbitTag(recoveryData);
    }
   
    //public void RecoveryInfoTick() {
    //    //Ends state when the timer has finished
    //    if ((Time.time - timeStartRecovery) > recoveryData.Time) {
    //        animator.SetTrigger(END_STATE_TRIGGER);
    //    }
    //}

    //Does a deceleration when finishing the movement
    //public void DecelerationTick() {
    //    boss.Deceleration(decelerationData.TimeDeceleration, decelerationData.LowSpeed , boss.MaxSpeed);
    //}

    void detectCollsion() {
        layerResult = boss.MovingDetectPlayer(iterations);
        animator.SetInteger("Layer", 0);

        if (layerResult == layerPlayer) {
            if (!boss.Player.IsImmortal) {
            PlayerController.DmgEvent();
            }
        }
    }

}
