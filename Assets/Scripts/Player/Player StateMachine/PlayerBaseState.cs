using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : BaseState {

    protected BossController boss;
    protected PlayerController player;
    protected Animator graphicAnimation;


    //SateMachine Parameters
    protected const string END_STATE_TRIGGER = "EndState";
    protected const string IDLE = "Idle";
    protected const string MOVEMENT_DECELERATION = "MovementDeceleration";
    protected const string DASH = "Dash";
    protected const string DASH_DECELERATION = "DashDeceleration";
    protected const string DASH_RESUME = "Resume";
    protected const string DASH_FRAME_PERFECT = "DashFramePerfect";


    public void SetContext(object context, Animator animator , Animator graphicAnimation) {
        player = context as PlayerController;
        this.graphicAnimation = graphicAnimation;
    }

    protected void TriggerExitState() {
        animator.SetTrigger(END_STATE_TRIGGER);
    }
}
