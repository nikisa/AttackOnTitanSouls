using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_BossIdle : BS_TimeBaseState
{
    public override void Enter()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetBool("IdleOrbit", true);
        }
     
        base.Enter();
        boss.MoveSpeed = 0;
    }
    public override void Tick()
    {
        base.Tick();
    }
    public override void Exit()
    {
        animator.SetBool("IdleOrbit", false);
    }

}
