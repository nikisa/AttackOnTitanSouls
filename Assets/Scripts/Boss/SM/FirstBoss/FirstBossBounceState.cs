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
    int HookPointLayerMask;

    public override void Enter() {

        HookPointLayerMask = 1 << 10 | 1 << 11;

        //freezeTimeStart = Time.time;
        //hitNormal = boss.hitObject.normal;
        //angle = Quaternion.AngleAxis(180 - bounceData.BounceAngle, Vector3.up) * hitNormal;
        //distance = (Mathf.Pow(boss.MoveSpeed, 2) / (2 * bounceData.Deceleration))/10;
        //stopPoint = angle * distance;
        //Instantiate(Objecttt , stopPoint , Quaternion.identity);
        //Debug.LogFormat("Velocità: {0} , Distance: {1} , StopPoint: {2}" , boss.MoveSpeed , distance ,stopPoint);
    }

    public override void Tick() {
        int iteration = 10;
        float skin = 4.2f;
        

        int interpolation = iteration;//(int)(MoveSpeed / 1f);

        for (int i = 0; i < interpolation; i++) {

            float time = Time.deltaTime / interpolation;
            RaycastHit[] hits = Physics.SphereCastAll(boss.transform.position + Vector3.up * 1.1f, skin, boss.MoveSpeed * Vector3.forward, (boss.MoveSpeed * time), HookPointLayerMask);

            if (hits != null || hits.Length != 0) {
                boss.hitObject = hits[0];
                Debug.LogFormat("Hit point:{0} ---- Object Hit:{1} ---- Object Transform:{2}", boss.hitObject.point, boss.hitObject.transform.gameObject.name, boss.hitObject.transform.position);
            }
        }
    }

    public override void Exit() {
        
    }
}



