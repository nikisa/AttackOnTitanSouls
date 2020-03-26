using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashDecelerationState : PlayerBaseState
{

    //Private
    PlayerDashData playerDashData;
    PlayerMovementData playerMovementData;
    bool IsTimerSet;

    #region CurveVariables
    AnimationCurve dashDecelCurve;
    AnimationCurve dashCurve;
    int IntegralIterations;
    float timer;
    
    #endregion

    public override void Enter() {

        #region setting private variables
        IntegralIterations = 1;
        IsTimerSet = false;
        timer = player.plusDeltaTime;
        playerDashData = player.playerDashData;
        playerMovementData = player.playerMovementData;
        dashCurve = playerDashData.DashDecelerationCurve;
        #endregion

        //Settaggio della DecelerationModule direttamente in Enter dato che si stratta di un valore non variabile
        player.DecelerationModule = player.dashVelocityModule / playerDashData.DashDecelerationTime;
        //Setto variabili per una lettura migliore
        dashDecelCurve = playerDashData.DashDecelerationCurve;
        //Crea la curva d'andamento del Dash
        setDashDecelerationCurve();
        

        //Dash eseguito in Enter della DashDecel dato lo scarto che si viene a creare nel Dash State
        player.Dash(player.dashVelocityModule, player.targetDir, dashCurve , 0 , timer, IntegralIterations);

    }

    public override void Tick() {
        timer += Time.deltaTime;
        player.dashVelocityModule = player.VelocityVector.magnitude;

        if (timer <= playerDashData.DashDecelerationTime) {
            player.DashDeceleration(dashDecelCurve, timer - Time.deltaTime, timer, IntegralIterations);
        }
        else {
            player.DashDeceleration(dashDecelCurve, timer - Time.deltaTime, playerDashData.DashDecelerationTime , IntegralIterations);
        }
        

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


    void setDashDecelerationCurve() {
        
        float dashSpeed = playerDashData.ActiveDashDistance / playerDashData.ActiveDashTime;

        //Resetto i KeyFrame prima di settare quelli nuovi in Enter
        playerDashData.DashDecelerationCurve.keys = null;

        //Setto i KeyFrame della curva del DashDeceleration in Enter
        playerDashData.DashDecelerationCurve.AddKey(0, dashSpeed);
        playerDashData.DashDecelerationCurve.AddKey(playerDashData.DashDecelerationTime, 0);
    }

}
