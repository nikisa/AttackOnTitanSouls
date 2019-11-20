
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_TimeBaseState : BossBaseState
{
    
    float endTime;

    public override void Enter()
    {
        endTime = Time.time + boss.Data.idleInfo.stateDuration;
    }

    public override void Tick()
    {
        if (Time.time >= endTime) TriggerExitState();
    }
}