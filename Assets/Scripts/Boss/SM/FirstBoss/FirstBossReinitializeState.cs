using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossReinitializeState : FirstBossState
{
    public AnticipationData anticipationData;

    public override void Enter() {
        boss.loops = anticipationData.Loops;
        animator.SetTrigger(END_STATE_TRIGGER);
    }


    
    

}
