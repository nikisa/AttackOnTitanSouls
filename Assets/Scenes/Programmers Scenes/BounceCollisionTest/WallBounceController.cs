using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounceController : MonoBehaviour
{


    //Temporary public values
    public ChaseTestScript fakeBoss;
    public float Mass;
    public Vector3 normal;
    public float normalAngle;
    public Vector3 vectorParal;
    public Vector3 vectorPerp;
    public Vector3 bounceVector;
    [Range(0, 1)]
    public float KineticEnergyLoss;

    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.transform.GetComponent<ChaseTestScript>()) {
    //        normal = -transform.forward;
    //        normalAngle = Vector3.Angle(normal, fakeBoss.VelocityVector) * Mathf.Deg2Rad;

    //        vectorParal = fakeBoss.VelocityVector * Mathf.Cos(normalAngle);
    //        vectorPerp = fakeBoss.VelocityVector * Mathf.Sin(normalAngle);

    //        //Bounce formula
    //        bounceVector = (vectorParal * (mass - fakeBoss.Mass) + 2 * fakeBoss.Mass * fakeBoss.vectorParal) / (mass + fakeBoss.Mass);
    //        VelocityVector = (bounceVector * (1 - KineticEnergyLoss)) + vectorPerp;
    //    }
    //}


}
