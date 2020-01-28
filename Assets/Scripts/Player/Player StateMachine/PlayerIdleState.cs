﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerIdleState : PlayerBaseState {

    //Inspector
    public PlayerIdleData playerIdleData;

    //Public 
    [HideInInspector]
    public PlayerDashData playerDashData;


    // Private
    //bool canDash = true;    
    float startDash;
    float startTime;
    float boostTime;
    float timer;
    float accelRatePerSec;
    float decelRatePerSec;
    float forwardVelocity;
    Vector3 movementVelocity = Vector3.zero;
    DataInput dataInput;


    public override void Enter() {

        playerDashData = player.playerDashData;

        player.layerMask = 1 << 10 /*| 1<<12*/;
        //timer = Time.time;

        accelRatePerSec = playerIdleData.maxSpeed / (playerIdleData.framesZeroToMax / 60);
        decelRatePerSec = -playerIdleData.maxSpeed / (playerIdleData.framesMaxToZero / 60);
        forwardVelocity = 0f;
    }

    public override void Tick() {

        Debug.Log(player.newInput);

        Debug.Log("SPEED: " + player.dashMovementSpeed);

        //player.CheckInput();

        player.PlayerInclination();

        if (player.newInput) {
            
            player.Movement();
        }
        else {
            animator.SetTrigger(MOVEMENT_DECELERATION);
        }

        if (Time.time - player.timerDash > playerDashData.EnableDashAt) {
            player.canDash = true;
        }
        //else {
        //    player.canDash = false;
        //}

        dataInput = player.dataInput;

        if (dataInput.Dash && player.canDash /*&& ((dataInput.Horizontal == 1 || dataInput.Horizontal == -1) || (dataInput.Vertical == 1 || dataInput.Vertical == -1))*/ )
        {
            startDash = Time.time;
            player.canDash = false;
            player.horizontalDash = player.dataInput.Horizontal;
            player.verticalDash = player.dataInput.Vertical;
            animator.SetTrigger(DASH);
        }

        //if (dataInput.Vertical != 0 || dataInput.Horizontal != 0) {
            //player.ReadInputGamepad(dataInput , accelRatePerSec);
            player.ReadInputKeyboard(dataInput , accelRatePerSec , playerIdleData.maxSpeed);
        //}

    }
    

    public override void Exit() {
        
    }
}

