using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_BossOrbitState : BossBaseState
{

    //Inspector
    public List<OrbitManagerData> OrbitManagerList;
    

    //Private
    float orbitTimeStart;
    BossOrbitManager orbitManager;
    float angleRotation;
    float currentRadius;
    int countRadius;
    int countInitial;
    float angularAcceleration;
    float angularDeceleration;

    
    public override void Enter()
    {
        orbitTimeStart = Time.time;
        orbitManager = FindObjectOfType<BossOrbitManager>();
        //SetTargets();
        OrbitEnter();
        orbitManager.SetUp();
        orbitManager.SetMasks(OrbitManagerList);
    }


    public override void Tick()
    {
        //if (Time.time - orbitTimeStart < orbitManagerData.Time) {
        //    OrbitTick();
        //}
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
                orbitManager.SetAllInitialPosition(countInitial , OrbitManagerList[i].orbitData[y]);
                countInitial++;

                if (OrbitManagerList[i].AngularAccelerationTime == 0) angularAcceleration = 0;

                if (OrbitManagerList[i].AngularDecelerationTime == 0) angularDeceleration = 0;
                
                if (OrbitManagerList[i].hasAngularDeleceration && OrbitManagerList[i].OrbitTravelTime >= OrbitManagerList[i].AngularAccelerationTime + OrbitManagerList[i].AngularDecelerationTime) {
                    angularAcceleration = OrbitManagerList[i].AngularMaxSpeed / OrbitManagerList[i].AngularAccelerationTime;
                    angularDeceleration = OrbitManagerList[i].AngularMaxSpeed / OrbitManagerList[i].AngularDecelerationTime;
                }
            }
        }

        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
                if (OrbitManagerList[i].orbitData[y].HasDeltaRadius) {
                    orbitManager.MoveRadius(OrbitManagerList[i].orbitData[y].FinalRadius, countRadius, OrbitManagerList[i].orbitData[y].initialPosition, OrbitManagerList[i].orbitData[y].TravelTime , OrbitManagerList[i].orbitData[y].OrbitMoveToEase);
                }
                Debug.Log(countRadius);
                countRadius++;
            }
        }
        countRadius = 0;
    }

    public void OrbitTick() {
        for (int i = 0; i < OrbitManagerList.Count; i++)
        {
            orbitManager.RotationMove(OrbitManagerList[i].AngularMaxSpeed, OrbitManagerList[i].AngularAccelerationTime, OrbitManagerList[i].CenterRotation);
        }
    }

    public void OrbitExit() {
        for (int i = 0; i < OrbitManagerList.Count; i++)
        {
            OrbitManagerList[i].CenterRotation.MoveSpeed = 0;
        }
    }
    

}
