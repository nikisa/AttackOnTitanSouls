using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDashState : PlayerBaseState {

    //Inspector
    public PlayerDashData playerDashData;

    // Private
    bool isDashing;
    RaycastHit hitDash;
    float realDashDistance;
    DataInput dataInput;
    Vector3 playerPosition;
    Transform transform;
    float Horizontal;
    float Vertical;
    float _timeFreeze;
    float _timeScale;
    float freezeTimeStart;
    

    public override void Enter() {
        player.playerDashData = playerDashData;

        isDashing = false;

        Horizontal = player.dataInput.Horizontal;
        Vertical = player.dataInput.Vertical;

        freezeTimeStart = Time.time;
        player.DoFreeze(playerDashData.PreDashFreeze ,0);
        player.SetDashVelocity(Horizontal , Vertical , playerDashData.ActiveDashDistance , playerDashData.ActiveDashTime);
        
    }

    public override void Tick() {

        //if (Time.time - freezeTimeStart > playerDashData.PreDashFreeze) {
        //    player.timeFreeze(1);

            if (!isDashing) {
                Dash(playerDashData.ActiveDashTime , playerDashData.ActiveDashDistance);
            }
        //}
    }

    public override void Exit() {
        
    }

    public void Dash(float _DashTimeFrames, float _DashTimeDistance) {
        isDashing = true;
        playerPosition = player.transform.position;
        hitDash = player.RayCastDash(Horizontal, Vertical);
        realDashDistance = Vector3.Distance(hitDash.point, player.transform.position);

        //if (Horizontal == 0 && Vertical == 0) {
        //    animator.SetTrigger(IDLE);
        //}

        if (realDashDistance > _DashTimeDistance) {
            Debug.Log("DASH 1: " + Horizontal + "  -  " + Vertical);
            //Debug.Log("DASH 1: " + Mathf.Sign(Horizontal) * 1 + "  ---  " + Mathf.Sign(Vertical) * 1);
            player.dashDirection = new Vector3((Horizontal < 0.9f ? 0 : (_DashTimeDistance * Mathf.Sign(Horizontal) * 1)) + playerPosition.x, playerPosition.y, (Vertical < 0.9f ? 0 : (_DashTimeDistance * Mathf.Sign(Vertical) * 1)) + playerPosition.z);
            player.transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_DECELERATION); });
        }

        else {
            Debug.Log("DASH 2");
            player.dashDirection = new Vector3(((realDashDistance - (player.skin + (player.skin / 2))) * Horizontal) + playerPosition.x, playerPosition.y, ((realDashDistance - 0.75f) * Vertical) + playerPosition.z);
            player.transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_DECELERATION); });
        }
    }
}
