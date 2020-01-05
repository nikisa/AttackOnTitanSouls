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

        hitDash= player.RayCastDash(Horizontal, Vertical);
        realDashDistance = Vector3.Distance(hitDash.point, player.transform.position);

        
        if (realDashDistance > playerDashData.DashDistance)
        {
            player.dashDirection = new Vector3((playerDashData.DashDistance * Horizontal) + position.x, position.y, (playerDashData.DashDistance * Vertical) + position.z);
            transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_RESUME); });
        }

        else {
            if (Horizontal != 0 && Vertical != 0) {
                animator.SetTrigger(IDLE);
            }
            else {
                player.dashDirection = new Vector3(((realDashDistance - (player.skin+(player.skin/2))) * Horizontal) + position.x, position.y, ((realDashDistance - 0.75f) * Vertical) + position.z);
                transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_RESUME); });
            }

            

        } // da togliere la skin+(skin/2) al posto di 0.75f

        player.InitialDashVelocity = playerDashData.DashDistance / (_DashTimeFrames / Time.deltaTime);
        animator.SetTrigger(DASH_RESUME);

    }
}
