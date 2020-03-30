using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDashState : PlayerBaseState
{

    //Inspector
    public PlayerDashData playerDashData;

    //Private
    float timeStart;
    float dashMovement;
    float timer;
    
    //Curve variables
    #region Curve Variables
    AnimationCurve dashCurve;
    float firstKeyFrameValue;
    float lastKeyFrameValue;
    int IntegralIterations;
    Vector3 realEndDashPosition;
    Vector3 fakeEndDashPosition;
    float endPointsDistance;
    #endregion

    public override void Enter() {
        
        IntegralIterations = 1;
        timeStart = Time.time;
        player.SetDashVelocity(playerDashData.ActiveDashDistance, playerDashData.ActiveDashTime);
        
        //Crea la curva d'andamento del Dash
        setDashCurve();

        //Setto variabili per una lettura migliore
        dashCurve = playerDashData.DashCurve;
        
        //realEndDashPosition = player.transform.position + player.targetDir.normalized * playerDashData.ActiveDashDistance;
        //fakeEndDashPosition = player.transform.position + player.targetDir.normalized * (playerDashData.ActiveDashDistance * 1.5f);
        //endPointsDistance = Vector3.Distance(realEndDashPosition, fakeEndDashPosition);

        timer = 0;
    }

    public override void Tick() {
        timer += Time.deltaTime;
        if (timer <= playerDashData.ActiveDashTime  /*Mathf.Floor(Vector3.Distance(player.transform.position, fakeEndDashPosition)) > endPointsDistance*/) {
            player.Dash(player.dashVelocityModule , player.targetDir , dashCurve , timer - Time.deltaTime , timer, IntegralIterations);
        }
        else {
            //if (timer > playerDashData.ActiveDashTime) {
            player.Dash(player.dashVelocityModule, player.targetDir, dashCurve, timer - Time.deltaTime, playerDashData.ActiveDashTime, IntegralIterations);
            //}

            player.plusDeltaTime = timer - playerDashData.ActiveDashTime;

            animator.SetTrigger(DASH_DECELERATION);
        }
        
    }

    public override void Exit() {
        
    }

    void setDashCurve() {

        float dashSpeed = playerDashData.ActiveDashDistance / playerDashData.ActiveDashTime;
        playerDashData.DashCurve.keys = null;

        playerDashData.DashCurve.AddKey(0, dashSpeed);
        playerDashData.DashCurve.AddKey(playerDashData.ActiveDashTime , dashSpeed);
    }


}
