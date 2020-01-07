using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossOrbitSetupState : FirstBossState
{
    //Inspector
    public List<OrbitManagerData> OrbitManagerList;

    //Private
    int orbitCount = 0;
    int countInitial = 0;
    int index = 0;
    float positionPointTime = 0.01f;
    float orientation;


    /// <summary>
    /// Mettere un controllo per vedere se il centerpoint è vuoto , in tal caso si fa Instantiate , altrimenti no
    /// </summary>
    public override void Enter()
    {
        orientation = 360;
        SetCenterPoint();
        FillPointsPosition();
        SetUpPositionPoints();
        
    }

    public override void Tick() {//Aggiungere una condizione per evitare che faccia doppio for in Tick

        //for (int i = 0; i < OrbitManagerList.Count; i++) {
        //    for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
        //        //orbitManager.SetAllInitialPosition(i, OrbitManagerList[i].orbitData[y]);
        //        bossOrbitManager.SetInitial(OrbitManagerList[i].orbitData[y].SetupRadius, y, OrbitManagerList[i].orbitData[y]);
        //        countInitial++;
        //    }
        //}


    }

    public override void Exit() {
        BossOrbitManager.prova = false;
    }

    public void FillPointsPosition() {
        GameObject InitialPosition;
        GameObject EndPosition;
        for (int i = 0; i < bossOrbitManager.HookPointList.Count; i++) {
            InitialPosition = GameObject.Instantiate(Resources.Load("PointPosition") as GameObject , bossOrbitManager.transform);
            EndPosition = GameObject.Instantiate(Resources.Load("PointPosition") as GameObject , bossOrbitManager.transform);
            bossOrbitManager.InitialPoints.Add(InitialPosition);
            bossOrbitManager.EndPoints.Add(EndPosition);
        }
        for (int i = 0; i < bossOrbitManager.HookPointList.Count; i++) {
            Debug.Log(bossOrbitManager.transform.position);
            bossOrbitManager.InitialPoints[i].transform.position = bossOrbitManager.transform.position;
            bossOrbitManager.EndPoints[i].transform.position = bossOrbitManager.transform.position;
        }
    }

    public void SetCenterPoint() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            OrbitManagerList[i].CenterRotation = GameObject.Instantiate(Resources.Load("CenterPoint") as GameObject, bossOrbitManager.transform).GetComponent<HookPointController>();
            OrbitManagerList[i].CenterRotation.transform.SetParent(boss.transform);

            //if (OrbitManagerList[i].CenterRotation.transform.childCount == 0) {
            //    Destroy(OrbitManagerList[i].CenterRotation);
            //}
        }
    }

    public void SetUpPositionPoints() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
                bossOrbitManager.SetObjectsPosition(OrbitManagerList[i].orbitData[y].SetupRadius, OrbitManagerList[i].orbitData[y].FinalRadius , index, positionPointTime , orientation, OrbitManagerList[i].orbitData[y].TravelTime , OrbitManagerList[i].orbitData[y].HasDeltaRadius);
                index++;
                orientation -= 360 / bossOrbitManager.EndPoints.Count;
            }
        }
        index = 0;
    }
}
