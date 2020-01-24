using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashDecelerationState : PlayerBaseState
{

    //Private
    PlayerIdleData playerIdleData;
    PlayerDashData playerDashData;
    float InitialVelocity;
    float DashTimeFrames;
    float timeDeceleration;
    float Horizontal;
    float Vertical;

    public override void Enter() {

        Horizontal = player.dataInput.Horizontal;
        Vertical = player.dataInput.Vertical;

        playerDashData = player.playerDashData;
    }

    public override void Tick() {
        player.DashDeceleration(Horizontal ,  Vertical , playerDashData.DashDecelerationTime , playerDashData.ActiveDashDistance , playerDashData.ActiveDashTime);
    }

    public override void Exit() {
        
    }
}
