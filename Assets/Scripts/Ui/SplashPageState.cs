using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashPageState : UiBaseState
{
    public override void Enter()
    {
        uiManager.ChangeMenu(UiManager.MenuType.SplashPage);
    }
    public override void Tick()
    {
        if (Input.anyKey)
        {
            animator.SetTrigger("MainMenu");
        }  
    }
    public override void Exit()
    {
        uiManager.DisableMenu(UiManager.MenuType.SplashPage);
    }
}
