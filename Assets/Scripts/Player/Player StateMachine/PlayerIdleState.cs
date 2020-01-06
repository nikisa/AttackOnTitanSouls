using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerIdleState : PlayerBaseState {

    //Inspector
    public PlayerIdleData playerIdleData;


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
        player.layerMask = 1 << 10 /*| 1<<12*/;
        timer = Time.time;

        accelRatePerSec = playerIdleData.maxSpeed / (playerIdleData.framesZeroToMax / 60);
        decelRatePerSec = -playerIdleData.maxSpeed / (playerIdleData.framesMaxToZero / 60);
        forwardVelocity = 0f;
    }

    public override void Tick() {

        player.PlayerInclination();

        if (Time.time < timer + playerIdleData.resumeControl) {
            canDash = false;
        }
        else {
            canDash = true;
        }

        dataInput = player.dataInput;

        if (dataInput.Dash && canDash)
        {
            startDash = Time.time;
            canDash = false;
            animator.SetTrigger(DASH);
        }

        if (forwardVelocity == playerIdleData.maxSpeed) {
            canDash = true;
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

    public override void Exit() {
        
    }
}

