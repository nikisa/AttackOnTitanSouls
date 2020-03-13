using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHookPoint : HookPointBase, IGrappable {
    
    //Inspector
    public bool isHookable;
    public bool isHooked;


    //Private
    private float distance;
    BossOrbitManager bossOrbitManager;


    bool hasAnyMask() {
        bool result = true;
        if (bossOrbitManager.MasksList.Count == 0) {
            result = true;
        }
        return result;
    }

}