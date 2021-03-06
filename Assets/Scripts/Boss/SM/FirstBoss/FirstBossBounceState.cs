﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossBounceState : FirstBossState
{
    //Inspector
    public BounceData bounceData;

    #region OldBounce Variables


    ////Private
    //int layerResult;
    //int iterations;
    //int layerWall;
    //int layerPlayer;
    //float timeStartMoveTo;
    //float reinitSphereCastTimer;

    //float distance;
    //float freezeTimeStart;
    //float waitOnStartTimeStart;
    //float angle;
    //float speed;
    //Vector3 stopPoint;
    //Vector3 hitObjectPosition;
    //Vector3 direction;
    //Vector3 oldPos;

    //Vector3 pointA;
    //Vector3 pointH;
    //Vector3 pointB;
    //Vector3 directionAB;
    //float vectorAH;
    //float vectorAB;
    //[Range(0, 1)]
    //public float lerpValue;
    #endregion

    //Private
    float angularVelocity;

    public override void Enter() {

        base.Enter();
        PlayerController Player = boss.Player;

        #region OldBonceEnter
        //iterations = 100;
        //layerWall = 10;
        //layerPlayer = 11;
        //lerpValue = 0;
        //timeStartMoveTo = Time.time;
        //reinitSphereCastTimer = 0.7f;

        //OldBounceEnter();
        #endregion
        boss.timerMaskCollision = Time.time;
        boss.impulseDeltaTime = bounceData.impulseDeltaTime;

        boss.KineticEnergyLoss = bounceData.kinetikEnergyLoss;
        Player.KineticEnergyLoss = bounceData.kinetikEnergyLoss;
        boss.SurfaceFriction = bounceData.surfaceFriction;
        Player.SurfaceFriction = bounceData.surfaceFriction;
        

        angularVelocity = getMostFarAngularVelocityMask();

            if (bossOrbitManager.ObjHit == 1) {
                boss.WallBounce(boss.hit , angularVelocity);
            }
            else if (bossOrbitManager.ObjHit == 2) {
                boss.BounceMovement(boss.hit.collider);
            }


            for (int i = 0; i < bossOrbitManager.MasksList.Count; i++) {
                if (bossOrbitManager.MasksList[i] != null) {
                bossOrbitManager.MasksList[i].UpdateAngularVelocity(bounceData.surfaceFriction);
                }
            }
        

        bossOrbitManager.hitMaskIndex = 0;
        animator.SetInteger("Layer", 0);

    }

    public override void Tick() {
        base.Tick();

        if (timer > bounceData.impulseDeltaTime) {
            animator.SetTrigger(END_STATE_TRIGGER);
        }

    }

    public override void Exit() {
        
        #region OldBounce Exit
        //layerResult = 0;
        //animator.SetInteger("Layer", layerResult);
        #endregion
    }


    float getMostFarAngularVelocityMask() {
        float result = 0;
        float currentRadius = 0;

        for (int i = 0; i < bossOrbitManager.MasksList.Count; i++) {
            if (bossOrbitManager.MasksList[i] != null) {
                if (bossOrbitManager.MasksList[i].currentRadius > currentRadius) {
                    currentRadius = bossOrbitManager.MasksList[i].currentRadius;
                    result = bossOrbitManager.MasksList[i].AngularVelocity;
                }
            }
        }
        return result;
    }

    #region OldBounceEnter Function
    //void OldBounceEnter() {


    //    freezeTimeStart = Time.time;
    //    speed = (1- bounceData.kinetikEnergyLoss) * boss.VelocityVector.magnitude;
    //    hitObjectPosition = boss.hitObject.collider.ClosestPointOnBounds(boss.transform.position);
    //    direction = boss.transform.position - hitObjectPosition;
    //    angle = Vector3.SignedAngle(boss.AccelerationVector, direction, Vector3.up);
    //    direction = Quaternion.Euler(0, angle, 0) * -direction;
    //    boss.AccelerationVector = Vector3.zero;
    //    Debug.DrawRay(boss.transform.position, boss.AccelerationVector * 10, Color.red, 3);
    //    Debug.DrawRay(boss.transform.position, direction * 10, Color.red, 3);
    //    if (bounceData.kinetikEnergyLoss > 0.8) {
    //        distance = (Mathf.Pow(speed, 2) / (2 * bounceData.Deceleration));
    //    }
    //    else {
    //        distance = Mathf.Sqrt((Mathf.Pow(speed, 2) / (2 * bounceData.Deceleration))) / 2;
    //    }

    //    stopPoint = (boss.transform.position + (direction * distance));


    //    boss.BouncePointC.transform.position = stopPoint;
    //    pointA = boss.transform.position;
    //    pointH = (boss.transform.position + (direction * (distance / 2)));
    //    vectorAH = Vector3.Distance(boss.transform.position, pointH);
    //    vectorAB = (vectorAH / Mathf.Sin((90 - bounceData.BounceAngle) * Mathf.Deg2Rad));
    //    directionAB = pointH - boss.transform.position;
    //    directionAB = Quaternion.Euler(0, bounceData.BounceAngle, 0) * directionAB;
    //    pointB = ((boss.transform.position + (directionAB.normalized * vectorAB)));
    //    boss.BouncePointB.transform.position = pointB;

    //}


    //void BounceDetectCollsion(Vector3 targetPoint) {

    //    layerResult = boss.DetectCollision(targetPoint);

    //    boss.CharacterController.Move((targetPoint-boss.transform.position) + Vector3.down * 10);

    //    if (layerResult == layerWall) {
    //        Debug.Log("RE-BOUNCING");
    //        animator.SetInteger("Layer", layerResult);
    //    }

    //}

    #endregion

}



