using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OldPlayerDashState : PlayerBaseState
{

    //Inspector
    public PlayerDashData playerDashData;
    public Ease ease;
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



    public override void Enter() {
        player.ImmortalTutorial = true;// rende immortale il player per il dash per il tutorial
        player.playerDashData = playerDashData;
        isDashing = false;
        Horizontal = player.horizontalDash;
        Vertical = player.verticalDash;
        //(player.SetDashVelocity(Horizontal, Vertical, playerDashData.ActiveDashDistance, playerDashData.ActiveDashTime);

        //hitDash = player.RayCastDash(Horizontal, Vertical);
        //realDashDistance = Vector3.Distance(hitDash.collider.ClosestPointOnBounds(player.transform.position), player.transform.position);
        //realDashDistance /= 2.2f;

    }

    public override void Tick() {

        //if (Time.time - freezeTimeStart > playerDashData.PreDashFreeze) {
        //    player.timeFreeze(1);
        //}

        if (!isDashing) {
            setDashAnimation();
            if (realDashDistance > playerDashData.ActiveDashDistance) {
                Dash(playerDashData.ActiveDashTime, playerDashData.ActiveDashDistance, playerDashData.DashDecelerationTime);
                Debug.Log(realDashDistance);
            }
            else {
                Dash(playerDashData.ActiveDashTime, realDashDistance, playerDashData.DashDecelerationTime);
                Debug.Log(realDashDistance);
            }
        }
    }

    public override void Exit() {
        player.ImmortalTutorial = false;
    }

    public void Dash(float _DashTimeFrames, float _DashTimeDistance, float _decelerationTime) {
        isDashing = true;
        playerPosition = player.transform.position;

        //if (realDashDistance > _DashTimeDistance) {

            if (Horizontal > 0 && Vertical > 0)
            {
                player.dashDirection = new Vector3(_DashTimeDistance * Mathf.Cos(Mathf.Atan(Vertical /Horizontal)) + playerPosition.x, playerPosition.y, _DashTimeDistance * Mathf.Sin( Mathf.Atan(Vertical /Horizontal)) + playerPosition.z);
            }
            else if (Horizontal < 0 && Vertical < 0)
            {
                player.dashDirection = new Vector3(playerPosition.x - _DashTimeDistance * Mathf.Cos(Mathf.Atan(Vertical / Horizontal))  , playerPosition.y, playerPosition.z -_DashTimeDistance * Mathf.Sin(Mathf.Atan(Vertical / Horizontal))  );
            }
            else
            {
                player.dashDirection = new Vector3(playerPosition.x + _DashTimeDistance * Horizontal , playerPosition.y , playerPosition.z + _DashTimeDistance * Vertical);
            }


            if (Horizontal >= 0.0001f || Horizontal <= -0.0001 || Vertical >= 0.0001f || Vertical <= -0.0001) {
                    player.DoFreeze(playerDashData.PreDashFreeze, 0);
                    player.transform.DOMove(player.dashDirection, _DashTimeFrames).SetEase(ease).OnComplete(() => { animator.SetTrigger(DASH_DECELERATION); });
                    Debug.Log("dashDirection in x : " + _DashTimeDistance * Mathf.Sign(Horizontal));
            }
            else {
                    animator.SetTrigger(IDLE);
            }
        //}

        //else {
        //    Debug.Log("DASH 2");
        //    player.dashDirection = new Vector3((Horizontal <= 0.99f ? 0 : (_DashTimeDistance * Mathf.Sign(Horizontal) * 1)) + playerPosition.x, playerPosition.y, (Vertical <= 0.99f ? 0 : (_DashTimeDistance * Mathf.Sign(Vertical) * 1)) + playerPosition.z);
        //    player.transform.DOMove(player.dashDirection, _DashTimeFrames).SetEase(ease).OnComplete(() => { animator.SetTrigger(DASH_DECELERATION); });
        //    }


        }

        void setDashAnimation() {
            player.graphicAnimator.SetFloat("Horizontal", Mathf.Ceil(Horizontal * 4));
            player.graphicAnimator.SetFloat("Vertical", Mathf.Ceil(Vertical * 4));
        }

    }



