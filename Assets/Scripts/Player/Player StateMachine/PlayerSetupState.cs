using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetupState : PlayerBaseState
{
    public PlayerIdleData playerIdleData;
    public PlayerDashData playerDashData;

    public override void Enter() {
        player.playerIdleData = playerIdleData;
        player.playerDashData = playerDashData;
    }
}
