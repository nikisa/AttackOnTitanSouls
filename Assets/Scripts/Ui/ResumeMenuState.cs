using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeMenuState : UiBaseState
{
 
    public override void Enter()
    {

        uiManager.ChangeMenu(UiManager.MenuType.PauseMenu);
        uiManager.ChangeMenu(UiManager.MenuType.BackGroundPause);
        Time.timeScale = 0;
    }
    public override void Tick()
    {
        if (Input.GetButtonDown("Pause"))
        {
            animator.SetTrigger("Gameplay");
             
        }
    }
    public override void Exit()
    {

        uiManager.DisableMenu(UiManager.MenuType.PauseMenu);
        uiManager.DisableMenu(UiManager.MenuType.BackGroundPause);

    }
}
