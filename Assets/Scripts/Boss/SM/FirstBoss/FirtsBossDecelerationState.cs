﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirtsBossDecelerationState : BossBaseState
{
   // public DecelerationData decelerationData;
    public RotationMoveData rotationMoveData;
    public RotationDecelerationData rotationDecelerationData;

    MoveToData moveToData;
    float distance;
    RaycastHit hit;
    public override void Enter()
    {
        CollisionEnter();
        DecelerationEnter();
    }
    public override void Tick()
    {
        DecelerationTick();
        RotationMoveTick();
        DecelerationRotationTick();
        CollisionTick();
        boss.Move();
    }
    public void DecelerationTick()
    {
        Debug.Log("decellera");
        boss.Deceleration(moveToData.TimeDeceleration, moveToData.LowSpeed , moveToData.MaxSpeed);
    }
    public void RotationMoveTick()
    {
        boss.View.MoveRotation(rotationMoveData.MaxSpeed);
    }
    public void DecelerationRotationTick()
    {
       boss.View.DecelerationRotation(rotationDecelerationData.DecelerationTime, rotationDecelerationData.LowSpeed);
        if (boss.MoveSpeed <= 0)
        {
            animator.SetTrigger(RECOVERY);
        }
        
    }
    public void CollisionTick()
    {
        distance = boss.CollisionDistance(hit.point);
        if (distance <= 2 && moveToData.StopOnSolid)
        {
            Debug.Log("collisione");
            animator.SetTrigger("Collision");
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
   
}
