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
    }
    public override void Exit()
    {
        OrbitExit();
    }

    public void OrbitEnter() {
        boss.Data.orbit.CenterPoint.transform.SetParent(boss.Data.orbit.Center.transform);
        //Tentacle.transform.localScale = new Vector3(Tentacle.transform.localScale.x, InitialRadius, Tentacle.transform.localScale.z); viaggione onirico sul tentacolo
        boss.Data.orbit.Tentacle.transform.SetParent(boss.Data.orbit.CenterPoint.transform);
        boss.Data.orbit.Tentacle.transform.position = new Vector3(boss.Data.orbit.Tentacle.transform.position.x + boss.Data.orbitInfo.InitialRadius, boss.Data.orbit.Tentacle.transform.position.y, boss.Data.orbit.Tentacle.transform.position.z);
        initialPosition = boss.Data.orbitInfo.InitialRadius;
        Debug.Log(initialPosition + "qui");
        
    }

    public void OrbitTick() {
        angleRotation = Time.deltaTime * boss.Data.orbitInfo.OrbitSpeed;
        if (boss.Data.orbit.CenterPoint.transform.localEulerAngles.y <= boss.Data.orbitInfo.Angle) {
            //if (HasDeltaRadius)
            //{
            //    currentRadius = CenterPoint.transform.localEulerAngles.y / Angle * 100;
            //    Tentacle.transform.position += new Vector3(angleRotation/100, Tentacle.transform.position.y, Tentacle.transform.position.z);
            //    Debug.Log(Tentacle.transform.position.x);
            //}
            //Debug.Log(CenterPoint.transform.localEulerAngles.y);
            boss.Data.orbit.CenterPoint.transform.Rotate(Vector3.up * angleRotation);
        }
        else {
            animator.SetTrigger("Idle");
        }
    }

    public void OrbitExit() {
        
    }
}
