using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMoveToState : FirstBossState
{
    //public AccelerationData accelerationData;
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MoveTo"))
        {
            animator.SetBool("MoveTo", true);
        }
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
        //SetToCenter();
        TrailTick();
       
    }
    public override void Exit()
    {
        animator.SetBool("MoveTo", false);
    }
    public void ChargeAttack()
    { 

        targetPosition = new Vector3(Target.transform.position.x, startY, Target.transform.position.z);
        boss.RotateTarget(targetPosition);

    }

    public void MoveToEnter() {
        boss.moveToData = moveToData;
        boss.MaxSpeed = moveToData.MaxSpeed;
        boss.MoveSpeed += moveToData.AddToVelocity;
        startY = boss.transform.position.y;
        ChargeAttack();
        hit = boss.RaycastCollision();
        //if (moveToData.HasTimer)
        //{
            timeStartMoveTo = Time.time;
        //}
        //else
        //{
        //    timeStartMoveTo = Mathf.Infinity;
        //}
        
    }
    public void RotationEnter()
    {
        boss.RotationSpeed += rotationMoveData.AddToRotationSpeed;
    }
    public void MoveToTick() {
        //WallDistance = boss.CollisionDistance(hit.point);
        //distance = Vector3.Distance(boss.transform.position, targetPosition);
        //if (WallDistance <= 2 && moveToData.StopOnSolid) {
        //    Debug.Log("collisione");
        //    animator.SetTrigger("Collision");
        //}
   
        if (boss.DetectCollision() == 1 && Time.time - timeStartMoveTo > 0.15)
        {
            Debug.Log("collisione");
              animator.SetTrigger("Collision");
        }
        else if (boss.DetectCollision() == 2)
        {
            Debug.Log("CAVOLFIORE");
        }
        else
        {
            boss.Move();
        }
     
        if (moveToData.StopsAtTargetOvertaking) {
            if (boss.transform.position.x >= Target.transform.position.x - range && boss.transform.position.x <= Target.transform.position.x + range
                || boss.transform.position.z >= Target.transform.position.z - range && boss.transform.position.z <= Target.transform.position.z + range) {
                animator.SetTrigger("Deceleration");
            }

        }
        if (Time.time - timeStartMoveTo > moveToData.Time)
        {
            Debug.Log("TEMPO");
            animator.SetTrigger("Deceleration");
        }
        //if (distance <= 1 && !moveToData.StopsAtTargetOvertaking) {
        //    animator.SetTrigger(RECOVERY);  bohhhhh
        //}

      
    }

    public void AccelerationEnter() {
        timeStartAcceleration = Time.time;
    }

    public void AccelerationTick() {
        //if (Time.time - timeStartAcceleration > moveToData.WaitOnStart) {
            boss.Acceleration(moveToData.TimeAcceleration, moveToData.MaxSpeed);
        //}
    }

    public void RotationMoveTick() {
        boss.View.MoveRotation(rotationMoveData.MaxSpeed);
    }
    public void TrailTick()
    {
        
        if (Time.time - timeStartTrail > moveToData.TrailDelay )
        {
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
