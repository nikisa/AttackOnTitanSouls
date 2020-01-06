using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstBossVulnerableState : FirstBossState {

    //Private
    GameObject BossCore;
    HookPoint BossHookPoint;

    public override void Enter() {
        animator.SetInteger(MASKS_COUNT, 99);
        BossCore = GameObject.FindGameObjectWithTag("BossCore");
        BossCore.transform.GetChild(0).gameObject.SetActive(true); //Set active the collider to permit the Victory statement
        BossHookPoint = BossCore.transform.GetChild(0).gameObject.GetComponent<HookPoint>();
    }

    public override void Tick() {
        if (BossHookPoint.isHooked) {
            PlayerController.VictoryEvent();
        }
    }

    public override void Exit() {
        
    }


}
