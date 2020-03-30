using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossReinitializeState : FirstBossState
{
    public AnticipationData anticipationData;

    public override void Enter() {
        boss.loops = anticipationData.Loops + 1;//+1 altrimenti fa ogni volta un ciclo in meno
    }

    public override void Tick() {
        boss.IsPrevStateReinitialize = true;
        animator.SetInteger("Loops", boss.loops);
        animator.SetTrigger(END_STATE_TRIGGER);
    }


}
