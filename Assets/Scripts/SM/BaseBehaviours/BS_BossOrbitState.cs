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
        boss.Data.orbitInfo.Tentacle.SetActive(true);
        boss.Data.orbitInfo.CenterPoint = Instantiate(GameObject.Instantiate(Resources.Load("CenterPoint") as GameObject), boss.Data.orbitInfo.Center.transform.position, Quaternion.identity); // istanzia un center in più perchè?
        boss.Data.orbitInfo.CenterPoint.transform.SetParent(boss.Data.orbitInfo.Center.transform);
        //Tentacle.transform.localScale = new Vector3(Tentacle.transform.localScale.x, InitialRadius, Tentacle.transform.localScale.z); viaggione onirico sul tentacolo
        boss.Data.orbitInfo.Tentacle.transform.SetParent(boss.Data.orbitInfo.CenterPoint.transform);
        boss.Data.orbitInfo.Tentacle.transform.position = new Vector3(boss.Data.orbitInfo.Tentacle.transform.position.x + boss.Data.orbitInfo.InitialRadius, boss.Data.orbitInfo.Tentacle.transform.position.y, boss.Data.orbitInfo.Tentacle.transform.position.z);
        initialPosition = 0 + boss.Data.orbitInfo.InitialRadius;
        Debug.Log(initialPosition + "qui");
    }

    public void OrbitTick() {
        angleRotation = Time.deltaTime * boss.Data.orbitInfo.OrbitSpeed;
        if (boss.Data.orbitInfo.CenterPoint.transform.localEulerAngles.y <= boss.Data.orbitInfo.Angle) {
            //if (HasDeltaRadius)
            //{
            //    currentRadius = CenterPoint.transform.localEulerAngles.y / Angle * 100;
            //    Tentacle.transform.position += new Vector3(angleRotation/100, Tentacle.transform.position.y, Tentacle.transform.position.z);
            //    Debug.Log(Tentacle.transform.position.x);
            //}
            //Debug.Log(CenterPoint.transform.localEulerAngles.y);
            boss.Data.orbitInfo.CenterPoint.transform.Rotate(Vector3.up * angleRotation);
        }
        else {
            animator.SetTrigger("Idle");
        }
    }

    public void OrbitExit() {
        boss.Data.orbitInfo.Tentacle.SetActive(false);
    }
}
