using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossBounceState : FirstBossState
{

    //Inspector
    public BounceData bounceData;

    //Private
    int layerResult;
    int iterations;
    int layerWall;
    int layerPlayer;
    float timeStartMoveTo;
    float reinitSphereCastTimer;

    float distance;
    float freezeTimeStart;
    float waitOnStartTimeStart;
    float angle;
    float speed;
    Vector3 stopPoint;
    Vector3 hitObjectPosition;
    Vector3 direction;
    Vector3 oldPos;

    Vector3 pointA;
    Vector3 pointH;
    Vector3 pointB;
    Vector3 directionAB;
    float vectorAH;
    float vectorAB;
    [Range(0, 1)]
    public float lerpValue;

    public override void Enter() {
        
        iterations = 100;
        layerWall = 10;
        layerPlayer = 11;
        lerpValue = 0;
        timeStartMoveTo = Time.time;
        reinitSphereCastTimer = 0.7f;

        bounceEnter();

    }

    public override void Tick() {
        //speed -= bounceData.Deceleration * Time.deltaTime;
        //lerpValue +=  Mathf.Abs(speed)/1000 * Time.deltaTime;+
        SetCycleTimer();
        Debug.DrawRay(boss.transform.position + new Vector3(0, 6, 0), boss.AccelerationVector * 10, Color.blue, .1f);

        lerpValue += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(Vector3.Lerp(pointA, boss.BouncePointB.transform.position, lerpValue), Vector3.Lerp(pointA, boss.BouncePointC.transform.position, lerpValue), lerpValue);
        

        Timer(bounceData);

        bounceDetectCollsion();


    }

    public override void Exit() {

        layerResult = 0;
        animator.SetInteger("Layer", layerResult);
        ResetTimer(bounceData);
    }

    void bounceEnter() {
        

        freezeTimeStart = Time.time;
        speed = boss.MoveSpeed - bounceData.kinetikEnergyLoss * boss.MoveSpeed;
        hitObjectPosition = boss.hitObject.collider.ClosestPointOnBounds(boss.transform.position);
        direction = boss.transform.position - hitObjectPosition;
        angle = Vector3.SignedAngle(boss.AccelerationVector, direction, Vector3.up);
        direction = Quaternion.Euler(0, angle, 0) * -direction;
        boss.AccelerationVector = boss.transform.position - oldPos;
        //boss.VelocityVector = direction;
        Debug.DrawRay(boss.transform.position, boss.AccelerationVector * 10, Color.red, 3);
        Debug.DrawRay(boss.transform.position, direction * 10, Color.red, 3);
        if (bounceData.kinetikEnergyLoss > 0.8) {
            distance = (Mathf.Pow(speed, 2) / (2 * bounceData.Deceleration));
        }
        else {
            distance = Mathf.Sqrt((Mathf.Pow(speed, 2) / (2 * bounceData.Deceleration))) / 2;
        }
        
        stopPoint = (boss.transform.position + (direction * distance));


        boss.BouncePointC.transform.position = stopPoint;
        pointA = boss.transform.position;
        pointH = (boss.transform.position + (direction * (distance / 2)));
        vectorAH = Vector3.Distance(boss.transform.position, pointH);
        vectorAB = (vectorAH / Mathf.Sin((90 - bounceData.BounceAngle) * Mathf.Deg2Rad));
        directionAB = pointH - boss.transform.position;
        directionAB = Quaternion.Euler(0, bounceData.BounceAngle, 0) * directionAB;
        pointB = ((boss.transform.position + (directionAB.normalized * vectorAB)));
        boss.BouncePointB.transform.position = pointB;

    }

    void bounceDetectCollsion() {

        Vector3 direction = boss.BouncePointC.transform.position - boss.transform.position;
        Debug.DrawRay(boss.transform.position, direction , Color.green);
        Vector3 nextPosition = boss.transform.position + direction.normalized * (speed * Time.deltaTime);
        Debug.Log("Next Position: " + nextPosition);

        layerResult = boss.MovingDetectCollision(iterations , nextPosition , speed);

        if (layerResult == layerWall) {
            Debug.Log("RE-BOUNCING");
            animator.SetInteger("Layer", layerResult);
        }
        else {

            oldPos = boss.transform.position;

            if (layerResult == layerPlayer) {
                if (!boss.Player.IsImmortal) {
                    PlayerController.DmgEvent();
                }
            }
        }

        
    }

}



