using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : BossController
{
    //Inspector
    public BossOrbitManager bossOrbitManager;
    public GameObject BouncePointC;
    public GameObject BouncePointB;
    

    //Public
    [HideInInspector]
    public float vectorAngle;
    [HideInInspector]
    public Vector3 OldPos;
    [HideInInspector]
    public Vector3 Inertia;
    [HideInInspector]
    public float AccelerationModule;
    [HideInInspector]
    public Vector3 AccelerationVector;
    [HideInInspector]
    public Vector3 VelocityVector;
    [HideInInspector]
    public Vector3 MaxSpeedVector;
    [HideInInspector]
    public float Drag;

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