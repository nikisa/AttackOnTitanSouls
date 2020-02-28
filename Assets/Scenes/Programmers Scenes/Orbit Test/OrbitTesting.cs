using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitTesting : MonoBehaviour
{

    public float OrbitRay;
    public float angularMaxSpeed;
    public float angularAccelerationTime;
    public float angularDecelerationTime;
    public GameObject cube;


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
        transform.position = new Vector3(cube.transform.position.x + OrbitRay * Mathf.Sin((transform.eulerAngles.y) * Mathf.Deg2Rad), 0, cube.transform.position.z + OrbitRay * Mathf.Cos((transform.eulerAngles.y) * Mathf.Deg2Rad));
        AngularVelocity += AngularAccelerationModule * Time.deltaTime;
        VelocityVector = new Vector3((AngularVelocity * Mathf.PI - 180) * OrbitRay * Mathf.Sin(transform.eulerAngles.x), 0, (AngularVelocity * Mathf.PI - 180) * OrbitRay * Mathf.Cos(transform.eulerAngles.z));
    }

    public void MaskDeceleration() {

        if (Mathf.Abs(AngularVelocity) > Mathf.Abs(AngularDecelerationModule) * Time.deltaTime) {
            AngularVelocity -= AngularDecelerationModule * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, AngularVelocity * Time.deltaTime, 0);
            transform.position = new Vector3(cube.transform.position.x + OrbitRay * Mathf.Sin((transform.eulerAngles.y) * Mathf.Deg2Rad), 0, cube.transform.position.z + OrbitRay * Mathf.Cos((transform.eulerAngles.y) * Mathf.Deg2Rad));
            VelocityVector = new Vector3((AngularVelocity * Mathf.PI - 180) * OrbitRay * Mathf.Sin(transform.eulerAngles.x), 0, (AngularVelocity * Mathf.PI - 180) * OrbitRay * Mathf.Cos(transform.eulerAngles.z));
        }
        else {
            AngularVelocity = 0;
            VelocityVector = Vector3.zero;
        }
        

    }
}
