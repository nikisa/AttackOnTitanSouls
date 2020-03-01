using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBase : MonoBehaviour
{

    //Inspector
    public CharacterController CharacterController;

    //Public
    #region MOVEMENT
    [HideInInspector]
    public Vector3 move;
    [HideInInspector]
    public Vector3 nextPosition;
    [HideInInspector]
    public float AccelerationModule;
    [HideInInspector]
    public Vector3 AccelerationVector;
    [HideInInspector]
    public Vector3 VelocityVector;
    [HideInInspector]
    public float Drag;
    [HideInInspector]
    public float DecelerationModule;
    [HideInInspector]
    public Vector3 DecelerationVector;
    [HideInInspector]
    public Vector3 targetDir;
    [HideInInspector]
    public float gravity;
    #endregion


    private void Awake() {
        gravity = 9.81f;
    }


    /// <summary>
    /// Movement with custom physics 
    /// </summary>
    /// <param name="_targetDir"></param>
    /// <param name="_maxSpeed"></param>
    /// <param name="_accelerationModule">accelerationModule = MaxSpeed / TimeAcceleration; </param>
    public void Movement(Vector3 _targetDir , float _maxSpeed , float _accelerationModule) {

        Vector3 accelerationVectorTemp = _targetDir;
        accelerationVectorTemp.y = 0;
        AccelerationVector = accelerationVectorTemp.normalized * _accelerationModule;
        Drag = _accelerationModule / _maxSpeed * Time.deltaTime;
        VelocityVector -= VelocityVector * Drag;
        move = VelocityVector * Time.deltaTime + 0.5f * AccelerationVector * Mathf.Pow(Time.deltaTime, 2); //Formula completa per un buon effetto fin dal primo frame
        nextPosition = transform.position + move;
        VelocityVector += AccelerationVector * Time.deltaTime;        
        CharacterController.Move(move + Vector3.down * gravity);

    }

    public void Deceleration() {
        //Vector3 decelerationVectorTemp = targetDir;
        //DecelerationVector = decelerationVectorTemp.normalized * DecelerationModule;
        //VelocityVector -= DecelerationVector * Time.deltaTime;
        //move = VelocityVector * Time.deltaTime;
        //CharacterController.Move(move + Vector3.down * gravity);

        float vectorAngle = Vector3.SignedAngle(Vector3.forward, VelocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
        DecelerationVector = new Vector3(Mathf.Sin(vectorAngle) * DecelerationModule, 0, Mathf.Cos(vectorAngle) * DecelerationModule);

        VelocityVector -= DecelerationVector * Time.deltaTime;
        move = VelocityVector * Time.deltaTime;
        CharacterController.Move(move + Vector3.down * gravity);
    }

    public void MovementReset() {

        move = Vector3.zero;
        nextPosition = Vector3.zero;
        AccelerationVector = Vector3.zero;
        VelocityVector = Vector3.zero;
        DecelerationVector = Vector3.zero;
        Drag = 0;
        AccelerationModule = 0;
        DecelerationModule = 0;

    }

}
