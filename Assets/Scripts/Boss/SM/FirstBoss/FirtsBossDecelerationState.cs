using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirtsBossDecelerationState : FirstBossState
{
   // public DecelerationData decelerationData;
    public RotationMoveData rotationMoveData;
    public RotationDecelerationData rotationDecelerationData;

    //Private
    MoveToData moveToData;
    float distance;
    RaycastHit hit;
    int iterations;

    public override void Enter()
    {
        OrbitTag();


        iterations = 30;

        CollisionEnter();
        DecelerationEnter();
    }
    public override void Tick()
    {
        DecelerationTick();
        //RotationMoveTick();
        //DecelerationRotationTick();
        CollisionTick();
      
    }
    public void DecelerationTick()
    {
        boss.Deceleration(moveToData.TimeDeceleration, moveToData.LowSpeed , moveToData.MaxSpeed);

        if (boss.MoveSpeed <= 0) {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }
    public void RotationMoveTick()
    {
        boss.View.MoveRotation(rotationMoveData.MaxSpeed);
    }
    public void DecelerationRotationTick()
    {
       boss.View.DecelerationRotation(rotationDecelerationData.DecelerationTime, rotationDecelerationData.LowSpeed);
        
    }
    public void CollisionTick()
    {
        if (boss.MovingDetectCollision(iterations) == 1)
        {
            animator.SetTrigger("Collision");
        }
        else
        {
            boss.Move();
        }
    }
    public void CollisionEnter()
    {
        hit = boss.RaycastCollision();
    }
    public void DecelerationEnter()
    {
        moveToData = boss.GetMoveToData();
    }
    public override void Exit()
    {
        boss.IsPrevStateReinitialize = false; // 
        CheckVulnerability();
        animator.SetBool("DecelerationMoveToOrbit", false);
    }
    public void OrbitTag()// funzione unica per tutto in orbit data in ingresso
    {

        //Debug.Log("Anticipation");
        //animator.SetInteger("OrbitTag", de.OrbitTag);


    }

}
