using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossOrbitSetupState : FirstBossState
{
    public List<OrbitManagerData> OrbitManagerList;
    BossOrbitManager orbitManager;
    int orbitCount = 0;
    int countInitial = 0;


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

            if (OrbitManagerList[i].CenterRotation.transform.childCount == 0) {
                Destroy(OrbitManagerList[i].CenterRotation);
            }
        }

        

    }

    public override void Tick() {//Aggiungere una condizione per evitare che faccia doppio for in Tick
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
                //orbitManager.SetAllInitialPosition(i, OrbitManagerList[i].orbitData[y]);
                orbitManager.SetInitial(OrbitManagerList[i].orbitData[y].SetupRadius, y, OrbitManagerList[i].orbitData[y]);
                countInitial++;
            }
        }
    }


}
