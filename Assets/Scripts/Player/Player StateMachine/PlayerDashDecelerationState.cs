using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashDecelerationState : PlayerBaseState
{

    //Private
    PlayerDashData playerDashData;
    PlayerDecelInTimeData playerDecelInTimeData;
    PlayerMovementData playerMovementData;
    float InitialVelocity;
    float DashTimeFrames;
    float timeDeceleration;
    float HorizontalDash;
    float VerticalDash;
    float Horizontal;
    float Vertical;
    bool IsTimerSet;
    

    public override void Enter() {
        IsTimerSet = false;

        playerDashData = player.playerDashData;
        playerMovementData = player.playerMovementData;

        player.DecelerationModule = player.dashVelocityModule / playerDashData.DashDecelerationTime;
        Debug.Log("(DASH DECEL) dashVelocityModule: " + player.dashVelocityModule);
        Debug.Log("(DASH DECEL) DecelerationModule: " + playerDashData.DashDecelerationTime);
    }

    public override void Tick() {
        player.dashVelocityModule = player.VelocityVector.magnitude;

        if (player.dashVelocityModule <= (playerDashData.ResumePlayerInput * playerMovementData.maxSpeed)) {

            if (!IsTimerSet) //Come trattarlo senza booleana?
            {
                PlayerController.TimerEvent();
                IsTimerSet = true;
            }

            if (Mathf.Pow(Input.GetAxis("Horizontal"), 2) + Mathf.Pow(Input.GetAxis("Vertical"), 2) > Mathf.Pow(player.DeadZoneValue, 2)) {
                animator.SetTrigger(MOVEMENT);
            }
        }

        if (player.dashVelocityModule < (player.DecelerationModule * Time.deltaTime) && Mathf.Pow(Input.GetAxis("Horizontal"), 2) + Mathf.Pow(Input.GetAxis("Vertical"), 2) < Mathf.Pow(player.DeadZoneValue, 2)) { //settare il controllo della deadzone in una booleana
            animator.SetTrigger(IDLE);
        }
        player.DashDeceleration();
    }


    public override void Exit() {
        //player.move = Vector3.zero;
        //player.nextPosition = Vector3.zero;
        player.AccelerationVector = Vector3.zero;
        player.VelocityVector = Vector3.zero;
        //player.DecelerationVector = Vector3.zero;
        //player.Drag = 0;
        //player.AccelerationModule = 0;
        //player.DecelerationModule = 0;
    }

}
