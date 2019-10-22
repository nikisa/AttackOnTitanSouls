using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState {

    //Inspector
    public float maxSpeed;
    public float framesZeroToMax;
    public float framesMaxToZero;
    public PlayerBaseState DashState;

    // Private
    bool newInput;
    float accelRatePerSec;
    float decelRatePerSec;
    float forwardVelocity;
    Vector3 movementVelocity = Vector3.zero;
    Rigidbody rb;
    //InputData inputData;
    PlayerController playerController;
    DataInput dataInput;

    public void ReadInputKeyboard(DataInput dataInput) {

        movementVelocity = Vector3.zero;

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
        Debug.Log("Enter");
        accelRatePerSec = maxSpeed / (framesZeroToMax / 60);
        decelRatePerSec = -maxSpeed / (framesMaxToZero / 60);
        forwardVelocity = 0f;
        rb = _rb;
        playerController = _playerController;
    }

    public override void Tick() {

        dataInput = playerController.dataInput;

        if (dataInput.Dash) {
            playerController.ChangeState(DashState);
        }

        if (dataInput.Vertical != 0 || dataInput.Horizontal != 0) {
            ReadInputGamepad(dataInput);
            ReadInputKeyboard(dataInput);
        }

        

        if (newInput) {
            rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
        }
        else {
            forwardVelocity = 0;

            if (movementVelocity.x < decelRatePerSec * Time.deltaTime)
                movementVelocity.x = movementVelocity.x - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.x > -decelRatePerSec * Time.deltaTime)
                movementVelocity.x = movementVelocity.x + decelRatePerSec * Time.deltaTime;
            else {
                movementVelocity.x = 0;
            }


            if (movementVelocity.z < decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.z > -decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z + decelRatePerSec * Time.deltaTime;
            else {
                movementVelocity.z = 0;
            }

            rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
            Debug.Log(rb.velocity);

        }

        newInput = false;
    }

    public override void Exit() {
        
    }

}

