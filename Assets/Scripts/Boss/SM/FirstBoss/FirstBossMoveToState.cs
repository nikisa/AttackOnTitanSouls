using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMoveToState : FirstBossState
{
    public AccelerationData accelerationData;
    public MoveToData moveToData;
    public RotationMoveData rotationMoveData;
    public BossController.Targets targets;
    [HideInInspector]
    public GameObject Target;
    //private 
    Vector3 targetPosition;
    float startY;
    int range=1;
    float distance;
    float WallDistance;
    float timeStartAcceleration;
    float timeStartTrail;
    float timeStartMoveTo;
    RaycastHit hit;


    public override void Enter()
    {
        RotationEnter();
        SetTarget();
        MoveToEnter();
        AccelerationEnter();
        TrailEnter();

    }
    public override void Tick()
    {
        MoveToTick();
        AccelerationTick();
        RotationMoveTick();
        SetToCenter();
        TrailTick();
       
    }
    public override void Exit()
    {
        
    }
    public void ChargeAttack()
    { 

        targetPosition = new Vector3(Target.transform.position.x, startY, Target.transform.position.z);
        boss.RotateTarget(targetPosition);

    }

    public void MoveToEnter() {
        boss.MoveSpeed += moveToData.AddToVelocity;
        startY = boss.transform.position.y;
        ChargeAttack();
        hit = boss.RaycastCollision();
        timeStartMoveTo = Time.time;
    }
    public void RotationEnter()
    {
        boss.RotationSpeed += rotationMoveData.AddToRotationSpeed;
    }
    public void MoveToTick() {
        WallDistance = boss.CollisionDistance(hit.point);
        distance = Vector3.Distance(boss.transform.position, targetPosition);
        if (WallDistance <= 2 && moveToData.StopOnSolid) {

            animator.SetTrigger("Collision");
        }
        
        
        if (moveToData.StopsAtTargetOvertaking) {
            if (boss.transform.position.x >= Target.transform.position.x - range && boss.transform.position.x <= Target.transform.position.x + range
                || boss.transform.position.z >= Target.transform.position.z - range && boss.transform.position.z <= Target.transform.position.z + range) {
                animator.SetTrigger(RECOVERY);
            }

        }
        if (Time.time - timeStartMoveTo > moveToData.MoveToDuration)
        {
            Debug.Log("TEMPO");
            animator.SetTrigger(RECOVERY);
        }
        //if (distance <= 1 && !moveToData.StopsAtTargetOvertaking) {
        //    animator.SetTrigger(RECOVERY);  bohhhhh
        //}

        boss.Move();
    }

    public void AccelerationEnter() {
        timeStartAcceleration = Time.time;
    }

    public void AccelerationTick() {
        if (Time.time - timeStartAcceleration > accelerationData.WaitOnStart) {
            boss.Acceleration(accelerationData.TimeAcceleration, moveToData.MaxSpeed);
        }
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation(rotationMoveData.MaxSpeed);
    }
    public void TrailTick()
    {
        
        if (Time.time - timeStartTrail > moveToData.TrailDelay )
        {
            Debug.Log("dentro");
            Instantiate(moveToData.TrailOb, boss.transform.position, Quaternion.identity);
            timeStartTrail = Time.time;
        }
      
    }
    public void TrailEnter()
    {
        timeStartTrail = Time.time;
    }
    public void SetToCenter() {
        if (boss.transform.position.x < -70 ||
            boss.transform.position.x > 70 ||
            boss.transform.position.z < -100 ||
            boss.transform.position.z > 120) {
            boss.transform.position = GameObject.FindGameObjectWithTag("Center").transform.position;
            animator.SetTrigger(IDLE);
        }
    }
    public void SetTarget()
    {
        Target=boss.SetTarget(targets);
    }
}
