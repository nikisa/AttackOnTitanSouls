﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTestScript : MonoBehaviour
{
    //Public
    public PlayerController Player;
    public CharacterController CharacterController;
    public ChaseTestScript OrbitSphere;

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

    public float Mass;


    //Private
    private Vector3 targetDir;
    private Vector3 move;


    //Temporary public collisionTest
    public Vector3 normal;
    public float normalAngle;
    public Vector3 vectorParal;
    public Vector3 vectorPerp;
    public Vector3 collisionVectorParal;
    public Vector3 collisionVectorPerp;
    public Vector3 bounceVector;
    [Range(0, 1)]
    public float KineticEnergyLoss;
    [Range(0, 1)]
    public float SurfaceFriction;
   


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
        move = VelocityVector * Time.fixedDeltaTime + 0.5f * AccelerationVector * Mathf.Pow(Time.deltaTime, 2);
        VelocityVector += AccelerationVector * Time.fixedDeltaTime;
        VelocityVector -= VelocityVector * Drag;
        CharacterController.Move(move + Vector3.down * 9.81f);

        //Debug.DrawLine(transform.position, transform.position + AccelerationVector, Color.red, .02f);
        //Debug.DrawLine(transform.position, transform.position + VelocityVector , Color.blue, .02f);
        //Debug.DrawLine(transform.position, Player.transform.position, Color.green, .02f);

        //Debug.DrawLine(boss.transform.position, boss.MaxSpeedVector, Color.green, .5f);

    }

    /*
     * Masse dei due oggetti
     * Velocity Vector dei 2 ogg
     * 
     * Angolo tra il pivot dei due oggetti
     * 
     * Scomporre la forza in 2 var --> VectorParal e VectorPerp
     * 
     * BouncingVector dei 2 ogg
     */

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.GetComponent<PlayerController>()) {

            Debug.Log("Collision");

            Player = collision.transform.GetComponent<PlayerController>();


            Vector3 fakePlayerPosition = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
            normal = (fakePlayerPosition - transform.position).normalized;

            vectorParal = Vector3.Project(VelocityVector, normal);
            vectorPerp =  Vector3.ProjectOnPlane(VelocityVector, normal);

            collisionVectorParal = Vector3.Project(Player.VelocityVector, -normal);
            collisionVectorPerp = Vector3.ProjectOnPlane(Player.VelocityVector, -normal);

            //Bounce formula
            bounceVector = (vectorParal * (Mass - Player.Mass) + 2 * Player.Mass * collisionVectorParal) / (Mass + Player.Mass);
            VelocityVector = (bounceVector * (1 - KineticEnergyLoss)) + (vectorPerp * (1 - KineticEnergyLoss));

            bounceVector = (collisionVectorParal * (Player.Mass - Mass) + 2 * Mass * vectorParal) / (Player.Mass + Mass);
            Player.VelocityVector = (bounceVector * (1 - Player.KineticEnergyLoss)) + collisionVectorPerp * (1 - Player.SurfaceFriction);
            
            

            Debug.DrawRay(transform.position, VelocityVector, Color.blue, 0.2f);
            Debug.DrawRay(Player.transform.position, Player.VelocityVector, Color.cyan, 0.2f);

            Player.animator.SetTrigger("Stunned");

        }


        if (collision.transform.GetComponent<WallBounceController>()) {

            normal = -collision.transform.forward;         
            //normalAngle = Vector3.Angle(normal, VelocityVector) * Mathf.Deg2Rad;

            //Debug.Log("normalAngle: " + normalAngle * Mathf.Rad2Deg);

            //vectorParal = VelocityVector * Mathf.Cos(normalAngle);
            //vectorPerp = VelocityVector * Mathf.Sin(normalAngle);

            vectorParal = Vector3.Project(VelocityVector, normal);
            vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);

            //Debug.DrawRay(transform.position, vectorParal, Color.red, 5);
            //Debug.DrawRay(transform.position, vectorPerp, Color.cyan, 5);

            //Bounce formula

            //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
            bounceVector = (vectorParal * (Mass - collision.transform.GetComponent<WallBounceController>().Mass) - 2 * collision.transform.GetComponent<WallBounceController>().Mass * vectorParal) / (Mass + collision.transform.GetComponent<WallBounceController>().Mass);
            //Debug.Log("Bounce: " + bounceVector);
            VelocityVector = (bounceVector * (1 - KineticEnergyLoss)) + vectorPerp * (1 - SurfaceFriction);
            //Debug.Log("newVelocityVector: " + VelocityVector);
        }

    }
}
