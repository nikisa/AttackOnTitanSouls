using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FirstBossRecoveryState : FirstBossState
{

    public RecoveryData recoveryData;
    
    public DecelerationData decelerationData;
    

    //private
    bool stop;
    float timeStartRecovery;
    float timeStartDeceleration;
    float timeStartRotationDeceleration;
   
    RaycastHit hit;

    public override void Enter()
    {
        RecoveryInfoEnter();
      //  DecelerationEnter();
        //DecelerationRotationEnter();
    }
    public override void Tick()
    {
        RecoveryInfoTick();
        DecelerationTick();
        //RotationMoveTick();
        //DecelerationRotationTick();
    }
    public override void Exit()
    {
       
    }

    public void RecoveryInfoEnter() {
        //moveToData = boss.GetMoveToData();
        //hit = boss.RaycastCollision();
        timeStartRecovery = Time.time;
        //stop = false;
    }
   
    public void RecoveryInfoTick() {
       

        if ((Time.time - timeStartRecovery) > recoveryData.Time) {
            animator.SetTrigger(END_STATE_TRIGGER);
        }
    }

    //public void DecelerationEnter() {
    //    timeStartDeceleration = Time.time;
    //}

    public void DecelerationTick() {

    //    //if (Time.time - timeStartDeceleration > recoveryData.WaitOnStart) {
          boss.Deceleration(decelerationData.TimeDeceleration, decelerationData.LowSpeed , boss.MaxSpeed);
        Debug.Log(boss.MaxSpeed);
    //    //}
    }

   

    //public void DecelerationRotationEnter() {

    //    timeStartRotationDeceleration = Time.time;
    //}

 
}
