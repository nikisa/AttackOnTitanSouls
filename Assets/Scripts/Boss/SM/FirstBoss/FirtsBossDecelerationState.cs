using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirtsBossDecelerationState : FirstBossState
{
    //Inspector
    public DecelerationData decelerationData;

    //Private
    MoveToData moveToData;
    int iterations;
    int wallLayer;

    public override void Enter()
    {
        wallLayer = 10;
        iterations = 30;
        DecelerationEnter();
    }
    public override void Tick()
    {
        DecelerationTick();
        CollisionTick();
        SetSpeed();
    }
    public void DecelerationTick()
    {
        boss.Deceleration(decelerationData.TimeDeceleration, decelerationData.LowSpeed , moveToData.MaxSpeed);

        if (boss.MoveSpeed <= 0) {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }
     

    public void CollisionTick()
    {
        if (boss.MovingDetectCollision(iterations) == wallLayer)
        {
            animator.SetTrigger("Collision");
        }
        else
        {
            boss.Move();
        }
    }

    public void DecelerationEnter()
    {
        moveToData = boss.GetMoveToData();
    }

    public override void Exit()
    {
        boss.IsPrevStateReinitialize = false;
        CheckVulnerability();
       
    }

    //Set speed parameter in the animator
    public void SetSpeed() {
        animator.SetFloat("Speed", boss.MoveSpeed);
    }

}
