using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_BossOrbitState : BossBaseState
{
    //private
    float angleRotation;
    float currentRadius;
    float initialPosition;

    public override void Enter()
    {
        OrbitEnter();   
    }
    public override void Tick()
    {
        OrbitTick();
        Debug.Log(boss.Data.orbit.CenterPoint.transform.localEulerAngles.y);
    }
    public override void Exit()
    {

    }

    public void OrbitEnter() {
        boss.Data.orbit.CenterPoint.transform.SetParent(boss.Data.orbit.Center.transform);
        //Tentacle.transform.localScale = new Vector3(Tentacle.transform.localScale.x, InitialRadius, Tentacle.transform.localScale.z); viaggione onirico sul tentacolo
        boss.Data.orbit.Tentacle.transform.SetParent(boss.Data.orbit.CenterPoint.transform);
        boss.Data.orbit.Tentacle.transform.position = new Vector3(boss.Data.orbit.Tentacle.transform.position.x + boss.Data.orbitInfo.InitialRadius, boss.Data.orbit.Tentacle.transform.position.y, boss.Data.orbit.Tentacle.transform.position.z);
        initialPosition = boss.Data.orbitInfo.InitialRadius;
    }

    public void OrbitTick() {
        angleRotation = Time.deltaTime * boss.Data.orbitInfo.OrbitSpeed;
        if (boss.Data.orbit.CenterPoint.transform.localEulerAngles.y <= boss.Data.orbitInfo.Angle) {
            
            if (boss.Data.orbitInfo.HasDeltaRadius) {
                Debug.LogFormat("currentRotation{0},finalPossition{1},FInalAngle{2}", boss.Data.orbit.CenterPoint.transform.localEulerAngles.y, boss.Data.orbitInfo.FinalRadius, boss.Data.orbitInfo.Angle);
                boss.Data.orbit.Tentacle.transform.localPosition = new Vector3(toFinalRadious(boss.Data.orbit.CenterPoint.transform.localEulerAngles.y, boss.Data.orbitInfo.FinalRadius, boss.Data.orbitInfo.Angle), boss.Data.orbit.Tentacle.transform.localPosition.y, boss.Data.orbit.Tentacle.transform.localPosition.z);

                Mathf.Clamp(boss.Data.orbit.Tentacle.transform.localPosition.x, initialPosition, boss.Data.orbitInfo.FinalRadius);
            }
            boss.Data.orbit.CenterPoint.transform.Rotate(Vector3.up * angleRotation);

        }
        else {
            animator.SetTrigger("Idle");
        }
    }

    public void OrbitExit() {
        
    }

    float toFinalRadious(float _curreRadious , float _finalPosition , float _angleRotation) {
        _finalPosition = boss.Data.orbitInfo.FinalRadius - boss.Data.orbitInfo.InitialRadius;
        float currentPosition = (_curreRadious * _finalPosition) / _angleRotation;

        if (currentRadius < boss.Data.orbitInfo.InitialRadius) {
            return currentPosition + boss.Data.orbitInfo.InitialRadius;
        }
        else {
            return currentPosition;
        }
    }
}
