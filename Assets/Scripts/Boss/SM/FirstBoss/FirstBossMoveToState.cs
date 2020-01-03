using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    int iterations;
    float timeMoveTo;

    public override void Enter()
    {
        OrbitTag();
        iterations = 30;
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
        //RotationMoveTick();
        //SetToCenter();
        //TrailTick();
       
    }
    public override void Exit()
    {
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
        animator.SetBool("MoveToOrbit", false);
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
        timeMoveTo = moveToData.Time - moveToData.TimeDeceleration;
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
   
        if (boss.MovingDetectCollision(iterations) == 1 && Time.time - timeStartMoveTo > .5f) // BUG HERE!!! Probabilmente causato dal Timer
        {
            Debug.Log("collisione");
              animator.SetTrigger("Collision");
        }
        else {

            boss.Move();

            if (boss.MovingDetectCollision(iterations) == 2) {
                SceneManager.LoadScene(2);
            }
        }
     
        if (moveToData.StopsAtTargetOvertaking) {
            if (boss.transform.position.x >= Target.transform.position.x - range && boss.transform.position.x <= Target.transform.position.x + range
                || boss.transform.position.z >= Target.transform.position.z - range && boss.transform.position.z <= Target.transform.position.z + range) {
                animator.SetTrigger("Deceleration");
            }

        }
        if (Time.time - timeStartMoveTo > timeMoveTo)
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
    public void OrbitTag()// funzione unica per tutto in orbit data in ingresso
    {

        Debug.Log("Anticipation");
        animator.SetInteger("OrbitTag", moveToData.OrbitTag);


    }
}
