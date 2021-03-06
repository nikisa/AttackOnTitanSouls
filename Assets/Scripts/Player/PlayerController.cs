﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;
using DG.Tweening;
using UnityEngine.SceneManagement;



public class PlayerController : MovementBase
{
    //Events
    #region Events
    public delegate void GameEvent();
    public static GameEvent DeathEvent;
    public static GameEvent VictoryEvent;
    public static GameEvent TimerEvent;
    public static GameEvent DmgEvent;
    public static GameEvent DisableInputEvent;
    

    private void OnEnable() {
        DeathEvent += PlayerDeath;
        VictoryEvent += Victory;
        TimerEvent += StartTimerDash;
        DmgEvent += TakeDmg;
        DisableInputEvent += DisableInputCall;
    }

    private void OnDisable() {
        DeathEvent -= PlayerDeath;
        VictoryEvent -= Victory;
        TimerEvent -= StartTimerDash;
        DmgEvent -= TakeDmg;
        DisableInputEvent -= DisableInputCall;
    }
    #endregion

    //Inspector
    #region Inspector Variables
    public DataInput dataInput;
    public Animator animator;
    public Animator graphicAnimator;
    public Transform rotationTransform;
    public GameObject body;
    public float movementRatio;
    public float movementAnimationDumpTime;
    public float DPS;
    public TargetType playerTarget;
    public UiManager uiManager;
    public float TimeInputDisable;
    [Range(0, 1)]
    public float MovementDeadZoneValue;
    [Range(0, 1)]
    public float AimDeadZoneValue;
    public float TweeningRotationTime;
    public Ease TweeningRotationEase;
    public GameObject PauseCanvas;
    #endregion

    //Public
    #region public variables
    [HideInInspector]
    public bool canDash;
    [HideInInspector]
    public float plusDeltaTime;
    [HideInInspector]
    public PlayerDashData playerDashData;
    [HideInInspector]
    public PlayerMovementData playerMovementData;
    [HideInInspector]
    public PlayerDecelInTimeData playerDecelInTimeData;
    [HideInInspector]
    public Vector3 dashDirection;
    [HideInInspector]
    public float InitialDashVelocity;
    [HideInInspector]
    public int layerMask;
    [HideInInspector]
    public float skin = .95f;
    [HideInInspector]
    public float dashVelocityModule;
    [HideInInspector]
    public float horizontalDash;
    [HideInInspector]
    public float verticalDash;
    [HideInInspector]
    public float timerDash;
    [HideInInspector]
    public bool IsImmortal;
    [HideInInspector]
    public bool ImmortalTutorial;

    public int Lifes;
    [HideInInspector]
    public float forwardVelocity;
    [HideInInspector]
    public bool InputDisable;
    [HideInInspector]
    public Vector3 MoveDirection;
    #endregion

    //Private
    Camera camera;
    Vector3 move;
    bool isPaused;


    protected virtual void Awake() {
        playerTarget.instance = this.gameObject;
        isPaused = false;

        foreach (var item in animator.GetBehaviours<PlayerBaseState>()) {
            item.SetContext(this, animator, graphicAnimator);
        }
    }

    protected virtual void Start() {
        canDash = true;
        camera = Camera.main;
    }

    private void Update() 
    {

        Debug.DrawRay(transform.position , VelocityVector, Color.blue, .02f );
        Debug.DrawRay(transform.position , AccelerationVector, Color.red, .02f);

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) {
            isPaused = !isPaused;

            if (isPaused) {
                PauseCanvas.SetActive(true);
                Time.timeScale = 0;
            }
            else {
                PauseCanvas.SetActive(false);
                Time.timeScale = 1;
            }
        }


        if (!InputDisable) // momentaneo da sistemare
        {
            CheckInput();
            UpdateOriantation();
            SetAnimationParameter();
        }



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

    public void SetAnimationParameter() {

        MoveDirection = new Vector3(Input.GetAxis("Horizontal") , 0, Input.GetAxis("Vertical")).normalized;

        MoveDirection = body.transform.InverseTransformDirection(MoveDirection);

        graphicAnimator.SetFloat("Horizontal", MoveDirection.x , movementAnimationDumpTime, Time.deltaTime);
        graphicAnimator.SetFloat("Vertical", MoveDirection.z , movementAnimationDumpTime, Time.deltaTime);
    }


    public void CheckInput() {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalRotation = Input.GetAxis("HorizontalLook");
        float verticalRotation = Input.GetAxis("VerticalLook");

        if (Mathf.Pow(horizontalRotation, 2) + Mathf.Pow(verticalRotation, 2) >= Mathf.Pow(AimDeadZoneValue, 2)) {
            dataInput.HorizontalLook = Input.GetAxis("HorizontalLook");
            dataInput.VerticalLook = Input.GetAxis("VerticalLook");
        }

        if (Mathf.Pow(horizontalMovement, 2) + Mathf.Pow(verticalMovement, 2) >= Mathf.Pow(MovementDeadZoneValue, 2)) {

            dataInput.Horizontal = Input.GetAxis("Horizontal");
            dataInput.Vertical = Input.GetAxis("Vertical");
        }
        else {
            dataInput.Horizontal = 0;
            dataInput.Vertical = 0;
        }


        dataInput.Dash = Input.GetButtonDown("Dash");

        CalculateOrientationFromMouse();//Da rimuovere e tenere solo nell'if sotto


        Vector3 lookVector = Vector3.right * dataInput.HorizontalLook + Vector3.forward * dataInput.VerticalLook;


        if (lookVector.sqrMagnitude < 0.0001f && Input.GetJoystickNames().Length <= 0)
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

    public void Dash(float _dashVelocityModule , Vector3 _targetDir , AnimationCurve _dashCurve , float _t0 , float _t1 , int _iterations) {
        targetDir = _targetDir;
        Vector3 dashVectorTemp = targetDir;
        VelocityVector = dashVectorTemp.normalized * _dashVelocityModule;
        move = dashVectorTemp.normalized * Integration.IntegrateCurve(_dashCurve , _t0, _t1 , _iterations);
        CharacterController.Move(move);
    }

    //Here and not in BaseMovement because it could change over time
    public void DashDeceleration(AnimationCurve _dashDecelCurve, float _t0 , float _t1, int _iterations) {
        float dashSpeedModule = playerDashData.ActiveDashDistance / playerDashData.ActiveDashTime;
        float vectorAngle = Vector3.SignedAngle(Vector3.forward, VelocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
        DecelerationVector = new Vector3(Mathf.Sin(vectorAngle) * DecelerationModule, 0, Mathf.Cos(vectorAngle) * DecelerationModule);
        VelocityVector = _dashDecelCurve.Evaluate(_t1) * DecelerationVector.normalized;
        move = DecelerationVector.normalized * Integration.IntegrateCurve(_dashDecelCurve, _t0, _t1, _iterations);
        CharacterController.Move(move);
    }


    public void TakeDmg()
    {
        Lifes--;
        uiManager.LifeUpdate(Lifes);
        
        if (Lifes == 0)
        {
            PlayerDeath();
        }
        else
        {            
            StartCoroutine(InvicibleSecond(2f));
        }
    }


    public void DisableInputCall()
    {
        StartCoroutine(InputDisableCourutine());
    }


    public IEnumerator InputDisableCourutine()
    {
        InputDisable = true;
        yield return new WaitForSeconds(TimeInputDisable);
        InputDisable = false;
    }
    

    public void SetDashVelocity(float _dashDistance, float _dashTime) {
        float dashVelocity = _dashDistance / _dashTime;
        dashVelocityModule = dashVelocity;
    }


    public bool checkDeadZone() {
        if (Mathf.Pow(Input.GetAxis("Horizontal"), 2) + Mathf.Pow(Input.GetAxis("Vertical"), 2) >= Mathf.Pow(MovementDeadZoneValue, 2)) {
            return true;
        }
        else {
            return false;
        }
    }


    public void PlayerInclination() {
        // Ruoto il personaggio in funzione della del suo movimento
        Vector3 rotationAxis = Quaternion.AngleAxis(90, Vector3.up) * VelocityVector;
        Quaternion moveRotation = Quaternion.AngleAxis(VelocityVector.magnitude * movementRatio, rotationAxis);
        body.transform.rotation = moveRotation * rotationTransform.rotation;
    }

    public void UpdateOriantation()
    {
        rotationTransform.DOLocalRotateQuaternion(dataInput.currentOrientation, TweeningRotationTime).SetEase(TweeningRotationEase);
    }


    public void StopPlayer()
    {
        forwardVelocity = 0;
        VelocityVector = Vector3.zero;
    }

    public void StartTimerDash()
    {
        timerDash = Time.time;
    }

    public void PlayerDeath() {
        Debug.Log("MORTO");
        SceneManager.LoadScene(2);
    }

    public void Victory() {
        SceneManager.LoadScene(3);
    }

    
    public IEnumerator InvicibleSecond(float _sec)
    {
        IsImmortal = true;
        #region Immortality Flickering
        body.SetActive(false);
        yield return new WaitForSeconds(_sec / 6);
        body.SetActive(true);
        yield return new WaitForSeconds(_sec / 6);
        body.SetActive(false);
        yield return new WaitForSeconds(_sec / 6);
        body.SetActive(true);
        yield return new WaitForSeconds(_sec / 6);
        body.SetActive(false);
        yield return new WaitForSeconds(_sec / 6);
        body.SetActive(true);
        yield return new WaitForSeconds(_sec / 6);
        IsImmortal = false;
        #endregion
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {

        if ((hit.collider.GetComponent<MovementBase>() || hit.collider.GetComponent<FirstBossMask>()) && !hit.collider.GetComponent<PlayerController>()) {
            BounceMovement(hit.collider);
            Debug.Log("HHHHH");
            GetDamage();
                

            // VelocityVector = Mathf.Clamp(VelocityVector.magnitude, MinBounceVector,MaxBounceVector) * VelocityVector.normalized;
            animator.SetTrigger("Stunned");
        }
    }


    public void GetDamage() {
        if (!IsImmortal) {
            PlayerController.DmgEvent();
        }
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


    #region FUNCTIONS CEMETERY

    //public RaycastHit RayCastDash(float _horizontal, float _vertical) {
    //    RaycastHit hitDash;
    //    Physics.Raycast(transform.position, new Vector3(_horizontal, 0, _vertical), out hitDash, playerDashData.ActiveDashDistance * 2);
    //    return hitDash;
    //}

    //public void ReadInputKeyboard(DataInput dataInput, float _acceleration, float _maxSpeed) {
    //    movementVelocity = Vector3.zero;

    //    // Set vertical movement
    //    //if (dataInput.Vertical != 0f) {
    //    forwardVelocity += _acceleration * Time.fixedDeltaTime;
    //    forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
    //    movementVelocity += Vector3.forward * (forwardVelocity * Mathf.Sin(GetLeftAnalogAngle()));
    //    movementDirection = movementVelocity;
    //    //}

    //    // Set horizontal movement
    //    //if (dataInput.Horizontal != 0f) {
    //    forwardVelocity += _acceleration * Time.fixedDeltaTime;
    //    forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
    //    movementVelocity += Vector3.right * (forwardVelocity * Mathf.Cos(GetLeftAnalogAngle()));
    //    movementDirection = movementVelocity;
    //    //}
    //}


    //public void DashDeceleration(float _horizontal, float _vertical, float _decelerationTime, float _dashDistance, float _dashTime) {

    //    Vector3 direction = new Vector3(_horizontal, 0, _vertical);
    //    RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 1.75f, skin * 1.5f, dashVelocityModule * direction, (dashDecelerationVelocity * Time.deltaTime), layerMask);

    //    if (hits == null || hits.Length == 0) {

    //        dashDecelerationVelocity = _dashDistance / _dashTime;
    //        dashDeceleration = dashDecelerationVelocity / _decelerationTime;
    //        dashVelocityModule -= dashDeceleration * Time.fixedDeltaTime;
    //        dashVelocityModule = Mathf.Clamp(dashVelocityModule, 0, dashDecelerationVelocity);
    //        transform.Translate((dashVelocityModule * Time.fixedDeltaTime) * direction);
    //    }
    //    else {
    //        dashVelocityModule = 0;
    //    }
    //}


    //public void Deceleration(float _deceleration) {

    //    if (movementVelocity.x < _deceleration * Time.deltaTime)
    //        movementVelocity.x = movementVelocity.x - _deceleration * Time.deltaTime;
    //    else if (movementVelocity.x > -_deceleration * Time.deltaTime)
    //        movementVelocity.x = movementVelocity.x + _deceleration * Time.deltaTime;
    //    else {
    //        movementVelocity.x = 0;
    //    }
    //    if (movementVelocity.z < _deceleration * Time.deltaTime)
    //        movementVelocity.z = movementVelocity.z - _deceleration * Time.deltaTime;
    //    else if (movementVelocity.z > -_deceleration * Time.deltaTime)
    //        movementVelocity.z = movementVelocity.z + _deceleration * Time.deltaTime;
    //    else {
    //        movementVelocity.z = 0;
    //    }

    //    movementDirection = movementVelocity;
    //    Movement();

    //}

    //public void Movement() {

    //    if (movementVelocity.sqrMagnitude < 0.001) return;

    //    int interpolation = (int)(movementVelocity.magnitude / 1f) + 1;

    //    for (int i = 0; i < interpolation; i++) {
    //        if (movementVelocity.sqrMagnitude < 0.001) return;

    //        float time = Time.deltaTime / interpolation;

    //        RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 1.5f, skin, movementDirection, (movementDirection * time).magnitude, layerMask);


    //        if (hits == null || hits.Length == 0) {
    //            transform.Translate(movementVelocity * time);
    //            animator.SetBool("isColliding", false);
    //        }
    //        else {

    //            character.Move(movementVelocity * time);

    //            #region Slope made by code
    //            //slope
    //            //if (hits.Length >= 1) {

    //            //    animator.SetBool("isColliding", true);
    //            //    animator.ResetTrigger("Dash");

    //            //    var normal = Quaternion.AngleAxis(90, Vector3.up) * hits[0].normal;
    //            //    Debug.DrawRay(hits[0].point, normal * 4, Color.red, 2);

    //            //    movementVelocity = normal * Vector3.Dot(movementVelocity, normal);
    //            //    movementVelocity.y = 0;

    //            //    transform.Translate(movementVelocity * time);
    //            //}
    //            #endregion

    //        }
    //    }
    //}

    #endregion

}

