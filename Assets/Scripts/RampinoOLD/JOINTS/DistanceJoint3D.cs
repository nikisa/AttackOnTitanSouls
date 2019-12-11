using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceJoint3D : MonoBehaviour
{
    public Rigidbody ConnectedRigidBody;
    public bool DetermineDistanceOnStart = true;
    public float Distance;
    public float Spring;
    public float Damper;

    protected Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        if (DetermineDistanceOnStart) {
            Distance = Vector3.Distance(rigidbody.position, ConnectedRigidBody.position);
        }
    }

    private void FixedUpdate() {
        var connection = rigidbody.position - ConnectedRigidBody.position;
        var distanceDiscrepancy = Distance - connection.magnitude;

        rigidbody.position += distanceDiscrepancy * connection.normalized;

        var velocityTarget = connection + (rigidbody.velocity + Physics.gravity * Spring);
        var projectOnConnection = Vector3.Project(velocityTarget, connection);
        rigidbody.velocity = (velocityTarget - projectOnConnection) / (1 + Damper * Time.fixedDeltaTime);

    }
}
