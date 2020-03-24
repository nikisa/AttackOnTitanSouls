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
    AnimationCurve dashCurve;
    float firstKeyFrameValue;
    float lastKeyFrameValue;
    int IntegralIterations;
    Vector3 realEndDashPosition;
    Vector3 fakeEndDashPosition;
    float endPointsDistance;

    public override void Enter() {
        
        IntegralIterations = 1;
        timeStart = Time.time;
        player.SetDashVelocity(playerDashData.ActiveDashDistance, playerDashData.ActiveDashTime);
        
        //Crea la curva d'andamento del Dash
        setDashCurve();
        //Setto variabili per una lettura migliore
        dashCurve = playerDashData.DashCurve;
        firstKeyFrameValue = playerDashData.DashCurve.keys[0].value;
        firstKeyFrameValue = playerDashData.DashCurve.keys[playerDashData.DashCurve.keys.Length-1].value;
        timer = 0;
        realEndDashPosition = player.transform.position + player.targetDir.normalized * playerDashData.ActiveDashDistance;
        fakeEndDashPosition = player.transform.position + player.targetDir.normalized * (playerDashData.ActiveDashDistance * 1.5f);
        endPointsDistance = Vector3.Distance(realEndDashPosition, fakeEndDashPosition);
        //dashMovement = Integration.IntegrateCurve(dashCurve , firstKeyFrameValue, firstKeyFrameValue , IntegralIterations);

        //startPointsDistance = player.transform.position; //Con la start non funziona perché prendendo direttamente player.transform.position ha troppi numeri dopo la virgola????

    }

    public override void Tick() {
        timer += Time.deltaTime;
        if (Mathf.Floor(Vector3.Distance(player.transform.position, fakeEndDashPosition)) > endPointsDistance) {
            Debug.LogError("Dist: " + Vector3.Distance(player.transform.position, realEndDashPosition));
            player.Dash(player.dashVelocityModule , player.targetDir , dashCurve , timer , IntegralIterations , player.playerDashData.frame);
        }
        else {
            animator.SetTrigger(DASH_DECELERATION);
        }
        
    }

    public override void Exit() {
        
    }

    void setDashCurve() {
        float dashSpeed = playerDashData.ActiveDashDistance / playerDashData.ActiveDashTime;
        for (int i = 0; i < playerDashData.DashCurve.keys.Length; i++) {
            playerDashData.DashCurve.RemoveKey(i);
        }
        playerDashData.DashCurve.AddKey(0, dashSpeed);
        playerDashData.DashCurve.AddKey(playerDashData.ActiveDashTime , dashSpeed);
    }

    //// Integrate area under AnimationCurve between start and end time
    //public static float IntegrateCurve(AnimationCurve curve, float startTime, float endTime, int steps) {
    //    return Integrate(curve.Evaluate, startTime, endTime, steps);
    //}

    //// Integrate function f(x) using the trapezoidal rule between x=x_low..x_high
    //public static float Integrate(Func<float, float> f, float x_low, float x_high, int N_steps) {
    //    float h = (x_high - x_low) / N_steps;
    //    float res = (f(x_low) + f(x_high)) / 2;
    //    for (int i = 1; i < N_steps; i++) {
    //        res += f(x_low + i * h);
    //    }
    //    return h * res;
    //}

}
