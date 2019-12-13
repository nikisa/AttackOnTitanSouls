using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDashState : PlayerBaseState {

    //Inspector
    public PlayerDashData playerDashData;

    // Private
    DataInput dataInput;
    Vector3 position;
    Transform transform;
    float Horizontal;
    float Vertical;
    float time;
    
    public override void Enter() {
        time = Time.time;
        Dash(playerDashData.DashTimeFrames , playerDashData.ResumeControl , playerDashData.DashTimeFreeze , player.dataInput);
        Debug.Log("Dash enter");
        
    }

    public override void Tick() {
        //if (Time.time > time + playerDashData.DashTimeFrames &&  Time.time < time + playerDashData.DashTimeFrames + playerDashData.frameCombo) {
        //    if (dataInput.Dash) 
        //        animator.SetTrigger(DASH);
        //}
        //else {
        //    animator.SetTrigger(IDLE);
        //}
    }

    public override void Exit() {
        
    }

    public void Dash(float _DashTimeFrames, float _ResumeControl, float _DashTimeFreeze, DataInput _dataInput) {
        
        position = player.transform.position;
        transform = player.transform;

        Horizontal = player.dataInput.Horizontal;
        Vertical = player.dataInput.Vertical;

        _DashTimeFrames = _DashTimeFrames / 60;
        _ResumeControl = playerDashData.ResumeControl / 60;
        _DashTimeFreeze = playerDashData.DashTimeFreeze / 60;

        if (!playerDashData.isHookTest) {
            transform.DOMove(new Vector3((playerDashData.DashDistance * Horizontal) + position.x, position.y, (playerDashData.DashDistance * Vertical) + position.z), _DashTimeFrames).SetEase(playerDashData.DashEase);//.OnComplete( () => { _playerController.ChangeState(IdleState); });
        }
        else {
            transform.DOMove(new Vector3((playerDashData.DashDistance * Horizontal) + position.x, -(playerDashData.DashDistance * Vertical) + position.y, position.z), _DashTimeFrames).SetEase(playerDashData.DashEase);
        }
        animator.SetTrigger(IDLE);
        

    }

}
