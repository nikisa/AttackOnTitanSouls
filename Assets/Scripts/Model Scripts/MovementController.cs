using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;

public class MovementController : Controller
{
    //Inspector
    public float maxSpeed;
    public float framesZeroToMax;
    public float framesMaxToZero;


    // Private
    float accelRatePerSec;
    float decelRatePerSec;
    float forwardVelocity;

    private void Start() {
        accelRatePerSec = maxSpeed / (framesZeroToMax / 60);
        decelRatePerSec = -maxSpeed / (framesMaxToZero / 60);
        forwardVelocity = 0f;
        Controller controller = GetComponent<Controller>();
    }

    Vector3 movementVelocity = Vector3.zero;

    public override void ReadInput(InputData data) {

        movementVelocity = Vector3.zero;

        // Set vertical movement
        if (data.axes[0] != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
            movementVelocity += Vector3.forward * data.axes[0] * forwardVelocity;
        }


        // Set horizontal movement
        if (data.axes[1] != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0 , maxSpeed);
            movementVelocity += Vector3.right * data.axes[1]  * forwardVelocity;
        }

        // Set vertical movement gamepad
        if (data.Vertical != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
            movementVelocity += Vector3.forward * data.Vertical * forwardVelocity;
        }

        // Set horizontal movement gamepad
        if (data.Horizontral != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
            movementVelocity += Vector3.right * data.Horizontral * forwardVelocity;
        }

        newInput = true;

    }

    private void LateUpdate() {
        if (newInput) {
            rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
            
        }
        else {
            forwardVelocity = 0;

            //decelRatePerSec * Time.deltaTime instead of 0?

            if (movementVelocity.x < decelRatePerSec * Time.deltaTime)
                movementVelocity.x = movementVelocity.x - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.x > -decelRatePerSec * Time.deltaTime)
                movementVelocity.x = movementVelocity.x + decelRatePerSec * Time.deltaTime;
            else
                movementVelocity.x = 0;

            if (movementVelocity.z < decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.z > -decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z + decelRatePerSec * Time.deltaTime;
            else
                movementVelocity.z = 0;

            //movementVelocity.x = movementVelocity.x < decelRatePerSec * Time.deltaTime ? movementVelocity.x -= (decelRatePerSec * Time.deltaTime) 
            //                                                                           : movementVelocity.x += (decelRatePerSec * Time.deltaTime);
            //movementVelocity.z = movementVelocity.z < decelRatePerSec * Time.deltaTime ? movementVelocity.z -= (decelRatePerSec * Time.deltaTime)
            //                                                                           : movementVelocity.z += (decelRatePerSec * Time.deltaTime);

            rb.velocity = new Vector3(movementVelocity.x , rb.velocity.y, movementVelocity.z);

            Debug.Log(rb.velocity);
            
        }
        
        newInput = false;
    }
}
