using System.Collections;
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
    bool canDash = true;    
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
        timer = Time.time;

        accelRatePerSec = playerIdleData.maxSpeed / (playerIdleData.framesZeroToMax / 60);
        decelRatePerSec = -playerIdleData.maxSpeed / (playerIdleData.framesMaxToZero / 60);
        forwardVelocity = 0f;
    }

    public override void Tick() {

        Debug.Log("SPEED: " + player.dashMovementSpeed);

        player.CheckInput();

        player.PlayerInclination();

        if (Time.time < timer + playerDashData.EnableDashAt) {
            canDash = false;
        }
        else {
            canDash = true;
        }

        dataInput = player.dataInput;

        SetAnimationParameter();
        if (dataInput.Dash && canDash)
        {
            startDash = Time.time;
            canDash = false;
            player.horizontalDash = player.dataInput.Horizontal;
            player.verticalDash = player.dataInput.Vertical;
            animator.SetTrigger(DASH);
        }

        if (dataInput.Vertical != 0 || dataInput.Horizontal != 0) {
            //player.ReadInputGamepad(dataInput , accelRatePerSec);
            player.ReadInputKeyboard(dataInput , accelRatePerSec , playerIdleData.maxSpeed);
        }

        if (player.newInput) {
            player.Movement();
        }
        else {
            player.Deceleration(decelRatePerSec);
        }

        player.newInput = false;
    }
    public void SetAnimationParameter()
    {
        animator.SetFloat("Horizontal", dataInput.Horizontal);
        animator.SetFloat("Vertical", dataInput.Vertical);
    }

    public override void Exit() {
        
    }
}

