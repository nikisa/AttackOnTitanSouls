using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_BossOrbitState : BossBaseState
{
    public OrbitData orbitData;
    public BossController.TargetsOrbit centerPoint;
    [HideInInspector]
    public GameObject CenterPoint;
    public BossController.TargetsOrbit center;
    [HideInInspector]
    public GameObject Center;
    public BossController.TargetsOrbit tentacle;
    [HideInInspector]
    public GameObject Tentacle;
    //private
    float angleRotation;
    float currentRadius;
    float initialPosition;


    public override void Enter()
    {
        SetTargets();
        OrbitEnter();   
    }
    public override void Tick()
    {
        OrbitTick();
        Debug.Log(CenterPoint.transform.localEulerAngles.y);
    }
    public override void Exit()
    {

    }

    public void OrbitEnter() {
        CenterPoint.transform.SetParent(Center.transform);
        //Tentacle.transform.localScale = new Vector3(Tentacle.transform.localScale.x, InitialRadius, Tentacle.transform.localScale.z); viaggione onirico sul tentacolo
        Tentacle.transform.SetParent(CenterPoint.transform);
        Tentacle.transform.position = new Vector3(Tentacle.transform.position.x + orbitData.InitialRadius, Tentacle.transform.position.y, Tentacle.transform.position.z);
        initialPosition = orbitData.InitialRadius;
    }

    public void OrbitTick() {
        angleRotation = Time.deltaTime * orbitData.OrbitSpeed;
        if (CenterPoint.transform.localEulerAngles.y <= orbitData.Angle) {
            
            if (orbitData.HasDeltaRadius) {
                Debug.LogFormat("currentRotation{0},finalPossition{1},FInalAngle{2}", CenterPoint.transform.localEulerAngles.y, orbitData.FinalRadius, orbitData.Angle);
                Tentacle.transform.localPosition = new Vector3(toFinalRadious(CenterPoint.transform.localEulerAngles.y, orbitData.FinalRadius, orbitData.Angle), Tentacle.transform.localPosition.y, Tentacle.transform.localPosition.z);

                Mathf.Clamp(Tentacle.transform.localPosition.x, initialPosition, orbitData.FinalRadius);
            }
           CenterPoint.transform.Rotate(Vector3.up * angleRotation);

        }
        else {
            animator.SetTrigger("Idle");
        }
    }

    public void OrbitExit() {
        
    }

    float toFinalRadious(float _curreRadious , float _finalPosition , float _angleRotation) {
        _finalPosition = orbitData.FinalRadius - orbitData.InitialRadius;
        float currentPosition = (_curreRadious * _finalPosition) / _angleRotation;

        if (currentRadius < orbitData.InitialRadius) {
            return currentPosition + orbitData.InitialRadius;
        }
        else {
            return currentPosition;
        }
    }
    public void SetTargets()
    {
       CenterPoint=boss.SetTargetOrbit(centerPoint);
       Center = boss.SetTargetOrbit(center);
       Tentacle = boss.SetTargetOrbit(tentacle);
    }
}
