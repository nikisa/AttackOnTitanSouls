using System.Collections;
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
    Vector3 hitNormal;
    Vector3 stopPoint;
    Vector3 middlePoint;
    int HookPointLayerMask;
    Vector3 hitObjectPosition;
    Vector3 direction;

    public override void Enter() {

        freezeTimeStart = Time.time;
        hitObjectPosition = boss.hitObject.collider.ClosestPoint(boss.transform.position);
        direction = boss.transform.position - hitObjectPosition;
        direction = Quaternion.Euler(0, bounceData.BounceAngle , 0) * direction;
        Debug.DrawRay(boss.transform.position, direction * 10, Color.red, 10);
        distance = (Mathf.Pow(boss.MoveSpeed, 2) / (2 * bounceData.Deceleration)) / 10;
        stopPoint = (boss.transform.position + (direction * distance));
        Instantiate(Objecttt, stopPoint, Quaternion.identity);
        Debug.LogFormat("Velocità: {0} , Distance: {1} , StopPoint: {2}", boss.MoveSpeed, distance, stopPoint);
    }

    public override void Tick() {
    }

    public override void Exit() {
        
    }
}



