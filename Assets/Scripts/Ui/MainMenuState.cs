using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : UiBaseState
{
    public override void Enter()
    {
        uiManager.ChangeMenu(UiManager.MenuType.Menu);
        uiManager.ChangeMenu(UiManager.MenuType.BackGroundMenu);
    }
    public override void Exit()
    {
        uiManager.DisableMenu(UiManager.MenuType.Menu);
        uiManager.DisableMenu(UiManager.MenuType.BackGroundMenu);
    }
}
