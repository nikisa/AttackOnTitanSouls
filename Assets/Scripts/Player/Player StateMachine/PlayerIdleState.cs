using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerIdleState : PlayerBaseState {

    //Inspector
    public PlayerIdleData playerIdleData;


    // Private
    bool newInput;
    bool canDash = true;    
    float accelRatePerSec;
    float decelRatePerSec;
    float forwardVelocity;
    float startDash;
    float startTime;
    float boostTime;
    float timer;
    int layerMask;
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
        layerMask = 1 << 10 | 1<<12;
        timer = Time.time;

        accelRatePerSec = playerIdleData.maxSpeed / (playerIdleData.framesZeroToMax / 60);
        decelRatePerSec = -playerIdleData.maxSpeed / (playerIdleData.framesMaxToZero / 60);
        forwardVelocity = 0f;
    }

    public override void Tick() {

        //Debug.LogFormat("Horizontal: {0} ||| Vertical: {1}", player.dataInput.Horizontal, player.dataInput.Vertical);

        if (Time.time < timer + playerIdleData.resumeControl) {
            canDash = false;
        }
        else {
            canDash = true;
        }

        dataInput = player.dataInput;

        player.rotationTransform.localRotation = dataInput.currentOrientation;

        // ruoto il personaggio in funzione della del suo movimento
        Vector3 rotationAxis = Quaternion.AngleAxis(90,Vector3.up)* movementVelocity;
        Quaternion moveRotation = Quaternion.AngleAxis(movementVelocity.magnitude * player.movimentRatio, rotationAxis);
        player.body.rotation = moveRotation * player.rotationTransform.rotation;

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


            Movement();
        }
        else {
            if (movementVelocity.y < decelRatePerSec * Time.deltaTime)
                movementVelocity.y = movementVelocity.z - decelRatePerSec * Time.deltaTime;
            else if (movementVelocity.y > -decelRatePerSec * Time.deltaTime)
                movementVelocity.y = movementVelocity.z + decelRatePerSec * Time.deltaTime;
            else {
                movementVelocity.y = 0;
            }

            Movement();

        }

    }

    public void Movement() 
    {
        float skin = .5f;
        float softSkin = 0.01f;

        if (movementVelocity.sqrMagnitude < 0.001) return;

        int interpolation = (int)(movementVelocity.magnitude/1f) +1;

        for (int i = 0; i < interpolation; i++)
        {
            if (movementVelocity.sqrMagnitude < 0.001) return;

            float time = Time.deltaTime / interpolation;

            RaycastHit[] hits = Physics.SphereCastAll(player.transform.position + Vector3.up * 1.1f, skin, movementVelocity, (movementVelocity * time).magnitude , layerMask);

            if (hits == null || hits.Length == 0) {
                player.transform.Translate(movementVelocity * time);
            }
        else {
                //pushable
                foreach (var hit in hits) {
                    if (hit.transform.gameObject.CompareTag("Pushable")) {
                        var rb = hit.transform.GetComponent<Rigidbody>();
                        rb.AddForceAtPosition(movementVelocity, hit.point, ForceMode.Acceleration);
                        //Debug.LogFormat("Impact:{0}", movementVelocity);
                    }
                    else if (hits[0].transform.gameObject.CompareTag("BossGraphics")) {
                        Debug.Log("CAVOLFIORE");
                        SceneManager.LoadScene(2);

                    }

                    //slope
                    if (hits.Length == 1) {
                        var normal = Quaternion.AngleAxis(90, Vector3.up) * hits[0].normal;
                        Debug.DrawRay(hits[0].point, normal * 4, Color.red, 2);

                        movementVelocity = normal * Vector3.Dot(movementVelocity, normal);
                        movementVelocity.y = 0;

                        player.transform.Translate(movementVelocity * time);
                        //Verlet movement
                    }

                }

            }

                //player.transform.Translate(movementVelocity.normalized * (hit.distance- softSkin));
                //Debug.Log(hit.transform.gameObject.name, hit.transform.gameObject);
           
        }


        
        //player.controller.Move(movementVelocity * Time.deltaTime + Vector3.down);

        /*
        if (!playerIdleData.isHookTest) {
            player.transform.Translate(movementVelocity * Time.deltaTime);
        }
        else {
            player.transform.Translate(movementVelocity * Time.deltaTime);
        }
        */
    }


    //?Test player rope length constraint?
    public void RopeLengthConstraint(Vector3 nodePosition) {
        Vector3 constraint = new Vector3(nodePosition.x, nodePosition.y, player.transform.position.z);
        player.transform.Translate(constraint * Time.deltaTime);
    }
}

