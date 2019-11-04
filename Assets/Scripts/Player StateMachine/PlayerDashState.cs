using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDashState : PlayerBaseState {

    // Public
    public float DashDistance;
    public float DashTimeFrames;
    public Ease DashEase;
    public float ResumeControl;
    public float DashTimeFreeze;
    public PlayerBaseState IdleState;

    // Private
    PlayerController _playerController;
    Vector3 position;
    Transform transform;
    float Horizontal;
    float Vertical;

    public void Dash(float _DashTimeFrames, float _ResumeControl, float _DashTimeFreeze, DataInput _dataInput) {

        position = _playerController.transform.position;
        transform = _playerController.transform;

        Horizontal = _playerController.dataInput.Horizontal;
        Vertical = _playerController.dataInput.Vertical;
        
        _DashTimeFrames = _DashTimeFrames / 60;
        _ResumeControl = ResumeControl / 60;
        _DashTimeFreeze = DashTimeFreeze / 60;

        transform.DOMove(new Vector3((DashDistance * Horizontal) + position.x, position.y, (DashDistance * Vertical) + position.z), _DashTimeFrames).SetEase(DashEase); //.OnComplete( () => { _playerController.ChangeState(IdleState); });


        _playerController.ChangeState(IdleState);



    }
    public override void Enter(PlayerController playerController, Rigidbody _rb) {
        _playerController = playerController;
        Dash(DashTimeFrames , ResumeControl , DashTimeFreeze , _playerController.dataInput);
    }

    public override void Tick() {
        
    }

    public override void Exit() {
        
    }
}
