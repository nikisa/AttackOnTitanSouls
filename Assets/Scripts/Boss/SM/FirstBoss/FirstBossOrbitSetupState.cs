﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossOrbitSetupState : FirstBossState
{
    //Inspector
    public List<MaskBehaviourData> MaskBehaviourList;

    //Private
    int orbitCount = 0;
    int countInitial = 0;
    int index = 0;
    float positionPointTime = 0.01f;
    float orientation;
    float maxMaskCurrentRadius;
    float currentRadius;


    public override void Enter()
    {

        bossOrbitManager.MasksBehaviourList = MaskBehaviourList;
        orientation = 360 + bossOrbitManager.SetupAngle;
        bossOrbitManager.SetupMask(MaskBehaviourList);
        FillPointsPosition();
        SetupPositionPoints(orientation);
        maxMaskCurrentRadius = 0;
        currentRadius = 0;
    }

    public override void Tick() {
        maxMaskCurrentRadius = bossOrbitManager.maxMaskCurrentRadius();
        UpdateCharacterControllerRadius(maxMaskCurrentRadius);
    }

    public override void Exit() {

    }

    void UpdateCharacterControllerRadius(float _maxMaskCurrentRadius) {
        currentRadius += Time.deltaTime * 10; //Moltiplico per n dato che radius riparte da 0 e c'è il rischio che durante un MoveTo il characterController non sia ancora arrivato al radius corretto
        currentRadius = Mathf.Clamp(currentRadius, 0, _maxMaskCurrentRadius);
        boss.CharacterController.radius = currentRadius;
    }

    public void FillPointsPosition() {
        GameObject InitialPosition;
        GameObject EndPosition;
        for (int i = 0; i < bossOrbitManager.MasksList.Count; i++) {
            InitialPosition = GameObject.Instantiate(Resources.Load("PointPosition") as GameObject , bossOrbitManager.transform);
            EndPosition = GameObject.Instantiate(Resources.Load("PointPosition") as GameObject , bossOrbitManager.transform);
            bossOrbitManager.InitialPoints.Add(InitialPosition);
            bossOrbitManager.EndPoints.Add(EndPosition);
        }
        for (int i = 0; i < bossOrbitManager.MasksList.Count; i++) {
            bossOrbitManager.InitialPoints[i].transform.position = bossOrbitManager.transform.position;
            bossOrbitManager.EndPoints[i].transform.position = bossOrbitManager.transform.position;
        }
    }

    //public void SetCenterPoint() {
    //    for (int i = 0; i < OrbitManagerList.Count; i++) {
    //        OrbitManagerList[i].CenterRotation = GameObject.Instantiate(Resources.Load("CenterPoint") as GameObject, bossOrbitManager.transform).GetComponent<HookPointController>();
    //        OrbitManagerList[i].CenterRotation.transform.SetParent(boss.transform);

    //        //if (OrbitManagerList[i].CenterRotation.transform.childCount == 0) {
    //        //    Destroy(OrbitManagerList[i].CenterRotation);
    //        //}
    //    }
    //}

    //public void SetupPositionPoints() {
    //    for (int i = 0; i < OrbitManagerList.Count; i++) {
    //        for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
    //            bossOrbitManager.SetObjectsPosition(OrbitManagerList[i].orbitData[y].SetupRadius, OrbitManagerList[i].orbitData[y].FinalRadius, index, positionPointTime, orientation, OrbitManagerList[i].orbitData[y].TravelTime, OrbitManagerList[i].orbitData[y].HasDeltaRadius, OrbitManagerList[i].orbitData[y].isSetup);
    //            index++;
    //            orientation -= 360 / bossOrbitManager.EndPoints.Count;
    //        }
    //    }
    //    index = 0;
    //}

    public void SetupPositionPoints(float _orientation) {
        for (int i = 0; i < MaskBehaviourList.Count; i++) {
            for (int j = 0; j < MaskBehaviourList[i].MaskTargets.Count; j++) {
                bossOrbitManager.SetObjectsPosition(MaskBehaviourList[i].SetupRadius, MaskBehaviourList[i].FinalRadius, j, positionPointTime, orientation, MaskBehaviourList[i].TravelTime, MaskBehaviourList[i].HasDeltaRadius, MaskBehaviourList[i].isSetup);
                orientation -= _orientation / bossOrbitManager.MasksList.Count;
            }
            

        }
    }
}
