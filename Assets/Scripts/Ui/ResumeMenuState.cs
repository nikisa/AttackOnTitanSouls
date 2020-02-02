using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeMenuState : UiBaseState
{
 
    public override void Enter()
    {
        uiManager.ChangeMenu(UiManager.MenuType.PauseMenu);
        uiManager.ChangeMenu(UiManager.MenuType.BackGroundPause);
    }
    public override void Tick()
    {
        if (Input.GetButtonDown("Pause"))
        {
            animator.SetTrigger("Gameplay");
            Time.timeScale = 1;
        }
    }
    public override void Exit()
    {
        uiManager.DisableMenu(UiManager.MenuType.PauseMenu);
        uiManager.DisableMenu(UiManager.MenuType.BackGroundPause);
    }
}
