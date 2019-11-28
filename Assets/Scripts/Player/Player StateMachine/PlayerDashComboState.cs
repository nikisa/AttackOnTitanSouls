using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashCombo : PlayerBaseState {

    //Inspector
    public PlayerDashComboData playerDashComboData;
    public PlayerIdleData playerIdleData;

    //Private
    DataInput dataInput;
    float time;

    public override void Enter() {
        time = Time.time;
    }
    public override void Tick() {
        if (Time.time < time + playerDashComboData.DashComboTime && dataInput.Dash) {
            animator.SetTrigger(DASH);
        }
        else {
            animator.SetTrigger(IDLE);
        }
    }

    public override void Exit() {

    }
}
