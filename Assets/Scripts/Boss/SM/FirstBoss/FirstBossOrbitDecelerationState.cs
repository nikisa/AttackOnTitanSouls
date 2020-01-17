using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossOrbitDecelerationState : FirstBossState
{

    public override void Tick() {
        DecelerationTick();
    }

    public override void Exit() {
        bossOrbitManager.actualSpeed = bossOrbitManager.OrbitManagerDataList[0].CenterRotation.MoveSpeed;
    }

    public void DecelerationTick() {
        for (int i = 0; i < bossOrbitManager.OrbitManagerDataList.Count; i++) {
            bossOrbitManager.OrbitDeceleration(bossOrbitManager.OrbitManagerDataList[i].AngularMaxSpeed , bossOrbitManager.OrbitManagerDataList[i].AngularDecelerationTime , bossOrbitManager.OrbitManagerDataList[i].CenterRotation);
        }
    }

}
