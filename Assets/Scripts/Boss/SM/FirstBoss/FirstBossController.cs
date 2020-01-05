using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : BossController
{
    //Inspector
    public BossOrbitManager bossOrbitManager;

    //Public
    //[HideInInspector]
    public int loops;
    [HideInInspector]
    public bool IsPrevStateReinitialize;

    protected override void Start() {
        base.Start();
        foreach (var item in animator.GetBehaviours<FirstBossState>()) {
            item.SetContext(this, animator , bossOrbitManager);
        }

    }
}