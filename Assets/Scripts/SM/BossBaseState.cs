using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaseState : BaseState
{
    protected BossController boss;
    protected FirstBossData data;

    protected const string END_STATE_TRIGGER = "EndState";

    //private
    protected const string IDLE = "Idle";
    protected const string ANTICIPATION = "Anticipation";
    protected const string MOVETO = "MoveTo";
    protected const string RECOVERY = "Recovery";

    public override void SetContext(object context, Animator animator)
    {
        base.SetContext(context, animator);
        boss = context as BossController;
        
        //data = boss.

    }

    protected void TriggerExitState()
    {
        animator.SetTrigger(END_STATE_TRIGGER);
    }
}