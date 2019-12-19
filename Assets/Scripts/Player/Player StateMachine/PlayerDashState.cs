using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDashState : PlayerBaseState {

    //Inspector
    public PlayerDashData playerDashData;

    // Private
    RaycastHit hitDash;
    float realDashDistance;
    DataInput dataInput;
    Vector3 position;
    Transform transform;
    float Horizontal;
    float Vertical;
    float _timeFreeze;
    float timeStart;


    public override void Enter() {
       
        player.playerDashData = playerDashData;
        player.DoFreeze(playerDashData.TimeFreeze , playerDashData.SetTimeScale);
        Dash(playerDashData.DashTimeFrames , playerDashData.ResumeControl , playerDashData.DashTimeFreeze , player.dataInput);
        Debug.Log("Dash enter");
        _timeFreeze = playerDashData.DashTimeFreeze;
    }

    public override void Tick() {
        timeFreeze(_timeFreeze);

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

        //if (!playerDashData.isHookTest) {
        Physics.Raycast(player.transform.position, new Vector3(Horizontal, 0, Vertical),out hitDash, playerDashData.DashDistance *2);
        realDashDistance=Vector3.Distance(hitDash.point, player.transform.position);
        if (realDashDistance > playerDashData.DashDistance)
        {
            transform.DOMove(new Vector3((playerDashData.DashDistance * Horizontal) + position.x, position.y, (playerDashData.DashDistance * Vertical) + position.z), _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_DECELERATION); });
        }
        else
        {
            transform.DOMove(new Vector3(((realDashDistance - 1) * Horizontal) + position.x, position.y, ((realDashDistance - 1) * Vertical) + position.z), _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_DECELERATION); });
        }
        // da togliere la skin al posto di 1

        //}
        //else {
        //    transform.DOMove(new Vector3((playerDashData.DashDistance * Horizontal) + position.x, -(playerDashData.DashDistance * Vertical) + position.y, position.z), _DashTimeFrames).SetEase(playerDashData.DashEase);
        //}

        //public void Deceleration() {
        //    Debug.Log(velocity);
        //    velocity -= timeDeceleration * Time.deltaTime;
        //    if (velocity <= playerIdleData.maxSpeed) {
        //        animator.SetTrigger(DASH_RESUME);
        //    }
        //}


        animator.SetTrigger(DASH_RESUME);

    }

    public void timeFreeze(float _timeFreeze) {
        if (Time.time - timeStart < _timeFreeze) {
            Time.timeScale = 1;
        }
    }

}
