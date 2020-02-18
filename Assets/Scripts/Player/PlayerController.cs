using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    //Events
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

    //Inspector
    public DataInput dataInput;
    public Animator animator;
    public Animator graphicAnimator;
    public CharacterController controller;
    public Transform rotationTransform;
    public GameObject body;
    public float movimentRatio;
    public float DPS;
    public BossOrbitManager bossOrbitManager;
    public TargetType playerTarget;
    public UiManager uiManager;
    public float TimeInputDisable;
    [Range(0, 1)]
    public float DeadZoneValue;


    //Public
    [HideInInspector]
    public bool canDash;
    [HideInInspector]
    public bool newInput;
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
    //[HideInInspector]
    public float forwardVelocity;
    [HideInInspector]
    public bool InputDisable;
    [HideInInspector]
    public Vector3 targetDir;

    //Private
    Vector3 targetDirection;
    Quaternion bossRotation;
    public float vectorAngle;
    public Vector3 OldPos;
    public Vector3 Inertia;
    public float AccelerationModule;
    public Vector3 AccelerationVector;
    public Vector3 VelocityVector;
    public Vector3 MaxSpeedVector;
    public float Drag;
    public float DecelerationModule;
    public Vector3 DecelerationVector;


    Camera camera;
    [HideInInspector]
    public Vector3 movementVelocity = Vector3.zero;
    [HideInInspector]
    public Vector3 movementDirection = Vector3.zero;

    float timeStart;
    float dashDecelerationVelocity;
    float dashDeceleration;
    #region testing

    CharacterController character; 
    #endregion

    //sghigna
    float minVelocity;

    protected virtual void Awake() {
        playerTarget.instance = this.gameObject;
        character = GetComponent<CharacterController>();
    }

    protected virtual void Start() {
      
        canDash = true;
        camera = Camera.main;
        
        foreach (var item in animator.GetBehaviours<PlayerBaseState>()) {
            item.SetContext(this, animator,  graphicAnimator);
        }
    }

    private void Update() 
    {
        if (!InputDisable) // momentaneo da sistemare
        {
            
            CheckInput();
            InputDetection();
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
        graphicAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        graphicAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    public float GetLeftAnalogAngle() {
        return (Vector2.SignedAngle(new Vector2(1, 0), new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) * Mathf.Deg2Rad);
    }

    public void CheckInput() {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalRotation = Input.GetAxis("HorizontalLook");
        float verticalRotation = Input.GetAxis("VerticalLook");

        if (Mathf.Pow(horizontalRotation, 2) + Mathf.Pow(verticalRotation, 2) >= Mathf.Pow(DeadZoneValue, 2)) {
            dataInput.HorizontalLook = Input.GetAxis("HorizontalLook");
            dataInput.VerticalLook = Input.GetAxis("VerticalLook");
        }

        if (Mathf.Pow(horizontalMovement, 2) + Mathf.Pow(verticalMovement, 2) >= Mathf.Pow(DeadZoneValue, 2)) {

            dataInput.Horizontal = Input.GetAxis("Horizontal");
            dataInput.Vertical = Input.GetAxis("Vertical");
        }
        else {
            dataInput.Horizontal = 0;
            dataInput.Vertical = 0;
        }


        dataInput.Dash = Input.GetButtonDown("Dash");

        Vector3 lookVector = Vector3.right * dataInput.HorizontalLook + Vector3.forward * dataInput.VerticalLook;

       CalculateOrientationFromMouse();
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


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("HookPoint")) {
            if (!IsImmortal)
            {
                PlayerController.DmgEvent();
            }
        }
    }


    public void Movement() {

        if (movementVelocity.sqrMagnitude < 0.001) return;

        int interpolation = (int)(movementVelocity.magnitude / 1f) + 1;

        for (int i = 0; i < interpolation; i++) {
            if (movementVelocity.sqrMagnitude < 0.001) return;

            float time = Time.deltaTime / interpolation;

            RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 1.5f, skin, movementDirection, (movementDirection * time).magnitude, layerMask);


            if (hits == null || hits.Length == 0) {
                transform.Translate(movementVelocity * time);
                animator.SetBool("isColliding", false);
            }
            else {

                character.Move(movementVelocity * time);

                //pushable
                foreach (var hit in hits) {
                    //    if (hit.transform.gameObject.CompareTag("Pushable")) {
                    //        var rb = hit.transform.GetComponent<Rigidbody>();
                    //        rb.AddForceAtPosition(movementVelocity, hit.point, ForceMode.Acceleration);
                    //    }

                    //slope
                    //if (hits.Length >= 1) {

                    //    animator.SetBool("isColliding", true);
                    //    animator.ResetTrigger("Dash");

                    //    var normal = Quaternion.AngleAxis(90, Vector3.up) * hits[0].normal;
                    //    Debug.DrawRay(hits[0].point, normal * 4, Color.red, 2);

                    //    movementVelocity = normal * Vector3.Dot(movementVelocity, normal);
                    //    movementVelocity.y = 0;

                    //    transform.Translate(movementVelocity * time);
                    //}
                }
            }
        }
    }

    public void newMovement(float _maxSpeed , float _timeAcceleration) {
        targetDir = new Vector3(dataInput.Horizontal,0,dataInput.Vertical);
        vectorAngle = Vector3.SignedAngle(Vector3.forward, targetDir, Vector3.up) * Mathf.Deg2Rad;
        AccelerationModule = _maxSpeed / _timeAcceleration;
        Drag = AccelerationModule / _maxSpeed * Time.deltaTime;
        VelocityVector -= VelocityVector * Drag;
        AccelerationVector = new Vector3(Mathf.Sin(vectorAngle) * AccelerationModule, 0, Mathf.Cos(vectorAngle) * AccelerationModule);
        transform.localPosition += VelocityVector * Time.deltaTime + 0.5f * AccelerationVector * Mathf.Pow(Time.deltaTime, 2);
        VelocityVector += AccelerationVector * Time.deltaTime;
    }


    public void newDeceleration() {
        vectorAngle = Vector3.SignedAngle(Vector3.forward, VelocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
        DecelerationVector = new Vector3(Mathf.Sin(vectorAngle) * DecelerationModule, 0, Mathf.Cos(vectorAngle) * DecelerationModule);
        VelocityVector -= DecelerationVector * Time.deltaTime;
        transform.localPosition += VelocityVector * Time.deltaTime;
    }

    public void newDash(float _dashVelocityModule , Vector3 _targetDir) {

        vectorAngle = Vector3.SignedAngle(Vector3.forward, _targetDir, Vector3.up) * Mathf.Deg2Rad;
        VelocityVector =  new Vector3(Mathf.Sin(vectorAngle) * (_dashVelocityModule) , 0 , Mathf.Cos(vectorAngle) * (_dashVelocityModule));
        transform.localPosition += VelocityVector * Time.deltaTime;

        Debug.Log("(DASH) VelocityVector: " + VelocityVector.normalized);
    }

    public void newDashDeceleration() {

        Debug.Log("(DASH DECEL) VelocityVector: " + VelocityVector.magnitude);

        vectorAngle = Vector3.SignedAngle(Vector3.forward, VelocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
        DecelerationVector = new Vector3(Mathf.Sin(vectorAngle) * DecelerationModule, 0, Mathf.Cos(vectorAngle) * DecelerationModule);
        VelocityVector -= DecelerationVector * Time.deltaTime;
        transform.localPosition += VelocityVector * Time.deltaTime;

        Debug.Log("(DASH DECEL) DecelerationVector: " + DecelerationVector.magnitude * Time.deltaTime);
    }


    public void Deceleration(float _deceleration) {

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

        movementDirection = movementVelocity;
        Movement();
        
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
    public void DashDeceleration(float _horizontal , float _vertical ,float _decelerationTime , float _dashDistance , float _dashTime) {

        Vector3 direction = new Vector3(_horizontal, 0, _vertical);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 1.75f, skin * 1.5f , dashVelocityModule * direction , (dashDecelerationVelocity * Time.deltaTime) , layerMask);

        if (hits == null || hits.Length == 0) {
            
            dashDecelerationVelocity = _dashDistance / _dashTime;
            //dashDecelerationVelocity /=  _decelerationTime; 
            dashDeceleration = dashDecelerationVelocity / _decelerationTime;
            dashVelocityModule -= dashDeceleration * Time.fixedDeltaTime;
            dashVelocityModule = Mathf.Clamp(dashVelocityModule, 0, dashDecelerationVelocity);
            transform.Translate((dashVelocityModule * Time.fixedDeltaTime) * direction);
        }
        else {
            dashVelocityModule = 0;
        }
    }



    public void SetDashVelocity(float _dashDistance, float _dashTime) {
        float dashVelocity = _dashDistance / _dashTime;
        dashVelocityModule = dashVelocity;
    }



    public float SetCorrectAccelerationOnResumeControl(float startVelocity) {
        return startVelocity;
    }


    public void ReadInputKeyboard(DataInput dataInput , float _acceleration , float _maxSpeed) {
        movementVelocity = Vector3.zero;
       
        // Set vertical movement
        //if (dataInput.Vertical != 0f) {
            forwardVelocity += _acceleration * Time.fixedDeltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
            movementVelocity += Vector3.forward  * (forwardVelocity * Mathf.Sin(GetLeftAnalogAngle()));
            movementDirection = movementVelocity;
        //}
        
        // Set horizontal movement
        //if (dataInput.Horizontal != 0f) {
            forwardVelocity += _acceleration * Time.fixedDeltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
            movementVelocity += Vector3.right * (forwardVelocity * Mathf.Cos(GetLeftAnalogAngle()));
            movementDirection = movementVelocity;
        //}
    }

    public void ReadInputGamepad(DataInput dataInput, float _acceleration, float _maxSpeed) {

        movementVelocity = Vector3.zero;

        // Set vertical movement gamepad
        if (dataInput.Vertical != 0f) {
            forwardVelocity += _acceleration * Time.fixedDeltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
            movementVelocity += Vector3.forward /* * dataInput.Vertical*/ * (forwardVelocity * Mathf.Sin(GetLeftAnalogAngle()));
        }

        // Set horizontal movement gamepad
        if (dataInput.Horizontal != 0f) {
            forwardVelocity += _acceleration * Time.fixedDeltaTime;
            forwardVelocity = Mathf.Clamp(forwardVelocity, 0, _maxSpeed);
            movementVelocity += Vector3.right /** dataInput.Horizontal*/ * (forwardVelocity * Mathf.Cos(GetLeftAnalogAngle()));
        }
        //newInput = true;
    }

    public void InputDetection() {
        if ((dataInput.Vertical > DeadZoneValue || dataInput.Vertical < -DeadZoneValue) || (dataInput.Horizontal > DeadZoneValue || dataInput.Horizontal < -DeadZoneValue)) {
            newInput = true;
        }
        else if((dataInput.Vertical < DeadZoneValue || dataInput.Vertical > -DeadZoneValue) || (dataInput.Horizontal < DeadZoneValue || dataInput.Horizontal > -DeadZoneValue)) {
            newInput = false;
        }
    }

    

    public void timeFreeze(float _timeScale)
    {
        Time.timeScale = _timeScale;
    }
    public RaycastHit RayCastDash(float _horizontal , float _vertical)
    {
        RaycastHit hitDash;
        Physics.Raycast(transform.position, new Vector3(_horizontal, 0, _vertical), out hitDash, playerDashData.ActiveDashDistance * 2);
        return hitDash;
    }
    public void Dash(float _dashTimeFrames)  // on complete sulla funzione ?
    {
        transform.DOMove(dashDirection, _dashTimeFrames);
    }

    public void PlayerInclination() {

        
        // Ruoto il personaggio in funzione della del suo movimento
        Vector3 rotationAxis = Quaternion.AngleAxis(90, Vector3.up) * movementVelocity;

        Quaternion moveRotation = Quaternion.AngleAxis(movementVelocity.magnitude * movimentRatio, rotationAxis);
        body.transform.rotation = moveRotation * rotationTransform.rotation;
    }
    public void UpdateOriantation()
    {
        rotationTransform.localRotation = dataInput.currentOrientation;

    }
    public void StopPlayer()
    {
        forwardVelocity = 0;
        movementVelocity = Vector3.zero;
    }

    public void StartTimerDash()
    {
        Debug.Log("TIMER");
        timerDash = Time.time;
    }
    public void PlayerDeath() {
        Debug.Log("MORTO");
        SceneManager.LoadScene(2);
    }

    public void Victory() {
        SceneManager.LoadScene(3);
    }

    //?Test player rope length constraint?
    public void RopeLengthConstraint(Vector3 nodePosition) {
        Vector3 constraint = new Vector3(nodePosition.x, nodePosition.y, transform.position.z);
        transform.Translate(constraint * Time.deltaTime);
    }
    public IEnumerator InvicibleSecond(float _sec)
    {
        IsImmortal = true;
        // yield return new WaitForSeconds(_sec);
       
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

