using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUiState : UiBaseState
{
    public override void Enter()
    {
        Time.timeScale = 1;
        uiManager.ChangeMenu(UiManager.MenuType.Gameplay);
    }
    public override void Tick()
    {
        if (Input.GetButtonDown("Pause"))
        {
            animator.SetTrigger("Pause");
            
        }    
    }
    public override void Exit()
    {
        uiManager.DisableMenu(UiManager.MenuType.Gameplay);
    }
}
