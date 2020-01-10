using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BS_BossOrbitState : FirstBossState
{
    //Inspector
    public List<OrbitManagerData> OrbitManagerList;

    //Private
    float orbitTimeStart;
    float angleRotation;
    float angularAcceleration;
    float angularDeceleration;
    float positionPointTime;
    float orientation;
    int index;

    public override void Enter() {
        bossOrbitManager.countMasksArrived = 0;
        orientation = 360;
        ResetPosition();
        SetOrbitData();
        UpdateOrbitData();
        SetUpPositionPoints();
        orbitTimeStart = Time.time;
        OrbitEnter();
        bossOrbitManager.SetMasksRotation(OrbitManagerList);
    }


    public override void Tick() {
        Debug.Log(bossOrbitManager.countMasksArrived);

        if (bossOrbitManager.countMasksArrived >= bossOrbitManager.HookPointList.Count) {
            OrbitTick();
        }
    }

    public override void Exit() {
        OrbitExit();
    }

    //SetInitial spostato in Orbit SetUp
    public void OrbitEnter() {

        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {

                orientation -= 360 / OrbitManagerList.Count;
            }
        }       
    }

    //Settare le maschere di questo OrbitManager al corrispettivo CenterRotation?
    public void OrbitTick() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            Debug.Log("PROBLEMA: " + OrbitManagerList[i].CenterRotation);
            bossOrbitManager.RotationMove(OrbitManagerList[i].AngularMaxSpeed, OrbitManagerList[i].AngularAccelerationTime, OrbitManagerList[i].CenterRotation);
        }
    }

    public void OrbitExit() {

        bossOrbitManager.EmptyOrbitDataList();

        for (int i = 0; i < OrbitManagerList.Count; i++) {
            OrbitManagerList[i].CenterRotation.MoveSpeed = 0;
        }
    }

    public void SetUpPositionPoints() {
            for (int i = 0; i < bossOrbitManager.OrbitDataList.Count; i++) {
                bossOrbitManager.SetObjectsPosition(bossOrbitManager.OrbitDataList[i].SetupRadius, bossOrbitManager.OrbitDataList[i].FinalRadius, index, positionPointTime, orientation, bossOrbitManager.OrbitDataList[i].TravelTime , bossOrbitManager.OrbitDataList[i].HasDeltaRadius , bossOrbitManager.OrbitDataList[i].isSetup);
                index++;
                orientation -= 360 / bossOrbitManager.EndPoints.Count;
            }
        index = 0;
    }

    public void ResetPosition() {
        for (int i = 0; i < bossOrbitManager.EndPoints.Count; i++) {
            bossOrbitManager.EndPoints[i].transform.position = boss.transform.position;
            bossOrbitManager.InitialPoints[i].transform.position = boss.transform.position;
        }
    }

    public void SetOrbitData() {
        int count = 1;
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
                Debug.Log("Contatore: "  + count);
                count++;
                bossOrbitManager.FillOrbitData(OrbitManagerList[i].orbitData[y]);
            }
        }
    }

    public void UpdateOrbitData() {
        bossOrbitManager.RemoveOrbitDataList();
    }
}
