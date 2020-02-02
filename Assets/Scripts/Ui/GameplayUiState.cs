using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUiState : UiBaseState
{
    public override void Enter()
    {
        uiManager.ChangeMenu(UiManager.MenuType.Gameplay);
    }
    public override void Tick()
    {
        if (Input.GetButtonDown("Pause"))
        {
            animator.SetTrigger("Pause");
          //  Time.timeScale = 0;
        }    
    }
    public override void Exit()
    {
        uiManager.DisableMenu(UiManager.MenuType.Gameplay);
    }
}
