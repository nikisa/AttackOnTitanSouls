using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossOrbitDecelerationState : FirstBossState
{

    public override void Tick() {
        bossOrbitManager.DecelerationMask();
    }

}
