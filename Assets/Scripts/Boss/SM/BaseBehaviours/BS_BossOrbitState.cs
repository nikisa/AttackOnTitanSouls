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

    public override void Enter()
    {
        orientation = 360;
        ResetPosition();
        //FillPointsPosition();
        SetCenterPoint();
        SetUpPositionPoints();

        orbitTimeStart = Time.time;
        OrbitEnter();
    }


    public override void Tick()
    {
        OrbitTick();
    }

    public override void Exit()
    {
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
                index++;
                orientation -= 360 / OrbitManagerList.Count;

            }
        }

        index = 0;




        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
                if (OrbitManagerList[i].orbitData[y].HasDeltaRadius) {
                    bossOrbitManager.MoveRadius(OrbitManagerList[i].orbitData[y].FinalRadius, countRadius, OrbitManagerList[i].orbitData[y].SetupRadius, OrbitManagerList[i].orbitData[y].TravelTime , OrbitManagerList[i].orbitData[y].OrbitMoveToEase);
                }
                Debug.Log(countRadius);
                countRadius++;
            }
        }

        countRadius = 0;
    }

    public void OrbitTick() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            bossOrbitManager.RotationMove(OrbitManagerList[i].AngularMaxSpeed, OrbitManagerList[i].AngularAccelerationTime, OrbitManagerList[i].CenterRotation);
        }

        for (int i = 0; i < bossOrbitManager.OrbitList.Count; i++) {
            bossOrbitManager.OrbitList[i].transform.position = bossOrbitManager.EndPoints[i].transform.position;
        }
    }

    public void OrbitExit() {
        for (int i = 0; i < OrbitManagerList.Count; i++)
        {
            OrbitManagerList[i].CenterRotation.MoveSpeed = 0;
        }
    }

    //public void FillPointsPosition() {
    //    GameObject InitialPosition;
    //    GameObject EndPosition;
    //    for (int i = 0; i < bossOrbitManager.HookPointList.Count; i++) {
    //        InitialPosition = GameObject.Instantiate(Resources.Load("PointPosition") as GameObject, bossOrbitManager.transform);
    //        EndPosition = GameObject.Instantiate(Resources.Load("PointPosition") as GameObject, bossOrbitManager.transform);
    //        bossOrbitManager.InitialPoints.Add(InitialPosition);
    //        bossOrbitManager.EndPoints.Add(EndPosition);
    //    }
    //    for (int i = 0; i < bossOrbitManager.HookPointList.Count; i++) {
    //        Debug.Log(bossOrbitManager.transform.position);
    //        bossOrbitManager.InitialPoints[i].transform.position = bossOrbitManager.transform.position;
    //        bossOrbitManager.EndPoints[i].transform.position = bossOrbitManager.transform.position;
    //    }
    //}

    public void SetCenterPoint() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            OrbitManagerList[i].CenterRotation = GameObject.Instantiate(Resources.Load("CenterPoint") as GameObject, bossOrbitManager.transform).GetComponent<HookPointController>();
            OrbitManagerList[i].CenterRotation.transform.SetParent(boss.transform);

            if (OrbitManagerList[i].CenterRotation.transform.childCount == 0) {
                Destroy(OrbitManagerList[i].CenterRotation);
            }
        }
    }

    public void SetUpPositionPoints() {
        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
                bossOrbitManager.SetObjectsPosition(OrbitManagerList[i].orbitData[y].SetupRadius, OrbitManagerList[i].orbitData[y].FinalRadius, index, positionPointTime, orientation);
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

}
