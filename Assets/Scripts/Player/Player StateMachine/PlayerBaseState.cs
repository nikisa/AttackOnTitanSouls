using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : BaseState {

    protected BossController boss;
    protected PlayerController player;


    //SateMachine Parameters
    protected const string END_STATE_TRIGGER = "EndState";
    protected const string IDLE = "Idle";
    protected const string DASH = "Dash";
    protected const string DASH_DECELERATION = "DashDeceleration";
    protected const string DASH_RESUME = "Resume";
    protected const string DASH_FRAME_PERFECT = "DashFramePerfect";
    

    public override void SetContext(object context, Animator animator) { //Togliere bossOrbitManager dal Player
        base.SetContext(context, animator);
        player = context as PlayerController;
    }

    protected void TriggerExitState() {
        animator.SetTrigger(END_STATE_TRIGGER);
    }
}
