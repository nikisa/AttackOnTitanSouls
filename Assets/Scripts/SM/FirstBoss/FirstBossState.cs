using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossState : BossBaseState
{

  




    public override void SetContext(object context, Animator animator)
    {
        base.SetContext(context, animator);

        boss = context as FirstBossController;
        
    }

}