using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;
using DG.Tweening;


public class MovementController : Controller
{
    //Inspector
    public float maxSpeed;
    public float framesZeroToMax;
    public float framesMaxToZero;

    public float DashDistance;
    public float DashTimeFrames;
    public Ease DashEase;
    public float ResumeControl;
    public float DashTimeFreeze;

    // Protected
    protected bool isMoving;

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
            isMoving = true;
        }


        // Set horizontal movement
        if (data.axes[1] != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0 , maxSpeed);
            movementVelocity += Vector3.right * data.axes[1]  * forwardVelocity;
            isMoving = true;
        }

        // Set vertical movement gamepad
        if (data.Vertical != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
            movementVelocity += Vector3.forward * data.Vertical * forwardVelocity;
            isMoving = true;
        }

        // Set horizontal movement gamepad
        if (data.Horizontral != 0f) {
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
            movementVelocity += Vector3.right * data.Horizontral * forwardVelocity;
            isMoving = true;
        }


        // Set Dash Action
        if (data.buttons[0] && isMoving) {
            Dash(DashTimeFrames, ResumeControl, DashTimeFreeze, data);
            isMoving = true;
        }

        newInput = true;

    }

    private void LateUpdate() {
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
                //isMoving = false;
            }
                

            if (movementVelocity.z < decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.z > -decelRatePerSec * Time.deltaTime)
                movementVelocity.z = movementVelocity.z + decelRatePerSec * Time.deltaTime;
            else {
                movementVelocity.z = 0;
                //isMoving = false;
            }
                

            //movementVelocity.x = movementVelocity.x < decelRatePerSec * Time.deltaTime ? movementVelocity.x -= (decelRatePerSec * Time.deltaTime) 
            //                                                                           : movementVelocity.x += (decelRatePerSec * Time.deltaTime);
            //movementVelocity.z = movementVelocity.z < decelRatePerSec * Time.deltaTime ? movementVelocity.z -= (decelRatePerSec * Time.deltaTime)
            //                                                                           : movementVelocity.z += (decelRatePerSec * Time.deltaTime);

            rb.velocity = new Vector3(movementVelocity.x , rb.velocity.y, movementVelocity.z);

            Debug.Log(rb.velocity);
            
        }
        
        newInput = false;
    }

    public void Dash(float _DashTimeFrames, float _ResumeControl, float _DashTimeFreeze, InputData _data) {
        _DashTimeFrames = _DashTimeFrames / 60;
        _ResumeControl = ResumeControl / 60;
        _DashTimeFreeze = DashTimeFreeze / 60;

        transform.DOMove(new Vector3((DashDistance * _data.axes[1]) + transform.position.x , transform.position.y, (DashDistance * _data.axes[0]) + transform.position.z), _DashTimeFrames).SetEase(DashEase);
        
        isMoving = false;
        
    }
}
