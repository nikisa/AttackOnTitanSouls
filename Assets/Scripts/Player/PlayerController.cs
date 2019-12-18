using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    
    public DataInput dataInput;
    public Animator animator;
    public CharacterController controller;
    public Transform rotationTransform;
    public Transform body;
    public float movimentRatio;
    public float DPS;
    [HideInInspector]
    public PlayerDashData playerDashData;
    [HideInInspector]
    public PlayerIdleData playerIdleData;

    Camera camera;

    protected virtual void Start() {
        camera = Camera.main;

        foreach (var item in animator.GetBehaviours<BaseState>()) {
            item.SetContext(this, animator);
        }
    }

    // Set Dash Action
    //    if (data.buttons[0] && isMoving) {
    //        Dash(DashTimeFrames, ResumeControl, DashTimeFreeze, data);
    //        isMoving = true;
    //    }

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

    //public InputData getData() {
    //    return im.data;
    //}



    public void CheckInput() {
        dataInput.Horizontal = Input.GetAxis("Horizontal");
        dataInput.Vertical = Input.GetAxis("Vertical");
        dataInput.HorizontalLook = Input.GetAxis("HorizontalLook");
        dataInput.VerticalLook = Input.GetAxis("VerticalLook");
        dataInput.Dash = Input.GetButtonDown("Dash");
        

        Vector3 lookVector = new Vector3(dataInput.HorizontalLook, 0, dataInput.VerticalLook);

        Debug.LogFormat("PAD:{0}", lookVector);

        if (lookVector.sqrMagnitude<0.0001f)
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

}

    public struct DataInput {
        public float Horizontal;
        public float HorizontalLook;
        public float Vertical;
        public float VerticalLook;
        public bool Dash;
        public Quaternion currentOrientation;


    public DataInput(float _horizontal, float _vertical , bool _dash , Quaternion _currentRotation) {
        Horizontal = _horizontal;
        Vertical = _vertical;
        Dash = _dash;
        this.currentOrientation = _currentRotation;
        HorizontalLook = 0;
        VerticalLook = 0;
    }
    
}
