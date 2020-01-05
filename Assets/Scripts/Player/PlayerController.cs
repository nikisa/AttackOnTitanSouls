﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    //Inspector
    public DataInput dataInput;
    public Animator animator;
    public CharacterController controller;
    public Transform rotationTransform;
    public Transform body;
    public float movimentRatio;
    public float DPS;
    public BossOrbitManager bossOrbitManager;

    //Public
    [HideInInspector]
    public bool newInput;
    [HideInInspector]
    public PlayerDashData playerDashData;
    [HideInInspector]
    public PlayerIdleData playerIdleData;
    [HideInInspector]
    public Vector3 dashDirection;
    [HideInInspector]
    public float InitialDashVelocity;
    [HideInInspector]
    public int layerMask;

    //Private
    Camera camera;
    Vector3 movementVelocity = Vector3.zero;
    float forwardVelocity;
    float timeStart;


    protected virtual void Start() {
        camera = Camera.main;
        
        foreach (var item in animator.GetBehaviours<BaseState>()) {
            item.SetContext(this, animator);
        }
    }

    private void Update() 
    {
        CheckInput();
    }

    void CalculateOrientationFromMouse()
    {
        // point in plane from mouse position
        Vector3 mouse = Input.mousePosition;
        var ray = camera.ScreenPointToRay(mouse);
        float x = ray.origin.x - ray.direction.x * ray.origin.y / ray.direction.y;
        float z = ray.origin.z - ray.direction.z * ray.origin.y / ray.direction.y;
        Vector3 point = new Vector3(x, 0, z);

        var direction = (point - transform.position);
        direction.y = 0;
        if (direction.sqrMagnitude > 0.001f) dataInput.currentOrientation = Quaternion.LookRotation(direction.normalized);
    }


    public void CheckInput() {
        dataInput.Horizontal = Input.GetAxis("Horizontal");
        dataInput.Vertical = Input.GetAxis("Vertical");
        dataInput.HorizontalLook = Input.GetAxis("HorizontalLook");
        dataInput.VerticalLook = Input.GetAxis("VerticalLook");
        dataInput.Dash = Input.GetButtonDown("Dash");

        Vector3 lookVector = Vector3.right * dataInput.HorizontalLook + Vector3.forward * dataInput.VerticalLook;

        if (lookVector.sqrMagnitude < 0.0001f && Input.GetJoystickNames().Length<=0)
        {
            CalculateOrientationFromMouse();
        }
        else
        {
            dataInput.currentOrientation = Quaternion.LookRotation(lookVector.normalized);
        }

    }

    public void DoFreeze(float _timeFreeze, float _rallenty) {
        StartCoroutine(DoFreezeCoroutine(_timeFreeze , _rallenty));
    }

    IEnumerator DoFreezeCoroutine(float _timeFreeze ,float _rallenty) {
        float originalScale = Time.timeScale;
        Time.timeScale = _rallenty;
        yield return new WaitForSecondsRealtime(_timeFreeze);
        Time.timeScale = originalScale;
    }



    public void Movement() {
        float skin = .95f;

        if (movementVelocity.sqrMagnitude < 0.001) return;

        int interpolation = (int)(movementVelocity.magnitude / 1f) + 1;

        for (int i = 0; i < interpolation; i++) {
            if (movementVelocity.sqrMagnitude < 0.001) return;

            float time = Time.deltaTime / interpolation;

            RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 1.5f, skin, movementVelocity, (movementVelocity * time).magnitude, layerMask);

            if (hits == null || hits.Length == 0) {
                transform.Translate(movementVelocity * time);
            }
            else {
                //pushable
                foreach (var hit in hits) {
                    if (hit.transform.gameObject.CompareTag("Pushable")) {
                        var rb = hit.transform.GetComponent<Rigidbody>();
                        rb.AddForceAtPosition(movementVelocity, hit.point, ForceMode.Acceleration);
                    }
                    else if (hits[0].transform.gameObject.CompareTag("BossGraphics")) {
                        SceneManager.LoadScene(2);
                    }

                    //slope
                    if (hits.Length == 1) {
                        var normal = Quaternion.AngleAxis(90, Vector3.up) * hits[0].normal;
                        Debug.DrawRay(hits[0].point, normal * 4, Color.red, 2);

                        movementVelocity = normal * Vector3.Dot(movementVelocity, normal);
                        movementVelocity.y = 0;

                        transform.Translate(movementVelocity * time);
                    }
                }
            }
        }
    }


    public void Deceleration(float _deceleration) {
        forwardVelocity = 0;

        if (movementVelocity.x < _deceleration * Time.deltaTime)
            movementVelocity.x = movementVelocity.x - _deceleration * Time.deltaTime;
        else if (movementVelocity.x > -_deceleration * Time.deltaTime)
            movementVelocity.x = movementVelocity.x + _deceleration * Time.deltaTime;
        else {
            movementVelocity.x = 0;
        }
            if (movementVelocity.z < _deceleration * Time.deltaTime)
                movementVelocity.z = movementVelocity.z - _deceleration * Time.deltaTime;
            else if (movementVelocity.z > -_deceleration * Time.deltaTime)
                movementVelocity.z = movementVelocity.z + _deceleration * Time.deltaTime;
            else {
                movementVelocity.z = 0;
            }
            Movement();
    }

    public void ReadInputKeyboard(DataInput dataInput , float _acceleration , float _maxSpeed) {

    movementVelocity = Vector3.zero;
    // Set vertical movement
    if (dataInput.Vertical != 0f) {
        forwardVelocity += _acceleration * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
        movementVelocity += Vector3.forward * dataInput.Vertical * forwardVelocity;
    }

    // Set horizontal movement
    if (dataInput.Horizontal != 0f) {
        forwardVelocity += _acceleration * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
        movementVelocity += Vector3.right * dataInput.Horizontal * forwardVelocity;
    }
        
    newInput = true;
    
}


    public void ReadInputGamepad(DataInput dataInput, float _acceleration , float _maxSpeed) {

        movementVelocity = Vector3.zero;

        // Set vertical movement gamepad
        if (dataInput.Vertical != 0f) {
            forwardVelocity += _acceleration * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
            movementVelocity += Vector3.forward * dataInput.Vertical * forwardVelocity;
        }

        // Set horizontal movement gamepad
        if (dataInput.Horizontal != 0f) {
            forwardVelocity += _acceleration * Time.deltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
            movementVelocity += Vector3.right * dataInput.Horizontal * forwardVelocity;
        }
        newInput = true;
    }

    public void timeFreeze(float _timeFreeze)
    {
        if (Time.time - timeStart < _timeFreeze)
        {
            Time.timeScale = 1;
        }
    }
    public RaycastHit RayCastDash(float _horizontal , float _vertical)
    {
        RaycastHit hitDash;
        Physics.Raycast(transform.position, new Vector3(_horizontal, 0, _vertical), out hitDash, playerDashData.DashDistance * 2);
        return hitDash;
    }
    public void Dash(float _dashTimeFrames)  // on complete sulla funzione ?
    {
        transform.DOMove(dashDirection, _dashTimeFrames);
    }

    public void PlayerInclination() {

        rotationTransform.localRotation = dataInput.currentOrientation;

        // Ruoto il personaggio in funzione della del suo movimento
        Vector3 rotationAxis = Quaternion.AngleAxis(90, Vector3.up) * movementVelocity;
        Quaternion moveRotation = Quaternion.AngleAxis(movementVelocity.magnitude * movimentRatio, rotationAxis);
        body.rotation = moveRotation * rotationTransform.rotation;
    }


    //?Test player rope length constraint?
    public void RopeLengthConstraint(Vector3 nodePosition) {
        Vector3 constraint = new Vector3(nodePosition.x, nodePosition.y, transform.position.z);
        transform.Translate(constraint * Time.deltaTime);
    }


}

public struct DataInput
{
    public float Horizontal;
    public float HorizontalLook;
    public float Vertical;
    public float VerticalLook;
    public bool Dash;
    public Quaternion currentOrientation;


    public DataInput(float _horizontal, float _vertical, bool _dash, Quaternion _currentRotation) {
        Horizontal = _horizontal;
        Vertical = _vertical;
        Dash = _dash;
        this.currentOrientation = _currentRotation;
        HorizontalLook = 0;
        VerticalLook = 0;
    }

   
}

