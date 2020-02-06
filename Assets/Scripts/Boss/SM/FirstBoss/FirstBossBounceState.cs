﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossBounceState : FirstBossState
{

    //Inspector
    public BounceData bounceData;
    public GameObject Objecttt; 

    //Private
    float distance;
    float freezeTimeStart;
    float waitOnStartTimeStart;
    float angle;
    float speed;
    Vector3 stopPoint;
    Vector3 hitObjectPosition;
    Vector3 direction;

    Vector3 pointA;
    Vector3 pointH;
    Vector3 pointB;
    Vector3 directionAB;
    float vectorAH;
    float vectorAB;

    [Range(0, 1)]
    public float lerpValue;

    public override void Enter() {

        freezeTimeStart = Time.time;
        speed = boss.MoveSpeed - bounceData.kinetikEnergyLoss * boss.MoveSpeed;
        hitObjectPosition = boss.hitObject.collider.ClosestPoint(boss.transform.position);
        direction = boss.transform.position - hitObjectPosition;
        angle = Vector3.SignedAngle(boss.VelocityVector , direction , Vector3.up);
        direction = Quaternion.Euler(0, angle , 0) * direction;
        Debug.DrawRay(boss.transform.position, boss.VelocityVector * 10, Color.red, 10);
        Debug.DrawRay(boss.transform.position, direction * 10, Color.red, 10);
        distance = (Mathf.Pow(speed, 2) / (2 * bounceData.Deceleration));
        stopPoint = (boss.transform.position + (direction * distance));
        boss.BouncePointC.transform.position = stopPoint;

        pointA = boss.transform.position;
        pointH = (boss.transform.position + (direction * (distance / 2)));
        vectorAH = Vector3.Distance(boss.transform.position, pointH);
        vectorAB = (vectorAH / Mathf.Sin((90 - bounceData.BounceAngle) * Mathf.Deg2Rad));
        directionAB = pointH - boss.transform.position;
        directionAB =  Quaternion.Euler(0, bounceData.BounceAngle ,0) * directionAB;
        pointB = ((boss.transform.position + (directionAB.normalized * vectorAB)));
        boss.BouncePointB.transform.position = pointB;

        
    }

    public override void Tick() {
        lerpValue += Time.deltaTime;
        boss.transform.position = Vector3.Lerp(Vector3.Lerp(pointA, boss.BouncePointB.transform.position, lerpValue), Vector3.Lerp(pointA, boss.BouncePointC.transform.position, lerpValue), lerpValue);

    }

    public override void Exit() {
        
    }
}



