using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBaseState : BaseState
{
    [HideInInspector]
    public UiManager uiManager;
    public override void SetContext(object context, Animator animator)
    {      
        uiManager = context as UiManager;
        this.animator = animator;
    }

}
