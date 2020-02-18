using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTestScript : MonoBehaviour
{
    //Public
    public PlayerController Player;

    public float TimeAcceleration;
    public float MaxSpeed;

    public float vectorAngle;
    //public float Acceleration;
    //public float VelocityX;
    //public float VelocityZ;
    //public float Drag;

    public Vector3 OldPos;
    public Vector3 Inertia;
    public Vector3 AccelerationVector;
    public Vector3 VelocityVector;
    public Vector3 MaxspeedVector;
    public float Drag;
    public float AccelerationModule;



    //Private
    private Vector3 targetDir;



    private void Update() {

        //targetDir = Player.transform.position - transform.position;
        //vectorAngle = Vector3.SignedAngle(Vector3.forward , targetDir , Vector3.up) * Mathf.Deg2Rad;
        //Acceleration = MaxSpeed / TimeAcceleration;
        //Drag = Acceleration / MaxSpeed * Time.deltaTime;
        //transform.position = new Vector3(transform.position.x + VelocityX * Time.deltaTime + 0.5f * Acceleration * Mathf.Sin(vectorAngle) * Mathf.Pow(Time.deltaTime, 2), transform.position.y, transform.position.z + VelocityZ * Time.deltaTime + 0.5f * Acceleration * Mathf.Cos(vectorAngle) * Mathf.Pow(Time.deltaTime, 2));
        //VelocityX += Acceleration * Mathf.Sin(vectorAngle) * Time.deltaTime; 
        //VelocityZ += Acceleration * Mathf.Cos(vectorAngle) * Time.deltaTime;
        //VelocityX -= VelocityX * Drag;
        //VelocityZ -= VelocityZ * Drag;

        targetDir = Player.transform.position - transform.localPosition;
        vectorAngle = Vector3.SignedAngle(Vector3.forward, targetDir, Vector3.up) * Mathf.Deg2Rad;
        AccelerationModule = MaxSpeed / TimeAcceleration;
        AccelerationVector = new Vector3(Mathf.Sin(vectorAngle) * AccelerationModule, 0, Mathf.Cos(vectorAngle) * AccelerationModule);
        //boss.MaxSpeedVector = new Vector3(Mathf.Cos(boss.vectorAngle) * chaseData.MaxSpeed, boss.AccelerationVector.y, Mathf.Sin(boss.vectorAngle) * chaseData.MaxSpeed);
        Drag = AccelerationModule / MaxSpeed * Time.fixedDeltaTime;
        OldPos = transform.position;
        transform.localPosition += VelocityVector * Time.fixedDeltaTime + 0.5f * AccelerationVector * Mathf.Pow(Time.deltaTime, 2);
        VelocityVector += AccelerationVector * Time.fixedDeltaTime;
        VelocityVector -= VelocityVector * Drag;

        Debug.DrawLine(transform.position, transform.position + AccelerationVector, Color.red, .02f);
        Debug.DrawLine(transform.position, transform.position + VelocityVector , Color.blue, .02f);
        Debug.DrawLine(transform.position, Player.transform.position, Color.green, .02f);
        //Debug.DrawLine(boss.transform.position, boss.MaxSpeedVector, Color.green, .5f);

    }
}
