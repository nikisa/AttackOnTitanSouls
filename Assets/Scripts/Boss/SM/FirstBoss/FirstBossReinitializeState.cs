using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossReinitializeState : FirstBossState
{
    public AnticipationData anticipationData;

    public override void Enter() {
        boss.loops = anticipationData.Loops +1;
        boss.IsPrevStateReinitialize = true;
        animator.SetTrigger(END_STATE_TRIGGER);
    }

}
