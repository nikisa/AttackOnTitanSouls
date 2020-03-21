using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossReinitializeState : FirstBossState
{
    public AnticipationData anticipationData;

    public override void Enter() {
        anticipationData.loops = anticipationData.Loops + 1;
        boss.loops = anticipationData.loops;
    }



    public override void Tick() {
        
        
        boss.IsPrevStateReinitialize = true;

        //if (boss.loops < 0) {
        //    boss.loops = anticipationData.Loops;
        //}

        animator.SetInteger("Loops", boss.loops);
        Debug.Log("boss Loops: " + boss.loops);

        animator.SetTrigger(END_STATE_TRIGGER);
    }


}
