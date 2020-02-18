using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirtsBossDecelerationState : FirstBossState
{
    //Inspector
    public DecelerationData decelerationData;
    public bool Debugging;
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
        //DecelerationTick();
        CollisionTick();
        // SetSpeed();
        setChaseRadius();
        NewDeceleration();
        SetCycleTimer();
    }
    //public void DecelerationTick()
    //{
    //    boss.Deceleration(decelerationData.TimeDeceleration, decelerationData.LowSpeed , moveToData.MaxSpeed);

    //    if (boss.MoveSpeed <= 0) {
    //        animator.SetTrigger(END_STATE_TRIGGER);
    //    }
    //}
     

    public void CollisionTick()
    {

        Vector3 nextPosition = boss.transform.position + (boss.MoveSpeed * Time.deltaTime) * boss.transform.forward;
        if (boss.MovingDetectCollision(iterations , nextPosition , boss.MoveSpeed) == wallLayer)
        {
            animator.SetTrigger("Collision");
        }
        else
        {
           // boss.Move();
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
    public void NewDeceleration()
    {
        boss.DeceleratioModule = decelerationData.Deceleration;

        if (boss.VelocityVector.magnitude > boss.DeceleratioModule * Time.deltaTime)
        {
            boss.vectorAngle = Vector3.SignedAngle(Vector3.forward, boss.VelocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
            boss.DecelerationVector = new Vector3(Mathf.Sin(boss.vectorAngle) * boss.DeceleratioModule, 0, Mathf.Cos(boss.vectorAngle) * boss.DeceleratioModule);
            boss.VelocityVector -= boss.DecelerationVector * Time.deltaTime;
            boss.transform.localPosition += boss.VelocityVector * Time.deltaTime;
        }
        else 
        {
            boss.VelocityVector = Vector3.zero;
            animator.SetTrigger(END_STATE_TRIGGER);
        }




        if (Debugging)
        {
            Debug.DrawLine(boss.transform.position, boss.transform.position + boss.AccelerationVector, Color.red, .02f);
            Debug.DrawLine(boss.transform.position, boss.transform.position + boss.VelocityVector, Color.blue, .02f);
            //Debug.DrawLine(transform.position, Player.transform.position, Color.green, .02f);
            //Debug.DrawLine(boss.transform.position, boss.MaxSpeedVector, Color.green, .5f);
        }

    }

    public void setChaseRadius()
    {
        float distance = (boss.Target.transform.position - boss.transform.position).magnitude;
        animator.SetFloat("ChaseRadius", distance);
    }


}
