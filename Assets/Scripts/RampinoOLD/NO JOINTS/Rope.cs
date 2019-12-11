using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    // Inspector
    [Tooltip("Distanza tra i nodi che compongono la corda")]
    public float ropeSegLen = 0.25f;
    [Tooltip("Lunghezza massima della corda")]
    public int segmentLength = 35;
    [Tooltip("Spessore della corda")]
    public float lineWidth = 0.1f;
    [Tooltip("Realismo simulazione")]
    [Range(2,50)]
    public int PhysicsSimulation = 25;

    public GameObject Hook;

    public GameObject Player;

    // Private
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();

    private void Start() {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < segmentLength; i++) {

            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;

        }
    }

    private void Update() {
        this.DrawRope();
    }

    private void FixedUpdate() {
        this.Simulate();
    }

    private void Simulate() {

        // Simulation
        Vector3 forceGravity = new Vector3(0, -9.81f, 0);

        for (int i = 0; i < this.segmentLength; i++) {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector3 velocity = firstSegment.posNow - firstSegment.posOld;

            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.deltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        // Constrains
        for (int i = 0; i < PhysicsSimulation; i++) {
            this.ApplyConstrains();
        }
    }

    private void ApplyConstrains() {
        // La pos del primo segmento corrisponde alla posizione del mouse
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = Player.transform.position;

        this.ropeSegments[0] = firstSegment;

        for (int i = 0; i < this.segmentLength - 1; i++) {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = dist - ropeSegLen;
            Vector3 changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;

            if (dist > ropeSegLen) {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen) {
                changeDir = (secondSeg.posNow - firstSegment.posNow).normalized;
            }

            Vector3 changeAmount = changeDir * error;
            if (i != 0 && i < this.segmentLength - 2) {
                firstSeg.posNow -= changeAmount * .01f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * .01f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }

            if (i == this.segmentLength - 2) {
                firstSeg.posNow -= changeAmount * .01f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow = Hook.transform.position;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope() {

        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];

        for (int i = 0; i < this.segmentLength; i++) {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);

    }


    public struct RopeSegment {
        public Vector3 posNow;
        public Vector3 posOld;

        public RopeSegment(Vector3 pos) {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

}
