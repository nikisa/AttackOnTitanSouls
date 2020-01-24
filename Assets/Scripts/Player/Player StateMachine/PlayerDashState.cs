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

        Horizontal = player.horizontalDash;
        Vertical = player.verticalDash;

        freezeTimeStart = Time.time;
        player.DoFreeze(playerDashData.PreDashFreeze ,0);
        player.SetDashVelocity(Horizontal , Vertical , playerDashData.ActiveDashDistance , playerDashData.ActiveDashTime);
        
    }

    public override void Tick() {

        //if (Time.time - freezeTimeStart > playerDashData.PreDashFreeze) {
        //    player.timeFreeze(1);

            if (!isDashing) {
                Dash(playerDashData.ActiveDashTime , playerDashData.ActiveDashDistance, playerDashData.DashDecelerationTime);
            }
        //}
    }

    public override void Exit() {
        
    }

    public void Dash(float _DashTimeFrames, float _DashTimeDistance, float _decelerationTime) {
        isDashing = true;
        playerPosition = player.transform.position;
        hitDash = player.RayCastDash(Horizontal, Vertical);
        realDashDistance = Vector3.Distance(hitDash.point, player.transform.position);
        realDashDistance = realDashDistance + Mathf.Pow(player.dashMovementSpeed, 2) / (2 * (player.dashMovementSpeed / _decelerationTime)); // questa formula è sbagliata fa 1000/1000 e non aggiunge nulla
        //if (Horizontal == 0 && Vertical == 0) {
        //    animator.SetTrigger(IDLE);
        //}

<<<<<<< HEAD
        if (realDashDistance > _DashTimeDistance) {
            Debug.Log("DASH 1: " + Horizontal + "  -  " + Vertical);
            //Debug.Log("DASH 1: " + Mathf.Sign(Horizontal) * 1 + "  ---  " + Mathf.Sign(Vertical) * 1);
            player.dashDirection = new Vector3((Horizontal < 0.9f ? 0 : (_DashTimeDistance * Mathf.Sign(Horizontal) * 1)) + playerPosition.x, playerPosition.y, (Vertical < 0.9f ? 0 : (_DashTimeDistance * Mathf.Sign(Vertical) * 1)) + playerPosition.z);
=======
        // (Mathf.Pow(boss.MoveSpeed, 2) / (2 * bounceData.Deceleration)) QUIII usare questa

         if (realDashDistance > _DashTimeDistance) {
            player.dashDirection = new Vector3((_DashTimeDistance * Horizontal) + playerPosition.x, playerPosition.y, (_DashTimeDistance * Vertical) + playerPosition.z);
>>>>>>> 7e6d4760223e2e4906f1e699b4281c052e7df23f
            player.transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_DECELERATION); });
        }

        else {
            Debug.Log("DASH 2");
            player.dashDirection = new Vector3(((realDashDistance - (player.skin + (player.skin / 2))) * Horizontal) + playerPosition.x, playerPosition.y, ((realDashDistance - 0.75f) * Vertical) + playerPosition.z);
            player.transform.DOMove(player.dashDirection, _DashTimeFrames).OnComplete(() => { animator.SetTrigger(DASH_DECELERATION); });
        }
    }
}
