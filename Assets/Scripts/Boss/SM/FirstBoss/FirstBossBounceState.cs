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
    Vector3 angle;
    Vector3 hitNormal;
    Vector3 stopPoint;
    Vector3 middlePoint;

    public override void Enter() {
        freezeTimeStart = Time.time;
        hitNormal = boss.hitObject.normal;
        angle = Quaternion.AngleAxis(180 - bounceData.BounceAngle, Vector3.up) * hitNormal;
        distance = Mathf.Pow(boss.MoveSpeed, 2) / (2 * bounceData.Deceleration);
        stopPoint = /*boss.transform.position +*/ angle * distance;
        Instantiate(Objecttt , stopPoint , Quaternion.identity);
        Debug.LogFormat("Velocità: {0} , Distance: {1} , StopPoint: {2}" , boss.MoveSpeed , distance ,stopPoint);
    }

    public override void Tick() {

        
    }

    public override void Exit() {
        
    }
}



