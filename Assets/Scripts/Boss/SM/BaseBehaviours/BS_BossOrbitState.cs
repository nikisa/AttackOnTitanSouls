using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BS_BossOrbitState : FirstBossState
{
    //Inspector
    public List<MaskBehaviourData> MasksBehaviourList; 



    //Private
    float orbitTimeStart;
    float angleRotation;
    float angularAcceleration;
    float angularDeceleration;
    float positionPointTime;
    float orientation;
    int index;

    public override void Enter() {

        bossOrbitManager.SetupMask(MasksBehaviourList);
        bossOrbitManager.BossFov.UpdateViewRadius();
        bossOrbitManager.MasksBehaviourList = MasksBehaviourList;

        orientation = 360;
        //ResetPosition();
        SetupPositionPoints();
        orbitTimeStart = Time.time;
    }


    public override void Tick() {
        bossOrbitManager.RotationMask(MasksBehaviourList);
        bossOrbitManager.SetCurrentRadius(MasksBehaviourList);
    }

    public override void Exit() {
        
    }

    
    public void SetupPositionPoints() {
        for (int i = 0; i < MasksBehaviourList.Count; i++) {
            bossOrbitManager.SetObjectsPosition(MasksBehaviourList[i].SetupRadius, MasksBehaviourList[i].FinalRadius, i, positionPointTime, orientation, MasksBehaviourList[i].TravelTime, MasksBehaviourList[i].HasDeltaRadius, MasksBehaviourList[i].isSetup);
            orientation -= 360 / bossOrbitManager.MasksList.Count;
        }
    }

    public void ResetPosition() {
        for (int i = 0; i < bossOrbitManager.EndPoints.Count; i++) {
            bossOrbitManager.EndPoints[i].transform.position = boss.transform.position;
            bossOrbitManager.InitialPoints[i].transform.position = boss.transform.position;
        }
    }



    #region Functions Cemetery

    //public void SetOrbitData() {
    //    for (int i = 0; i < OrbitManagerList.Count; i++) {
    //        for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
    //            bossOrbitManager.FillOrbitData(OrbitManagerList[i].orbitData[y]);
    //        }
    //    }
    //}

    //public void UpdateOrbitData() {
    //    bossOrbitManager.RemoveOrbitDataList();
    //}

    //public void FillOrbitManagerDataList() {
    //    for (int i = 0; i < OrbitManagerList.Count; i++) {
    //        bossOrbitManager.OrbitManagerDataList.Add(OrbitManagerList[i]);
    //        OrbitManagerList[i].CenterRotation.MoveSpeed = bossOrbitManager.actualSpeed;
    //    }
    //}

    //public void ClearOrbitManagerDataList() {
    //    bossOrbitManager.OrbitManagerDataList.Clear();
    //}

    //public void SetUpPositionPoints() {

    //    for (int i = 0; i < bossOrbitManager.OrbitManagerDataList.Count; i++) {
    //        for (int y = 0; y < bossOrbitManager.OrbitManagerDataList[i].orbitData.Count; y++) {
    //            bossOrbitManager.SetObjectsPosition(bossOrbitManager.OrbitManagerDataList[i].orbitData[y].SetupRadius, bossOrbitManager.OrbitManagerDataList[i].orbitData[y].FinalRadius, index, positionPointTime, orientation, bossOrbitManager.OrbitManagerDataList[i].orbitData[y].TravelTime, bossOrbitManager.OrbitManagerDataList[i].orbitData[y].HasDeltaRadius, bossOrbitManager.OrbitDataList[i].isSetup);
    //            index++;
    //            orientation -= 360 / bossOrbitManager.HookPointList.Count;
    //        }
    //    }
    //    index = 0;
    //}


    #endregion

}