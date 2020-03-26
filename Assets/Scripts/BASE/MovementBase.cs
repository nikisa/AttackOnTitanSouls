using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementBase : MonoBehaviour
{

    //Inspector
    public float MinBounceVector;
    public float MaxBounceVector;
    public CharacterController CharacterController;
    public float Mass;
    [HideInInspector]
    [Range(0, 1)]
    public float KineticEnergyLoss;
    [HideInInspector]
    [Range(0, 1)]
    public float SurfaceFriction;

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
    public Vector3 BounceVector;
    [HideInInspector]
    public float Drag;
    [HideInInspector]
    public float DecelerationModule;
    [HideInInspector]
    public float BounceDecelerationModule;
    [HideInInspector]
    public Vector3 DecelerationVector;
    [HideInInspector]
    public Vector3 BounceDecelerationVector;
    [HideInInspector]
    public Vector3 targetDir;
    [HideInInspector]
    public float gravity;
    [HideInInspector]
    public float impulseDeltaTime;
    #endregion


    private void Awake() {
        gravity = 0f;
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
        //Drag = _accelerationModule / _maxSpeed * Time.deltaTime;
        //VelocityVector -= VelocityVector * Drag;
        move = Vector3.ClampMagnitude((VelocityVector * Time.deltaTime + 0.5f * AccelerationVector * Mathf.Pow(Time.deltaTime, 2)) , _maxSpeed * Time.deltaTime); //Formula completa per un buon effetto fin dal primo frame
        nextPosition = transform.position + move;
        VelocityVector = Vector3.ClampMagnitude((VelocityVector + AccelerationVector * Time.deltaTime), _maxSpeed);
        CharacterController.Move(move + Vector3.down * gravity);

        //Debug.DrawRay(transform.position, VelocityVector, Color.blue, 0.2f);
        //Debug.DrawRay(transform.position, AccelerationVector, Color.red, 0.2f);

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

    public void BounceDeceleration() {

        float vectorAngle = Vector3.SignedAngle(Vector3.forward, BounceVector.normalized, Vector3.up) * Mathf.Deg2Rad;
        BounceDecelerationVector = new Vector3(Mathf.Sin(vectorAngle) * BounceDecelerationModule, 0, Mathf.Cos(vectorAngle) * BounceDecelerationModule);

        BounceVector -= BounceDecelerationVector * Time.deltaTime;
        move = BounceVector * Time.deltaTime;
        CharacterController.Move(move + Vector3.down * gravity);

        Debug.DrawRay(transform.position, BounceVector, Color.black, .03f);
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

    public void BounceMovement(Collider hit) {

        #region Bounce variables
        MovementBase collidingObject = hit.gameObject.GetComponent<MovementBase>();
        Vector3 normal;
        Vector3 vectorParal;
        Vector3 vectorPerp;
        Vector3 collisionVectorParal;
        Vector3 collisionVectorPerp;
        Vector3 bounceVector;
        #endregion


        Vector3 fakeCollidingObjectPosition = new Vector3(collidingObject.transform.position.x, transform.position.y, collidingObject.transform.position.z);
        normal = (fakeCollidingObjectPosition - transform.position).normalized;
        
        vectorParal = Vector3.Project(VelocityVector, normal);
        vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);
        
        collisionVectorParal = Vector3.Project(collidingObject.VelocityVector, -normal);
        collisionVectorPerp = Vector3.ProjectOnPlane(collidingObject.VelocityVector, -normal);
        
        //Bounce formula
        bounceVector = (vectorParal * (Mass - collidingObject.Mass) + 2 * collidingObject.Mass * collisionVectorParal) / (Mass + collidingObject.Mass);
        VelocityVector = (bounceVector * (1 - KineticEnergyLoss)) + (vectorPerp * (1 - KineticEnergyLoss));
        //VelocityVector = Mathf.Clamp(VelocityVector.magnitude, MinBounceVector, MaxBounceVector) * VelocityVector.normalized;
        AccelerationVector = VelocityVector.normalized * AccelerationVector.magnitude;
        
        bounceVector = (collisionVectorParal * (collidingObject.Mass - Mass) + 2 * Mass * vectorParal) / (collidingObject.Mass + Mass);
        collidingObject.VelocityVector = (bounceVector * (1 - collidingObject.KineticEnergyLoss)) + collisionVectorPerp * (1 - collidingObject.SurfaceFriction);
        
        Debug.DrawRay(transform.position, VelocityVector, Color.blue, 0.2f);
        Debug.DrawRay(collidingObject.transform.position, collidingObject.VelocityVector, Color.black, 0.2f);

    }

    public void WallBounce(ControllerColliderHit hit , float _angularVelocity) {

        #region Bounce variables
        GameObject collidingObject = hit.collider.gameObject;

        Vector3 normal;
        Vector3 vectorParal;
        Vector3 vectorPerp;
        Vector3 bounceVector;
        float plusAngle;
        #endregion

        normal = -collidingObject.transform.forward;

        vectorParal = Vector3.Project(VelocityVector, normal);
        vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);

        //Debug.DrawRay(transform.position, vectorParal, Color.red, 5);
        //Debug.DrawRay(transform.position, vectorPerp, Color.cyan, 5);

        //Bounce formula
        //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
        bounceVector = (-2 * Mass * vectorParal) / (2 * Mass);
        bounceVector *= 1 - KineticEnergyLoss;
        VelocityVector = (bounceVector * (1 - KineticEnergyLoss)) + vectorPerp * (1 - SurfaceFriction);

        Debug.DrawRay(transform.position, VelocityVector, Color.cyan, .2f);
        plusAngle = -(_angularVelocity * impulseDeltaTime);



        VelocityVector = Quaternion.AngleAxis(plusAngle, Vector3.up) * VelocityVector;
        AccelerationVector = VelocityVector.normalized * AccelerationVector.magnitude;



        Debug.DrawRay(transform.position, VelocityVector, Color.blue, .02f);
        Debug.DrawRay(transform.position, AccelerationVector, Color.red, .02f);
        
    }

}
