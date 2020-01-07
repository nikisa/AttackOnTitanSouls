using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BS_BossOrbitState : FirstBossState
{
    //Inspector
    public List<OrbitManagerData> OrbitManagerList;

    //Private
    int countRadius;
    int countInitial;
    float orbitTimeStart;
    float angleRotation;
    float angularAcceleration;
    float angularDeceleration;
    float positionPointTime;
    float orientation;
    int index;

    public override void Enter() {
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
        if (BossOrbitManager.prova) {
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

                //bossOrbitManager.SetAllInitialPosition(countInitial , OrbitManagerList[i].orbitData[y]);
                //countInitial++;

                //if (OrbitManagerList[i].AngularAccelerationTime == 0) angularAcceleration = 0;

                //if (OrbitManagerList[i].AngularDecelerationTime == 0) angularDeceleration = 0;

                //if (OrbitManagerList[i].hasAngularDeleceration && OrbitManagerList[i].OrbitTravelTime >= OrbitManagerList[i].AngularAccelerationTime + OrbitManagerList[i].AngularDecelerationTime) {
                //    angularAcceleration = OrbitManagerList[i].AngularMaxSpeed / OrbitManagerList[i].AngularAccelerationTime;
                //    angularDeceleration = OrbitManagerList[i].AngularMaxSpeed / OrbitManagerList[i].AngularDecelerationTime;
                //}


                //bossOrbitManager.setObjectsPosition(OrbitManagerList[i].orbitData[y].SetupRadius , OrbitManagerList[i].orbitData[y].FinalRadius , index , positionPointTime , orientation);

                //bossOrbitManager.SetMasks(index , positionPointTime);
                //bossOrbitManager.MoveMasks(index, 1);
                //index++;
                orientation -= 360 / OrbitManagerList.Count;

            }
        }

        //index = 0;




        //for (int i = 0; i < OrbitManagerList.Count; i++) {
        //    for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
        //        if (OrbitManagerList[i].orbitData[y].HasDeltaRadius) {
        //            bossOrbitManager.MoveRadius(OrbitManagerList[i].orbitData[y].FinalRadius, countRadius, OrbitManagerList[i].orbitData[y].SetupRadius, OrbitManagerList[i].orbitData[y].TravelTime, OrbitManagerList[i].orbitData[y].OrbitMoveToEase);
        //        }
        //        Debug.Log(countRadius);
        //        countRadius++;
        //    }
        //}

        //countRadius = 0;
    }

    public void OrbitTick() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            Debug.Log("PROBLEMA: " + OrbitManagerList[i].CenterRotation);
            bossOrbitManager.RotationMove(OrbitManagerList[i].AngularMaxSpeed, OrbitManagerList[i].AngularAccelerationTime, OrbitManagerList[i].CenterRotation);
        }

    }

    public void OrbitExit() {

        BossOrbitManager.prova = false;

        bossOrbitManager.EmptyOrbitDataList();

        for (int i = 0; i < OrbitManagerList.Count; i++) {
            OrbitManagerList[i].CenterRotation.MoveSpeed = 0;
        }
    }




    public void SetUpPositionPoints() {
            for (int i = 0; i < bossOrbitManager.OrbitDataList.Count; i++) {
                bossOrbitManager.SetObjectsPosition(bossOrbitManager.OrbitDataList[i].SetupRadius, bossOrbitManager.OrbitDataList[i].FinalRadius, index, positionPointTime, orientation, bossOrbitManager.OrbitDataList[i].TravelTime , bossOrbitManager.OrbitDataList[i].HasDeltaRadius);
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
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
                bossOrbitManager.FillOrbitData(OrbitManagerList[i].orbitData[y]);
            }
        }
    }

    public void UpdateOrbitData() {
        bossOrbitManager.RemoveOrbitDataList();
    }
}
