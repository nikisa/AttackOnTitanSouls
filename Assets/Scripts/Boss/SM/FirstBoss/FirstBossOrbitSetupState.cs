using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossOrbitSetupState : FirstBossState
{
    public List<OrbitManagerData> OrbitManagerList;
    BossOrbitManager orbitManager;
    int orbitCount = 0;


    /// <summary>
    /// Mettere un controllo per vedere se il centerpoint è vuoto , in tal caso si fa Instantiate , altrimenti no
    /// </summary>
    public override void Enter()
    {
        orbitManager = FindObjectOfType<BossOrbitManager>();
        for (int i = 0; i < OrbitManagerList.Count; i++)
        {
            OrbitManagerList[i].CenterRotation = GameObject.Instantiate(Resources.Load("CenterPoint") as GameObject, orbitManager.transform).GetComponent<HookPointController>();
            OrbitManagerList[i].CenterRotation.transform.SetParent(boss.transform);            
        }
    }
}
