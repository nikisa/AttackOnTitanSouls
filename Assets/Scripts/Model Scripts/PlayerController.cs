using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;
using DG.Tweening;


public class PlayerController : Controller
{

    public InputManager im;
    protected Rigidbody rb;
    public DataInput dataInput;

    // StateMachine
    public PlayerBaseState currentState;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        ChangeState(currentState);

    }
    
    // Set Dash Action
    //    if (data.buttons[0] && isMoving) {
    //        Dash(DashTimeFrames, ResumeControl, DashTimeFreeze, data);
    //        isMoving = true;
    //    }

    private void FixedUpdate() {
        
        currentState.Tick();
        CheckInput();
    }

   
    //public InputData getData() {
    //    return im.data;
    //}

    public void ChangeState(PlayerBaseState _state) {

        if (currentState) {
            currentState.Exit();
            currentState = _state;
            currentState.Enter(this , rb);
        }
    }

    public override void ReadInput(InputData data) {
        //throw new System.NotImplementedException();
    }

    public void CheckInput() {
        dataInput.Horizontal = Input.GetAxis("Horizontal");
        dataInput.Vertical = Input.GetAxis("Vertical");
        dataInput.Dash = Input.GetButton("Dash");
    }

}

    public struct DataInput {
        public float Horizontal;
        public float Vertical;
        public bool Dash;


    public DataInput(float _horizontal, float _vertical , bool _dash) {
        Horizontal = _horizontal;
        Vertical = _vertical;
        Dash = _dash;
    }


}
