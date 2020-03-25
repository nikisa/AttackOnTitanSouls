using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashDecelerationState : PlayerBaseState
{

    //Private
    PlayerDashData playerDashData;
    PlayerMovementData playerMovementData;
    bool IsTimerSet;

    float dashMovement;
    //Curve variables
    AnimationCurve dashDecelCurve;
    float firstKeyFrameValue;
    float lastKeyFrameValue;
    int IntegralIterations;
    float timer;

    public override void Enter() {
        IsTimerSet = false;
        timer = 0;
        playerDashData = player.playerDashData;
        playerMovementData = player.playerMovementData;


        player.DecelerationModule = player.dashVelocityModule / playerDashData.DashDecelerationTime;
        //Setto variabili per una lettura migliore
        dashDecelCurve = playerDashData.DashDecelerationCurve;

        //Crea la curva d'andamento del Dash
        setDashDecelerationCurve();
        
        IntegralIterations = 1;

        //Debug.Log("(DASH DECEL) dashVelocityModule: " + player.dashVelocityModule);
        //Debug.Log("(DASH DECEL) DecelerationModule: " + playerDashData.DashDecelerationTime);
        //firstKeyFrameValue = playerDashData.DashDecelerationCurve.keys[0].time;
        //lastKeyFrameValue = playerDashData.DashDecelerationCurve.keys[playerDashData.DashDecelerationCurve.keys.Length-1].time;

        //float space = Integration.IntegrateCurve(dashDecelCurve, firstKeyFrameValue, lastKeyFrameValue, 10);
        //Debug.Log("DashDecelSpace: " + space);
        //Debug.Log("xmin: " + firstKeyFrameValue);
        //Debug.Log("xMax: " + lastKeyFrameValue);

        player.decelSpace = 0;
    }

    public override void Tick() {
        Debug.Log("VelocityVector: " + player.VelocityVector);
        timer += Time.deltaTime;
        player.dashVelocityModule = player.VelocityVector.magnitude;

        player.DashDeceleration(dashDecelCurve, timer, IntegralIterations, player.playerDashData.frame);

        if (player.dashVelocityModule <= (playerDashData.ResumePlayerInput * playerMovementData.maxSpeed)) {

            if (!IsTimerSet) //Si può trattare senza booleana?
            {
                PlayerController.TimerEvent();
                IsTimerSet = true;
            }

            if (player.checkDeadZone()) {
                animator.SetTrigger(MOVEMENT);
            }
        }

        if (player.dashVelocityModule < (player.DecelerationModule * player.playerDashData.frame) && !player.checkDeadZone()) {
            animator.SetTrigger(IDLE);
        }
        
    }


    public override void Exit() {
        //player.move = Vector3.zero;
        //player.nextPosition = Vector3.zero;
        //player.AccelerationVector = Vector3.zero;
        //player.VelocityVector = Vector3.zero;
        //player.DecelerationVector = Vector3.zero;
        //player.Drag = 0;
        //player.AccelerationModule = 0;
        //player.DecelerationModule = 0;
    }

    void setDashDecelerationCurve() {
        
        float dashSpeed = playerDashData.ActiveDashDistance / playerDashData.ActiveDashTime;

        for (int i = 0; i < playerDashData.DashDecelerationCurve.keys.Length; i++) {
            playerDashData.DashDecelerationCurve.RemoveKey(i);
        }

        playerDashData.DashDecelerationCurve.AddKey(0, dashSpeed);
        playerDashData.DashDecelerationCurve.AddKey(playerDashData.DashDecelerationTime, 0);
    }

}
