using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : BossController
{
    //Inspector
    public BossOrbitManager bossOrbitManager;
    public float RotationVelocity;

    //Public
    [HideInInspector]
    public float vectorAngle;
    [HideInInspector]
    public FirstBossMask firstBossMask;


    //[HideInInspector]
    public int loops;
    [HideInInspector]
    public bool IsPrevStateReinitialize;

    protected override void Start() {
        base.Start();
        foreach (var item in animator.GetBehaviours<FirstBossState>()) {
            item.SetContext(this, firstBossMask , animator , bossOrbitManager);
        }

    }
}