using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState {

    //ButtonDown:
    //startTime = Time.time

    //ButtonUp:
    //Time.Time - startTime = Tempo Boost
    //if(tempoBoost > x)
    //	ChangeState(BoostState)
    //else
    //	ChangeState(DashState)
    //startTime = 0

    //Tick() :
    //if(Time.Time - startTime > x)
    //	Animation del Boost(caricamento barra)

    //Inspector
    public bool isHookTest;
    public float maxSpeed;
    public float framesZeroToMax;
    public float framesMaxToZero;
    public PlayerBaseState DashState;
    public PlayerBaseState BoostState;
    public float resumeControl;

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
    Rigidbody rb;
    //InputData inputData;
    PlayerController playerController;
    DataInput dataInput;

    
    public void ReadInputKeyboard(DataInput dataInput) {

        movementVelocity = Vector3.zero;

        if (!isHookTest) {
            // Set vertical movement
            if (dataInput.Vertical != 0f) {
                forwardVelocity += accelRatePerSec * Time.deltaTime;
                forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
                movementVelocity += Vector3.forward * dataInput.Vertical * forwardVelocity;
            }

            // Set horizontal movement
            if (dataInput.Horizontal != 0f) {
                forwardVelocity += accelRatePerSec * Time.deltaTime;
                forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
                movementVelocity += Vector3.right * dataInput.Horizontal * forwardVelocity;
            }
        }
        else {
            // Set vertical movement
            if (dataInput.Vertical != 0f) {
                forwardVelocity += accelRatePerSec * Time.deltaTime;
                forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
                movementVelocity += Vector3.up * dataInput.Vertical * forwardVelocity;
            }

            // Set horizontal movement
            if (dataInput.Horizontal != 0f) {
                forwardVelocity += accelRatePerSec * Time.deltaTime;
                forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
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
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
            movementVelocity += Vector3.forward * dataInput.Vertical * forwardVelocity;
        }

        // Set horizontal movement gamepad
        if (dataInput.Horizontal != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
            movementVelocity += Vector3.right * dataInput.Horizontal * forwardVelocity;
        }

        newInput = true;
    }

    public override void Enter(PlayerController _playerController, Rigidbody _rb) {

        accelRatePerSec = maxSpeed / (framesZeroToMax / 60);
        decelRatePerSec = -maxSpeed / (framesMaxToZero / 60);
        forwardVelocity = 0f;
        rb = _rb;
        playerController = _playerController;
    }

    public override void Tick() {

        //ButtonDown:
        //startTime = Time.time

        //ButtonUp:
        //Time.Time - startTime = Tempo Boost
        //if(tempoBoost > x)
        //	ChangeState(BoostState)
        //else
        //	ChangeState(DashState)
        //startTime = 0

        //Tick() :
        //if(Time.Time - startTime > x)
        //	Animation del Boost(caricamento barra)

        dataInput = playerController.dataInput;

        //if (dataInput.DashDown) {
        //    startTime = Time.time;
        //}
        //else if (dataInput.DashUp) {
        //    boostTime = Time.time - startTime;
        //    if (boostTime > 0.5f) {
        //        playerController.ChangeState(BoostState);
        //    }
        //    else if(boostTime <= 0.5f && canDash) {
        //        startDash = Time.time;
        //        canDash = false;
        //        playerController.ChangeState(DashState);
        //    }
        //}

        //if (Time.time - startTime > 0.5f) {
        //    //playAnimation
        //}


        if (dataInput.Dash && canDash)
        {
            startDash = Time.time;
            canDash = false;
            playerController.ChangeState(DashState);
        }

        if ((Time.time - startDash) > resumeControl) {
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

        if (!isHookTest) {
            if (movementVelocity.z < decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.z > -decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z + decelRatePerSec * Time.deltaTime;
            else {
                movementVelocity.z = 0;
            }

            rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);

        }
        else {
            if (movementVelocity.y < decelRatePerSec * Time.deltaTime)
                movementVelocity.y = movementVelocity.z - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.y > -decelRatePerSec * Time.deltaTime)
                movementVelocity.y = movementVelocity.z + decelRatePerSec * Time.deltaTime;
            else {
                movementVelocity.y = 0;
            }

            rb.velocity = new Vector3(movementVelocity.x, movementVelocity.y ,rb.velocity.z);

        }

    }

    public void Movement() {
        if (!isHookTest) {
            rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
        }
        else {
            rb.velocity = new Vector3(movementVelocity.x, -movementVelocity.y , rb.velocity.z);
        }
    }


    //?Test player rope length constraint?
    public void RopeLengthConstraint(Vector3 nodePosition) {
        rb.velocity = new Vector3(nodePosition.x ,nodePosition.y, rb.velocity.z);
    }
}

