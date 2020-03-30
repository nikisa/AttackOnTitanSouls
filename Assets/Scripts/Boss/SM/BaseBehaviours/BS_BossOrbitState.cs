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
    float maxMaskCurrentRadius;
    float currentRadius;

    public override void Enter() {
        
        bossOrbitManager.MasksBehaviourList = MasksBehaviourList;
        bossOrbitManager.ResetVelocity();
        bossOrbitManager.SetupMask(MasksBehaviourList);
        
        bossOrbitManager.BossFov.UpdateViewRadius();
        

        orientation = 360  + bossOrbitManager.SetupAngle;
        //ResetPosition();
        SetupPositionPoints(orientation);
        orbitTimeStart = Time.time;
        maxMaskCurrentRadius = 0;
        currentRadius = 0;
    }


    public override void Tick() {
        bossOrbitManager.RotationMask(MasksBehaviourList);
        bossOrbitManager.SetCurrentRadius(MasksBehaviourList);
        maxMaskCurrentRadius = bossOrbitManager.maxMaskCurrentRadius();
        UpdateCharacterControllerRadius(maxMaskCurrentRadius);
    }

    public override void Exit() {
        
    }


    void UpdateCharacterControllerRadius(float _maxMaskCurrentRadius) {
        currentRadius += Time.deltaTime * 10; //Moltiplico per n dato che radius riparte da 0 e c'è il rischio che durante un MoveTo il characterController non sia ancora arrivato al radius corretto
        currentRadius = Mathf.Clamp(currentRadius, 0, _maxMaskCurrentRadius);
        boss.CharacterController.radius = currentRadius;
        boss.sphereCollider.radius = currentRadius;
    }

    public void SetupPositionPoints(float _orientation) {
        for (int i = 0; i < MasksBehaviourList.Count; i++) {
            for (int j = 0; j < MasksBehaviourList[i].MaskTargets.Count; j++) {
                bossOrbitManager.SetObjectsPosition(MasksBehaviourList[i].SetupRadius, MasksBehaviourList[i].FinalRadius, j, positionPointTime, orientation, MasksBehaviourList[i].TravelTime, MasksBehaviourList[i].HasDeltaRadius, MasksBehaviourList[i].isSetup);
                orientation -= _orientation / bossOrbitManager.MasksList.Count;
            }
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