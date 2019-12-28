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
        OrbitTag();
     

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
        animator.SetBool("RecoveryOrbit", false);
        CheckVulnerability();
        boss.IsPrevStateReinitialize = false;
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
    public void OrbitTag()// funzione unica per tutto in orbit data in ingresso
    {

        Debug.Log("Anticipation");
        animator.SetInteger("OrbitTag", recoveryData.OrbitTag);


    }



    //public void DecelerationRotationEnter() {

    //    timeStartRotationDeceleration = Time.time;
    //}


}
