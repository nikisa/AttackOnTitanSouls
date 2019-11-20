using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_BossAcceleration : BossBaseState
{
    

    //private
    float timeStart;
    public override void Enter()
    {
        timeStart = Time.time;
    }
    public override void Tick()
    {
        
    }
    public override void Exit()
    {

    }
}