using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_BossOrbitState : BossBaseState
{

    //Inspector
    public List <OrbitData> orbitData;
    public OrbitManagerData orbitManagerData;

    //Private
    float orbitTimeStart;
    BossOrbitManager orbitManager;
    float angleRotation;
    float currentRadius;

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

    }

    public void OrbitEnter() {
        //CenterPoint.transform.SetParent(Center.transform);
        ////Tentacle.transform.localScale = new Vector3(Tentacle.transform.localScale.x, InitialRadius, Tentacle.transform.localScale.z); viaggione onirico sul tentacolo
        //Tentacle.transform.SetParent(CenterPoint.transform);
        //Tentacle.transform.position = new Vector3(Tentacle.transform.position.x + orbitData.InitialRadius, Tentacle.transform.position.y, Tentacle.transform.position.z);
        //initialPosition = orbitData.InitialRadius;
        for (int i = 0; i < orbitData.Count; i++)
        {
            //girare le maschere e vedere se funziona
            orbitManager.SetAllInitialPosition(i, orbitData[i]);
        }
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
        orbitManager.RotationMove(orbitManagerData.speedRadius , orbitManagerData.MaxSpeed);


        for (int i = 0; i < orbitData.Count; i++)
        {
            if (orbitData[i].HasDeltaRadius)
            {
                orbitManager.MoveRadius(orbitData[i].FinalRadius, i, orbitData[i].initialPosition, orbitData[i].HasPingPong , orbitManagerData.speedRadius);
            }
        }
    }

    public void OrbitExit() {
        
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
