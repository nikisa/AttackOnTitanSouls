using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossState : BaseState
{
    protected FirstBossController boss;



    protected const string END_STATE_TRIGGER = "EndState";

    //private
    protected const string IDLE = "Idle";
    protected const string ANTICIPATION = "Anticipation";
    protected const string MOVETO = "MoveTo";
    protected const string RECOVERY = "Recovery";



    public override void SetContext(object context, Animator animator , BossOrbitManager bossOrbitManager)
    {
        base.SetContext(context, animator , bossOrbitManager);

        boss = context as FirstBossController;
        
    }
    protected void TriggerExitState()
    {
        animator.SetTrigger(END_STATE_TRIGGER);
    }

    protected void DetectCollision(int _iteration) {
        if (boss.MovingDetectCollision(_iteration) == 2) {
            Debug.Log("CAVOLFIORE");
        }
    }

}