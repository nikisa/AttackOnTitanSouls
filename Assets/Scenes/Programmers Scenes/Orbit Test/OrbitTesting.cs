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


    private void OnCollisionEnter(Collision collision) {


        if (collision.transform.GetComponent<PlayerController>()) {

            Debug.Log("COLLISION");

            Vector3 fakePlayerPosition = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
            normal = (fakePlayerPosition - transform.position).normalized;

            vectorParal = Vector3.Project(VelocityVector, normal);
            vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);

            //Bounce formula
            bounceVector = (vectorParal * (Mass - Player.mass) + 2 * Player.mass * Player.vectorParal) / (Mass + Player.mass);
            bounceVector *= 1 - KineticEnergyLoss;

            boss.VelocityVector = vectorPerp * (1 - SurfaceFriction);


            Vector3 fakeSpherePosition = new Vector3(transform.position.x, boss.transform.position.y, transform.position.z);
            normal = fakeSpherePosition - boss.transform.position;

            vectorParal = Vector3.Project(bounceVector, normal);
            vectorPerp = Vector3.ProjectOnPlane(bounceVector, normal);

            boss.VelocityVector += vectorParal;

            AngularVelocity = ((vectorPerp.magnitude * Mathf.Rad2Deg) / OrbitRay) * Mathf.Sign(-AngularVelocity);

        }


        if (collision.transform.GetComponent<WallBounceController>()) {

            normal = -collision.transform.forward;
            VelocityVector += boss.VelocityVector;

            vectorParal = Vector3.Project(VelocityVector, normal);
            //vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);

            //Debug.DrawRay(transform.position, vectorParal, Color.red, 5);
            //Debug.DrawRay(transform.position, vectorPerp, Color.cyan, 5);

            //Bounce formula

            //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
            bounceVector = (-2 * Mass * vectorParal) / (2 * Mass);
            bounceVector *= 1 - KineticEnergyLoss;

            Vector3 fakeSpherePosition = new Vector3(transform.position.x, boss.transform.position.y, transform.position.z);

            normal = fakeSpherePosition - boss.transform.position;

            vectorParal = Vector3.Project(bounceVector, normal);
            vectorPerp = Vector3.ProjectOnPlane(bounceVector, normal);

            //Se c'è possibilità di stun allora:
            boss.VelocityVector = vectorParal;

            //altrimenti:
            //boss.VelocityVector += vectorParal;

            AngularVelocity = ((vectorPerp.magnitude * Mathf.Rad2Deg) / OrbitRay) * Mathf.Sign(-AngularVelocity);


            Debug.DrawRay(boss.transform.position, vectorParal, Color.blue, .016f);
            Debug.DrawRay(transform.position, vectorPerp , Color.blue, .016f);
            Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, .016f);
            Debug.Log("VectorPerp: " + vectorPerp);

        }

    }

}
