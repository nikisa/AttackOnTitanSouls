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
    



    public override void Enter() {
       
        player.playerDashData = playerDashData;
        player.DoFreeze(playerDashData.TimeFreeze , playerDashData.SetTimeScale);
        Dash(playerDashData.DashTimeFrames , playerDashData.ResumeControl , player.dataInput);
        Debug.Log("Dash enter");
        _timeFreeze = playerDashData.TimeFreeze;
    }

    public override void Tick() {
        player.timeFreeze(_timeFreeze);
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
        /*
         * Dash compenetra quando il movimento è diagonale (horizonta e vertical insieme)
         * Sostituire RayCast con SphereCastAll per quanto riguarda il controllo?
         */

        hitDash= player.RayCastDash(Horizontal, Vertical);
        realDashDistance = Vector3.Distance(hitDash.point, player.transform.position);

        //Physics.SphereCastAll(player.transform.position , player.transform.localScale.x , new Vector3(Horizontal , 0 , Vertical) , playerDashData.DashDistance);

        if (realDashDistance > playerDashData.DashDistance)
        {
            player.dashDirection = new Vector3((playerDashData.DashDistance * Horizontal) + position.x, position.y, (playerDashData.DashDistance * Vertical) + position.z);
            transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_RESUME); });
        }
        else {
            if (Horizontal != 0 && Vertical != 0) {

                //Sottrarre o sommare da realDashDistance in base alla direzione del dash

                //if (Horizontal > 0  && Vertical > 0) {
                //    player.dashDirection = new Vector3(((realDashDistance - 2.75f) * Horizontal) + position.x, position.y, ((realDashDistance - 2.75f) * Vertical) + position.z);                   
                //}

                //else if (Horizontal > 0 && Vertical < 0) {
                //    player.dashDirection = new Vector3(((realDashDistance - 2.75f) * Horizontal) + position.x, position.y, ((realDashDistance + 2.75f) * Vertical) + position.z);
                //}

                //else if (Horizontal < 0 && Vertical > 0) {
                //    player.dashDirection = new Vector3(((realDashDistance + 2.75f) * Horizontal) + position.x, position.y, ((realDashDistance - 2.75f) * Vertical) + position.z);
                //}

                //else if (Horizontal < 0 && Vertical < 0) {
                //    player.dashDirection = new Vector3(((realDashDistance - 2.75f) * Horizontal) + position.x, position.y, ((realDashDistance - 2.75f) * Vertical) + position.z);
                //}
                animator.SetTrigger(IDLE);

            }
            else {
                player.dashDirection = new Vector3(((realDashDistance - 0.75f) * Horizontal) + position.x, position.y, ((realDashDistance - 0.75f) * Vertical) + position.z);
                transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_RESUME); });
            }

            

        } // da togliere la skin+(skin/2) al posto di 0.75f



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

  

}
