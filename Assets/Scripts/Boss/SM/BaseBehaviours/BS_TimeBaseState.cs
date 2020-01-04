
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_TimeBaseState : FirstBossState
{
    //PROBABILMENTE NON SERVE 

    public IdleData idleData; 
    float endTime;

    public override void Enter()
    {
        endTime = Time.time + idleData.stateDuration;
    }

    public override void Tick()
    {
        if (Time.time >= endTime) TriggerExitState();
    }
}