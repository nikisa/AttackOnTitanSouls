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


    private void Awake() {
        //for (int i = 0; i < orbitData.Count; i++) {
        //    orbitManager.SetInitial(orbitData[i].InitialRadius, i, orbitData[i]);
        //}
        
    }


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
        //CenterPoint.transform.SetParent(Center.transform);
        ////Tentacle.transform.localScale = new Vector3(Tentacle.transform.localScale.x, InitialRadius, Tentacle.transform.localScale.z); viaggione onirico sul tentacolo
        //Tentacle.transform.SetParent(CenterPoint.transform);
        //Tentacle.transform.position = new Vector3(Tentacle.transform.position.x + orbitData.InitialRadius, Tentacle.transform.position.y, Tentacle.transform.position.z);
        //initialPosition = orbitData.InitialRadius;

        for (int i = 0; i < OrbitManagerList.Count; i++) {
            for (int y = 0; y < OrbitManagerList[i].orbitData.Count; y++) {
                orbitManager.SetAllInitialPosition(countInitial , OrbitManagerList[i].orbitData[y]);
                countInitial++;
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
        //angleRotation = Time.deltaTime * orbitData.OrbitSpeed;
        //if (CenterPoint.transform.localEulerAngles.y <= orbitData.Angle) {

        //    if (orbitData.HasDeltaRadius) {

        //        Tentacle.transform.localPosition = new Vector3(toFinalRadious(CenterPoint.transform.localEulerAngles.y, orbitData.FinalRadius, orbitData.Angle), Tentacle.transform.localPosition.y, Tentacle.transform.localPosition.z);

        //        Mathf.Clamp(Tentacle.transform.localPosition.x, initialPosition, orbitData.FinalRadius);
        //    }
        //   CenterPoint.transform.Rotate(Vector3.up * angleRotation);

        //}
        //else {
        //    animator.SetTrigger("Idle");
        //}

        for (int i = 0; i < OrbitManagerList.Count; i++)
        {
            orbitManager.RotationMove(OrbitManagerList[i].AngularMaxSpeed, OrbitManagerList[i].AngularAccelerationTime, OrbitManagerList[i].CenterRotation);
            Debug.Log("YYYYYYYYYY");
        }
    }

    public void OrbitExit() {
        for (int i = 0; i < OrbitManagerList.Count; i++)
        {
            OrbitManagerList[i].CenterRotation.MoveSpeed = 0;
        }
    }
    

    //float toFinalRadious(float _curreRadious , float _finalPosition , float _angleRotation) {
    //    _finalPosition = orbitData.FinalRadius - orbitData.InitialRadius;
    //    float currentPosition = (_curreRadious * _finalPosition) / _angleRotation;

    //    if (currentRadius < orbitData.InitialRadius) {
    //        return currentPosition + orbitData.InitialRadius;
    //    }
    //    else {
    //        return currentPosition;
    //    }
    //}
    //public void SetTargets()
    //{
    //   CenterPoint=boss.SetTargetOrbit(centerPoint);
    //   Center = boss.SetTargetOrbit(center);
    //   Tentacle = boss.SetTargetOrbit(tentacle);
    //}
}
