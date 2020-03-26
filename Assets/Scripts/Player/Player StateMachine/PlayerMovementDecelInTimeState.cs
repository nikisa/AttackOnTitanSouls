using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDecelInTimeState : PlayerBaseState
{
    
    //Inspector
    public PlayerDecelInTimeData playerDecelInTimeData;

    //Private
    PlayerMovementData playerMovementData;
    float timer;
    int iterations;
    float finalDeltaTime;


    public override void Enter() {
        playerMovementData = player.playerMovementData;
        player.DecelerationModule = (playerMovementData.maxSpeed) / (playerDecelInTimeData.DecelerationTime);
        setMovementDecelerationCurve();
        iterations = 1;
        timer = 0;
    }

    public override void Tick() {
        timer += Time.deltaTime;
        if (player.VelocityVector.magnitude > player.DecelerationModule * Time.deltaTime) {

            player.Deceleration(playerDecelInTimeData.MovementDecelerationCurve , timer - Time.deltaTime , timer , iterations);
        }
        else {
            player.VelocityVector = Vector3.zero;
            animator.SetTrigger(IDLE);
        }

        if (player.checkDeadZone()) {
            animator.SetTrigger(MOVEMENT);
        }

    }

    void setMovementDecelerationCurve() {

        playerDecelInTimeData.MovementDecelerationCurve.keys = null;
        finalDeltaTime = player.VelocityVector.magnitude / player.DecelerationModule;

        playerDecelInTimeData.MovementDecelerationCurve.AddKey(0, player.VelocityVector.magnitude);
        playerDecelInTimeData.MovementDecelerationCurve.AddKey(finalDeltaTime, 0);
    }


    public override void Exit() {
        player.Deceleration(playerDecelInTimeData.MovementDecelerationCurve, timer - Time.deltaTime ,finalDeltaTime , iterations);
    }

}
