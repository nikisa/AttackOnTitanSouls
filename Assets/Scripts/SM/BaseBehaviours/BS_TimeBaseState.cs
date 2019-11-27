
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_TimeBaseState : BossBaseState
{
    public IdleData idleData; // da sistemare in time data
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