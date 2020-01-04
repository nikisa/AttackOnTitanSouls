using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaseState : BaseState
{
    protected BossController boss;



    //SateMachine Parameters
    protected const string IDLE = "Idle";
    protected const string ANTICIPATION = "Anticipation";
    protected const string MOVETO = "MoveTo";
    protected const string RECOVERY = "Recovery";
    protected const string END_STATE_TRIGGER = "EndState";

    public override void SetContext(object context, Animator animator)
    {
        base.SetContext(context, animator);
        boss = context as BossController;
    }

    protected void TriggerExitState()
    {
        animator.SetTrigger(END_STATE_TRIGGER);
    }


}