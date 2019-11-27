using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIdleState : PlayerBaseState {

    //Inspector
    public PlayerIdleData playerIdleData;
    //public PlayerDashState playerDashData;

    // Private
    bool newInput;
    bool canDash = true;    
    float accelRatePerSec;
    float decelRatePerSec;
    float forwardVelocity;
    float startDash;
    float startTime;
    float boostTime;
    Vector3 movementVelocity = Vector3.zero;
    //InputData inputData;
    DataInput dataInput;

    
    public void ReadInputKeyboard(DataInput dataInput) {

        movementVelocity = Vector3.zero;

        if (!playerIdleData.isHookTest) {
            // Set vertical movement
            if (dataInput.Vertical != 0f) {
                forwardVelocity += accelRatePerSec * Time.deltaTime;
                forwardVelocity = Mathf.Clamp(forwardVelocity, 0, playerIdleData.maxSpeed);
                movementVelocity += Vector3.forward * dataInput.Vertical * forwardVelocity;
            }

            // Set horizontal movement
            if (dataInput.Horizontal != 0f) {
                forwardVelocity += accelRatePerSec * Time.deltaTime;
                forwardVelocity = Mathf.Clamp(forwardVelocity, 0, playerIdleData.maxSpeed);
                movementVelocity += Vector3.right * dataInput.Horizontal * forwardVelocity;
            }
        }
        else {
            // Set vertical movement
            if (dataInput.Vertical != 0f) {
                forwardVelocity += accelRatePerSec * Time.deltaTime;
                forwardVelocity = Mathf.Clamp(forwardVelocity, 0, playerIdleData.maxSpeed);
                movementVelocity += Vector3.up * dataInput.Vertical * forwardVelocity;
            }

            // Set horizontal movement
            if (dataInput.Horizontal != 0f) {
                forwardVelocity += accelRatePerSec * Time.deltaTime;
                forwardVelocity = Mathf.Clamp(forwardVelocity, 0, playerIdleData.maxSpeed);
                movementVelocity += Vector3.right * dataInput.Horizontal * forwardVelocity;
            }
        }
        
        newInput = true;
    }

    public void ReadInputGamepad(DataInput dataInput) {

        movementVelocity = Vector3.zero;

        // Set vertical movement gamepad
        if (dataInput.Vertical != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, playerIdleData.maxSpeed);
            movementVelocity += Vector3.forward * dataInput.Vertical * forwardVelocity;
        }

        // Set horizontal movement gamepad
        if (dataInput.Horizontal != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, playerIdleData.maxSpeed);
            movementVelocity += Vector3.right * dataInput.Horizontal * forwardVelocity;
        }

        newInput = true;
    }

    public override void Enter() {

        accelRatePerSec = playerIdleData.maxSpeed / (playerIdleData.framesZeroToMax / 60);
        decelRatePerSec = -playerIdleData.maxSpeed / (playerIdleData.framesMaxToZero / 60);
        forwardVelocity = 0f;
    }

    public override void Tick() {

        dataInput = player.dataInput;

        if (dataInput.Dash && canDash)
        {
            startDash = Time.time;
            canDash = false;
            animator.SetTrigger(DASH);
        }

        if ((Time.time - startDash) > playerIdleData.resumeControl) {
            canDash = true;
        }

        if (dataInput.Vertical != 0 || dataInput.Horizontal != 0) {
            ReadInputGamepad(dataInput);
            ReadInputKeyboard(dataInput);
        }

        if (newInput) {
            Movement();
        }
        else {
            Deceleration();
        }

        newInput = false;
    }

    public override void Exit() {
        
    }

    public void Deceleration() {
        forwardVelocity = 0;

        if (movementVelocity.x < decelRatePerSec * Time.deltaTime)
            movementVelocity.x = movementVelocity.x - decelRatePerSec * Time.deltaTime;
        else if (movementVelocity.x > -decelRatePerSec * Time.deltaTime)
            movementVelocity.x = movementVelocity.x + decelRatePerSec * Time.deltaTime;
        else {
            movementVelocity.x = 0;
        }

        if (!playerIdleData.isHookTest) {
            if (movementVelocity.z < decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.z > -decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z + decelRatePerSec * Time.deltaTime;
            else {
                movementVelocity.z = 0;
            }

            
            player.transform.Translate(movementVelocity * Time.deltaTime);
        }
        else {
            if (movementVelocity.y < decelRatePerSec * Time.deltaTime)
                movementVelocity.y = movementVelocity.z - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.y > -decelRatePerSec * Time.deltaTime)
                movementVelocity.y = movementVelocity.z + decelRatePerSec * Time.deltaTime;
            else {
                movementVelocity.y = 0;
            }

            player.transform.Translate(movementVelocity * Time.deltaTime);

        }

    }

    public void Movement() {
        if (!playerIdleData.isHookTest) {
            player.transform.Translate(movementVelocity * Time.deltaTime);
        }
        else {
            player.transform.Translate(movementVelocity * Time.deltaTime);
        }
    }


    //?Test player rope length constraint?
    public void RopeLengthConstraint(Vector3 nodePosition) {
        Vector3 constraint = new Vector3(nodePosition.x, nodePosition.y, player.transform.position.z);
        player.transform.Translate(constraint * Time.deltaTime);
    }
}

