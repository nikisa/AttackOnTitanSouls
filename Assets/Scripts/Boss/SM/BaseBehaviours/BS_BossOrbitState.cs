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

    // Se bossOrbitManager.SetMasksRotation(OrbitManagerList) è presente , le funzioni sotto non vengono chiamate [SET PARENT]
    public override void Enter() {
        ClearOrbitManagerDataList();
        FillOrbitManagerDataList();
        bossOrbitManager.SetMasksRotation(OrbitManagerList);
        bossOrbitManager.countMasksArrived = 0;
        orientation = 360;
        ResetPosition();
        SetOrbitData();
        UpdateOrbitData();
        SetUpPositionPoints();
        orbitTimeStart = Time.time;
    }


    public override void Tick() {
        Debug.Log(bossOrbitManager.countMasksArrived);

        if (bossOrbitManager.countMasksArrived >= bossOrbitManager.HookPointList.Count) {
            OrbitTick();
        }
    }

    public override void Exit() {
        bossOrbitManager.actualSpeed = OrbitManagerList[0].CenterRotation.MoveSpeed;
    }


    public void OrbitTick() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            Debug.Log("PROBLEMA: " + OrbitManagerList[i].CenterRotation);
            bossOrbitManager.RotationMove(OrbitManagerList[i].AngularMaxSpeed, OrbitManagerList[i].AngularAccelerationTime, OrbitManagerList[i].CenterRotation);
        }
    }

    public void SetUpPositionPoints() {
        for (int i = 0; i < bossOrbitManager.OrbitManagerDataList.Count; i++) {
            for (int y = 0; y < bossOrbitManager.OrbitManagerDataList[i].orbitData.Count; y++) {
                bossOrbitManager.SetObjectsPosition(bossOrbitManager.OrbitManagerDataList[i].orbitData[y].SetupRadius, bossOrbitManager.OrbitManagerDataList[i].orbitData[y].FinalRadius, index, positionPointTime, orientation, bossOrbitManager.OrbitManagerDataList[i].orbitData[y].TravelTime, bossOrbitManager.OrbitManagerDataList[i].orbitData[y].HasDeltaRadius, bossOrbitManager.OrbitDataList[i].isSetup);
                index++;
                orientation -= 360 / bossOrbitManager.EndPoints.Count;
            }
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
                count++;
                bossOrbitManager.FillOrbitData(OrbitManagerList[i].orbitData[y]);
            }
        }
    }

    public void UpdateOrbitData() {
        bossOrbitManager.RemoveOrbitDataList();
    }

    public void FillOrbitManagerDataList() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            bossOrbitManager.OrbitManagerDataList.Add(OrbitManagerList[i]);
            OrbitManagerList[i].CenterRotation.MoveSpeed = bossOrbitManager.actualSpeed;
        }
    }

    public void ClearOrbitManagerDataList() {
        bossOrbitManager.OrbitManagerDataList.Clear();
    }
}