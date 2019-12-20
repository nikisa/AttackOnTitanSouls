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
        BossCore.transform.GetChild(0).gameObject.SetActive(true);
        BossHookPoint = BossCore.transform.GetChild(0).gameObject.GetComponent<HookPoint>();
    }

    public override void Tick() {

        if (BossHookPoint.isHooked) {
            Debug.Log("YOU WON");
            SceneManager.LoadScene(3);
        }



    }

    public override void Exit() {
        
    }


}
