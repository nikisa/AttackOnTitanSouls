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

        base.Enter();

        iterations = 1;
        layerWall = 10;
        layerPlayer = 11;

        RecoveryInfoEnter();
    }
    public override void Tick()
    {
        detectCollsion();
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
        timeStartRecovery = Time.time;
        OrbitTag(recoveryData);
    }
   
    void detectCollsion() {
        layerResult = boss.MovingDetectCollision(iterations, boss.nextPosition, boss.VelocityVector.magnitude);
        animator.SetInteger("Layer", 0);

        if (layerResult == layerPlayer) {
            if (!boss.Player.IsImmortal) {
            PlayerController.DmgEvent();
            }
        }
    }

}
