﻿using System.Collections;
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
        Dash(playerDashData.DashTimeFrames , playerDashData.ResumeControl , player.dataInput);
        Debug.Log("Dash enter");
        _timeFreeze = playerDashData.TimeFreeze;
    }

    public override void Tick() {
        timeFreeze(_timeFreeze);

    }

    public override void Exit() {
        
    }

    public void Dash(float _DashTimeFrames, float _ResumeControl, DataInput _dataInput) {

        

        position = player.transform.position;
        transform = player.transform;

        Horizontal = player.dataInput.Horizontal;
        Vertical = player.dataInput.Vertical;

        _DashTimeFrames = _DashTimeFrames / 60;
        _ResumeControl = playerDashData.ResumeControl / 60;

        //if (!playerDashData.isHookTest) {
        Physics.Raycast(player.transform.position, new Vector3(Horizontal, 0, Vertical),out hitDash, playerDashData.DashDistance *2);
        realDashDistance=Vector3.Distance(hitDash.point, player.transform.position);
        if (realDashDistance > playerDashData.DashDistance)
        {
            player.dashDirection = new Vector3((playerDashData.DashDistance * Horizontal) + position.x, position.y, (playerDashData.DashDistance * Vertical) + position.z);
            transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_RESUME); });
        }
        else
        {
            player.dashDirection = new Vector3(((realDashDistance - 1) * Horizontal) + position.x, position.y, ((realDashDistance - 1) * Vertical) + position.z);
            transform.DOMove(player.dashDirection , _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_RESUME); });
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

        player.InitialDashVelocity = playerDashData.DashDistance / (_DashTimeFrames / Time.deltaTime);
        animator.SetTrigger(DASH_RESUME);

    }

    public void timeFreeze(float _timeFreeze) {
        if (Time.time - timeStart < _timeFreeze) {
            Time.timeScale = 1;
        }
    }

}
