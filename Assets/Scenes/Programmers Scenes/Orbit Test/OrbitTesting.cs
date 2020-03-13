using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitTesting : MonoBehaviour
{

    public float OrbitRay;
    public float angularMaxSpeed;
    public float angularAccelerationTime;
    public float angularDecelerationTime;
    public ChaseTestScript boss;
    public PlayerController Player;
    public float Mass;
    public float RotationAngle;


    //Public
    [HideInInspector]
    public float AngularAccelerationModule;
    [HideInInspector]
    public float AngularDecelerationModule;
    [HideInInspector]
    public Vector3 AngularAcceleration;
    [HideInInspector]
    public float AngularVelocity;
    [HideInInspector]
    public Vector3 VelocityVector;
    [HideInInspector]
    public float VelocityVectorModule;
    [HideInInspector]
    public float Drag;
    [HideInInspector]
    public float DecelerationModule;
    [HideInInspector]
    public Vector3 DecelerationVector;
    [HideInInspector]
    public Vector3 targetDir;
    [HideInInspector]
    Vector3 orbitOrientation;

    //Private
    bool pressed;

    //Bouncing values
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




    private void Start() {
        AngularDecelerationModule = angularMaxSpeed / angularDecelerationTime;
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!pressed) {
            MaskRotate();
        }
        else {
            MaskDeceleration();
        }

        
        if (Input.GetKeyDown(KeyCode.Space)) {
            pressed = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space)) {
            pressed = false;
        }

        

    }


    public void MaskRotate() {
        AngularAccelerationModule = angularMaxSpeed / angularAccelerationTime;
        Drag = AngularAccelerationModule / angularMaxSpeed * Time.deltaTime;
        AngularVelocity -= AngularVelocity * Drag;
        transform.eulerAngles += new Vector3(0, AngularVelocity * Time.deltaTime + 0.5f * AngularAccelerationModule * Mathf.Pow(Time.deltaTime, 2), 0);
        transform.position = new Vector3(boss.transform.position.x + OrbitRay * Mathf.Sin((transform.eulerAngles.y) * Mathf.Deg2Rad), transform.position.y, boss.transform.position.z + OrbitRay * Mathf.Cos((transform.eulerAngles.y) * Mathf.Deg2Rad));
        AngularVelocity += AngularAccelerationModule * Time.deltaTime;
        VelocityVector = new Vector3((AngularVelocity * Mathf.PI / 180) * OrbitRay * Mathf.Sin((transform.eulerAngles.y + 90) * Mathf.Deg2Rad), transform.position.y, (AngularVelocity * Mathf.PI / 180) * OrbitRay * Mathf.Cos((transform.eulerAngles.y + 90) * Mathf.Deg2Rad));
        //Debug.DrawRay(transform.position, VelocityVector + boss.VelocityVector , Color.blue, .016f);
    }

    public void MaskDeceleration() {

        if (Mathf.Abs(AngularVelocity) > Mathf.Abs(AngularDecelerationModule) * Time.deltaTime) {
            AngularVelocity -= AngularDecelerationModule * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, AngularVelocity * Time.deltaTime, 0);
            transform.position = new Vector3(boss.transform.position.x + OrbitRay * Mathf.Sin((transform.eulerAngles.y) * Mathf.Deg2Rad), transform.position.y, boss.transform.position.z + OrbitRay * Mathf.Cos((transform.eulerAngles.y) * Mathf.Deg2Rad));
            VelocityVector = new Vector3((AngularVelocity * Mathf.PI / 180) * OrbitRay * Mathf.Sin(transform.eulerAngles.x), transform.position.y, (AngularVelocity * Mathf.PI / 180) * OrbitRay * Mathf.Cos(transform.eulerAngles.z));
        }
        else {
            AngularVelocity = 0;
            VelocityVector = Vector3.zero;
        }
    }


    /// <summary>
    /// 
    /// Somma di 3 vettori:
    /// 
    /// VelocityVector dell'oggetto orbitante
    /// +
    /// VelocityVector della Forza Centripeta --> ((VelocityVector dell'oggetto orbitante)^2 / (OrbitRay)) * dt
    /// +
    /// VelocityVector del boss
    /// =
    /// TotalVector
    /// _________________________________________________________________________________________________________
    /// Scomposizione del TotalVector in:
    /// VectorParal e VectorPerp
    /// 
    /// FrictionImpulse = VectorParal * (2 * SurfaceFriction) 
    /// FrictionImpulse = Quaternion.LookAt(VectorPerp , Vector3.Up)0
    /// BounceVector = (vectorParal * (Mass - Player.Mass) + 2 * Player.Mass * collisionVectorParal) / (Mass + Player.Mass);
    /// BossVelocityVector = BounceVector * (1- KineticEnergyLoss) + (VectorPerp - FrictionImpulse) 
    /// 
    /// AngularVelocity = Mathf.Sign(VectorPerp - FrictionImpulse) * AngularVelocity * (1 - SurfaceFriction)
    /// 
    /// 
    /// </summary>
    /// <param name="collision"></param>
    /// 

    #region NewBounce 

    private void OnCollisionEnter(Collision collision) {

        if (collision.transform.GetComponent<WallBounceController>()) {

            float plusAngle;

            normal = -collision.transform.forward;

            vectorParal = Vector3.Project(VelocityVector, normal);
            vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);

            //Debug.DrawRay(transform.position, vectorParal, Color.red, 5);
            //Debug.DrawRay(transform.position, vectorPerp, Color.cyan, 5);

            //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
            bounceVector = (-2 * Mass * (vectorParal) / (2 * Mass));

            plusAngle = AngularVelocity * Time.deltaTime * vectorPerp.magnitude / VelocityVector.magnitude;

            boss.transform.RotateAround(transform.position, Vector3.up, plusAngle);

            Vector3 bossVectorParal = Vector3.Project(boss.VelocityVector, normal);
            Vector3 bossVectorPerp = Vector3.ProjectOnPlane(boss.VelocityVector, normal);


            Vector3 newBounceVector = (-2 * Mass * (bossVectorParal) / (2 * Mass));

            boss.VelocityVector = (newBounceVector + bossVectorPerp) * (1 - KineticEnergyLoss);
            boss.AccelerationVector = boss.VelocityVector.normalized * boss.AccelerationVector.magnitude;
            boss.VelocityVector += bounceVector * (1 - KineticEnergyLoss) * Mass/boss.Mass;

            AngularVelocity *= -(1 - SurfaceFriction); //angularMaxSpeed * (1 - SurfaceFriction) * vectorPerp.magnitude / VelocityVector.magnitude; 

            Debug.DrawRay(boss.transform.position, vectorParal, Color.blue, .016f);
            Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, .016f);
            Debug.Log("VectorPerp: " + vectorPerp);

        }

    }

    #endregion


    #region NewBounce NotWorking
    //private void OnCollisionEnter(Collision collision) {

    //    if (collision.transform.GetComponent<WallBounceController>()) {

    //        VelocityVectorModule = VelocityVector.magnitude;
    //        float centripetalVectorModule = (Mathf.Pow(VelocityVectorModule, 2) / OrbitRay) * Time.deltaTime;
    //        Vector3 centripetalVector = (boss.transform.position - transform.position).normalized * centripetalVectorModule;

    //        normal = -collision.transform.forward;

    //        VelocityVector += boss.VelocityVector + centripetalVector;

    //        vectorParal = Vector3.Project(VelocityVector, normal);
    //        vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);

    //        //Debug.DrawRay(transform.position, vectorParal, Color.red, 5);
    //        //Debug.DrawRay(transform.position, vectorPerp, Color.cyan, 5);

    //        //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
    //        bounceVector = (-2 * Mass * vectorParal) / (2 * Mass);
    //        //bounceVector *= 1 - KineticEnergyLoss;

    //        Vector3 frictionImpulse = vectorParal * (2 * SurfaceFriction);
    //        frictionImpulse = -(vectorPerp).normalized * frictionImpulse.magnitude;
    //        Debug.DrawRay(transform.position, frictionImpulse, Color.cyan, .016f);
    //        float sign = Mathf.Sign((vectorPerp.magnitude - frictionImpulse.magnitude));

    //        vectorPerp -= frictionImpulse;
    //        Debug.DrawRay(transform.position, vectorPerp, Color.blue, .016f);

    //        if (sign < 0) {
    //            boss.VelocityVector = (bounceVector * (1 - KineticEnergyLoss)) + vectorPerp;
    //        }
    //        else {
    //            boss.VelocityVector = (bounceVector * (1 - KineticEnergyLoss));
    //        }

    //        AngularVelocity *= sign * (1 - SurfaceFriction);

    //        Debug.DrawRay(boss.transform.position, vectorParal, Color.blue, .016f);

    //        Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, .016f);
    //        Debug.Log("VectorPerp: " + vectorPerp);

    //    }

    //}

    #endregion


    #region OldBunce
    //private void OnCollisionEnter(Collision collision) {


    //    if (collision.transform.GetComponent<PlayerController>()) {

    //        Player = collision.transform.GetComponent<PlayerController>();

    //        Vector3 fakePlayerPosition = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
    //        normal = (fakePlayerPosition - transform.position).normalized;

    //        vectorParal = Vector3.Project(VelocityVector, normal);
    //        vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);


    //        collisionVectorParal = Vector3.Project(Player.VelocityVector, -normal);
    //        collisionVectorPerp = Vector3.ProjectOnPlane(Player.VelocityVector, -normal);

    //        //Bounce formula
    //        bounceVector = (vectorParal * (Mass - Player.Mass) + 2 * Player.Mass * collisionVectorParal) / (Mass + Player.Mass);
    //        bounceVector *= 1 - KineticEnergyLoss;
    //        //Debug.DrawRay(transform.position, bounceVector, Color.blue, 0.2f);

    //        bounceVector += vectorPerp * (1-KineticEnergyLoss);


    //        //boss.VelocityVector = vectorPerp * (1 - SurfaceFriction);
    //        //Debug.DrawRay(transform.position, vectorPerp, Color.green, 0.2f);

    //        //PLAYER__________

    //        normal = (transform.position - fakePlayerPosition).normalized;

    //        //Bounce formula
    //        bounceVector = (collisionVectorParal * (Player.Mass - Mass) + 2 * Mass * vectorParal) / (Player.Mass + Mass);
    //        Player.VelocityVector = (bounceVector * (1 - Player.KineticEnergyLoss)) + collisionVectorPerp * (1 - Player.SurfaceFriction);
    //        //Debug.DrawRay(transform.position, VelocityVector, Color.blue, 0.2f);

    //        Player.animator.SetTrigger("Stunned");

    //        //__________________________________


    //        Vector3 fakeSpherePosition = new Vector3(transform.position.x, boss.transform.position.y, transform.position.z);
    //        normal = fakeSpherePosition - boss.transform.position;



    //        vectorParal = Vector3.Project(bounceVector, normal);
    //        vectorPerp = Vector3.ProjectOnPlane(bounceVector, normal);

    //        Debug.Log("Result: " + (vectorPerp.normalized - transform.localPosition));

    //        //Debug.DrawRay(transform.position, vectorParal, Color.red, 0.6f);
    //        //Debug.DrawRay(transform.position, vectorPerp, Color.cyan, 0.6f);


    //        boss.VelocityVector += vectorParal;

    //        AngularVelocity = ((vectorPerp.magnitude * Mathf.Rad2Deg) / OrbitRay) * Mathf.Sign(-AngularVelocity /*(Mass * VelocityVector.sqrMagnitude) - (Player.mass * VelocityVector.sqrMagnitude)*/);

    //        Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, 0.2f);
    //        Debug.DrawRay(Player.transform.position, Player.VelocityVector, Color.green, 0.2f);
    //        Debug.DrawRay(transform.position, VelocityVector, Color.blue, 0.2f);


    //    }


    //    if (collision.transform.GetComponent<WallBounceController>()) {

    //        normal = -collision.transform.forward;
    //        VelocityVector += boss.VelocityVector;

    //        vectorParal = Vector3.Project(VelocityVector, normal);
    //        //vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);

    //        //Debug.DrawRay(transform.position, vectorParal, Color.red, 5);
    //        //Debug.DrawRay(transform.position, vectorPerp, Color.cyan, 5);

    //        //Bounce formula

    //        //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
    //        bounceVector = (-2 * Mass * vectorParal) / (2 * Mass);
    //        bounceVector *= 1 - KineticEnergyLoss;

    //        Vector3 fakeSpherePosition = new Vector3(transform.position.x, boss.transform.position.y, transform.position.z);

    //        normal = fakeSpherePosition - boss.transform.position;

    //        vectorParal = Vector3.Project(bounceVector, normal);
    //        vectorPerp = Vector3.ProjectOnPlane(bounceVector, normal);

    //        //Se c'è possibilità di stun allora:
    //        boss.VelocityVector = vectorParal;

    //        //altrimenti:
    //        //boss.VelocityVector += vectorParal;

    //        AngularVelocity = ((vectorPerp.magnitude * Mathf.Rad2Deg) / OrbitRay) * Mathf.Sign(-AngularVelocity);


    //        Debug.DrawRay(boss.transform.position, vectorParal, Color.blue, .016f);
    //        Debug.DrawRay(transform.position, vectorPerp , Color.blue, .016f);
    //        Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, .016f);
    //        Debug.Log("VectorPerp: " + vectorPerp);

    //    }

    //}
    #endregion

}
