using System.Collections.Generic;
using UnityEngine;

public class RopeCollisionGoldenLeg : MonoBehaviour {

    //Inspector
    public Transform Player;
    public Transform Hook;
    public float NodeDistance = .1f;
    public float MaxNodeDistance;
    public int TotalNodes = 100;
    public float RopeWidth = 0.1f;
    public int Iterations = 50;
    public float currentDistance;
    public float BounceConstraint;


    //Private 
    LineRenderer LineRenderer;
    Vector3[] LinePositions;
    
    private List<RopeNode> RopeNodes = new List<RopeNode>();
    private PlayerMovementState PlayerState;
    
    Camera Camera;
    
    int LayerMask = 9;
    ContactFilter2D ContactFilter;
    RaycastHit2D[] RaycastHitBuffer = new RaycastHit2D[10];
    Collider2D[] ColliderHitBuffer = new Collider2D[10];

    //new

    RaycastHit[] hits = new RaycastHit[10];
    Collider[] collisions = new Collider[10];

    //new


    Vector3 Gravity = new Vector3(0f, 0f , -1f);
    Vector3 Node1Lock;
    Vector3 LastNodeLock;

    void Awake() {
        Camera = Camera.main;
        PlayerState = Player.GetComponent<PlayerMovementState>();
        ContactFilter = new ContactFilter2D {
        layerMask = LayerMask,
        useTriggers = false,
        };

        LineRenderer = this.GetComponent<LineRenderer>();

        // Generate some rope nodes based on properties
        Vector3 startPosition = Vector2.zero;
        for (int i = 0; i < TotalNodes; i++) {
            RopeNode node = (GameObject.Instantiate(Resources.Load("RopeNode") as GameObject)).GetComponent<RopeNode>();
            node.transform.position = startPosition;
            node.PreviousPosition = startPosition;
            RopeNodes.Add(node);

            startPosition.y -= NodeDistance;
        }

        // for line renderer data
        LinePositions = new Vector3[TotalNodes];
    }


    void Update() {
        // Attach rope end to mouse click position

        Node1Lock = Player.position;
        //LastNodeLock = Hook.position;
        DrawRope();

        
    }

    private void FixedUpdate() {
        Simulate();

        // Higher iteration results in stiffer ropes and stable simulation
        for (int i = 0; i < Iterations; i++) {
            ApplyConstraint();

            // Playing around with adjusting collisions at intervals - still stable when iterations are skipped
            if (i % 2 == 1)
                AdjustCollisions();
        }
        
    }

    private void Simulate() {
        

        // step each node in rope
        for (int i = 0; i < TotalNodes; i++) {
            // derive the velocity from previous frame
            Vector3 velocity = RopeNodes[i].transform.position - RopeNodes[i].PreviousPosition;
            RopeNodes[i].PreviousPosition = RopeNodes[i].transform.position;

            // calculate new position
            Vector3 newPos = RopeNodes[i].transform.position + velocity;
            newPos += Gravity * Time.fixedDeltaTime;
            Vector3 direction = RopeNodes[i].transform.position - newPos;

            // cast ray towards this position to check for a collision
            //int result = -1;
            //result = Physics2D.CircleCast(RopeNodes[i].transform.position, RopeNodes[i].transform.localScale.x / 2f, -direction.normalized, ContactFilter, RaycastHitBuffer, direction.magnitude);

            float distance = Vector3.Distance(RopeNodes[i].transform.position , RopeNodes[i].PreviousPosition);

            //if (Physics.Raycast(newPos , direction , out hit , distance)) {
            //    RopeNodes[i].hasHit = true;
            //}

            //if (RopeNodes[i].hasHit) {
            //    newPos = (velocity - RopeNodes[i].transform.localScale);
            //}

            hits = Physics.SphereCastAll(RopeNodes[i].transform.position , RopeNodes[i].transform.localScale.x / 2f , -direction.normalized , NodeDistance , LayerMask , QueryTriggerInteraction.UseGlobal);
            if (hits.Length > 0) {
                for (int n = 0; n < hits.Length; n++) {
                    if (hits[n].collider.gameObject.layer == 9) {
                        Vector3 collidercenter = new Vector3(hits[n].collider.transform.position.x, RaycastHitBuffer[n].collider.transform.position.y);
                        Vector3 collisionDirection = hits[n].point - collidercenter;
                        // adjusts the position based on a circle collider
                        Vector3 hitPos = collidercenter + collisionDirection.normalized * (hits[n].collider.transform.localScale.x / 2f + RopeNodes[n].transform.localScale.x / 2f);
                        newPos = hitPos;
                        //break; //Just assuming a single collision to simplify the model
                    }
                }
            }


            //if (result > 0) {
            //    for (int n = 0; n < result; n++) {
            //        if (RaycastHitBuffer[n].collider.gameObject.layer == 9) {
            //            Vector2 collidercenter = new Vector2(RaycastHitBuffer[n].collider.transform.position.x, RaycastHitBuffer[n].collider.transform.position.y);
            //            Vector2 collisionDirection = RaycastHitBuffer[n].point - collidercenter;
            //            // adjusts the position based on a circle collider
            //            Vector2 hitPos = collidercenter + collisionDirection.normalized * (RaycastHitBuffer[n].collider.transform.localScale.x / 2f + RopeNodes[i].transform.localScale.x / 2f);
            //            newPos = hitPos;
            //            break; //Just assuming a single collision to simplify the model
            //        }
            //    }
            //}

            RopeNodes[i].transform.position = newPos;
        }
    }

    private void AdjustCollisions() {
        
        // Loop rope nodes and check if currently colliding
        for (int i = 0; i < TotalNodes - 1; i++) {
            RopeNode node = this.RopeNodes[i];


            int numberOfHits = Physics.OverlapSphereNonAlloc(node.transform.position , node.transform.localScale.x / 16, collisions);
            //int numberOfHits = Physics.SphereCastNonAlloc(node.transform.position , node.transform.localScale.x / 2 , (node.transform.position - node.PreviousPosition).normalized , collisions , NodeDistance , LayerMask , QueryTriggerInteraction.UseGlobal);
            if (numberOfHits > 0) {
                for (int n = 0; n < numberOfHits; n++) {
                    if (collisions[n].gameObject.layer != 8) {
                        Vector3 collidercenter = collisions[n].transform.position;
                        Vector3 collisionDirection = node.transform.position - collidercenter;

                        Vector3 hitPos = collidercenter + collisionDirection.normalized * ((collisions[n].transform.localScale.x / 2f) + (node.transform.localScale.x / 2f));
                        node.transform.position = hitPos;
                        //break;
                    }
                }
            }


            //int result = -1;
            //result = Physics2D.OverlapCircleNonAlloc(node.transform.position, node.transform.localScale.x / 64f, ColliderHitBuffer);

            //if (result > 0) {
            //    for (int n = 0; n < result; n++) {
            //        if (ColliderHitBuffer[n].gameObject.layer != 8) {
            //            // Adjust the rope node position to be outside collision
            //            Vector3 collidercenter = ColliderHitBuffer[n].transform.position;
            //            Vector3 collisionDirection = node.transform.position - collidercenter;

            //            Vector3 hitPos = collidercenter + collisionDirection.normalized * ((ColliderHitBuffer[n].transform.localScale.x / 2f) + (node.transform.localScale.x / 2f));
            //            node.transform.position = hitPos;
            //            break;
            //        }
            //    }
            //}
        }
    }

    private void ApplyConstraint() {
        if (Node1Lock != Vector3.zero) {
            RopeNodes[0].transform.position = Node1Lock;
        }
        else {
            RopeNodes[0].transform.position = Player.position;
        }

        if (LastNodeLock != Vector3.zero) {
            RopeNodes[RopeNodes.Count - 1].transform.position = LastNodeLock;
        }
        else {
            RopeNodes[RopeNodes.Count - 1].transform.position = Hook.position;
        }

        for (int i = 0; i < TotalNodes - 1; i++) {
            RopeNode node1 = this.RopeNodes[i];
            RopeNode node2 = this.RopeNodes[i + 1];

            // Get the current distance between rope nodes            
            currentDistance = (node1.transform.position - node2.transform.position).magnitude;
            float difference = Mathf.Abs(currentDistance - NodeDistance);
            Vector3 direction = Vector3.zero;

            // determine what direction we need to adjust our nodes
            if (currentDistance > NodeDistance) {
                direction = (node1.transform.position - node2.transform.position).normalized;
            }
            else if (currentDistance < NodeDistance) {
                direction = (node2.transform.position - node1.transform.position).normalized;
            }


            // calculate the movement vector
            Vector3 movement = direction * difference;

            // apply correction

            if (currentDistance <= MaxNodeDistance) {//TEST
                node1.transform.position -= (movement * 0.5f);
                node2.transform.position += (movement * 0.5f);
            }
            else {//TEST
                Player.position += node2.transform.position;
            }

        }
    }

    private void DrawRope() {
        LineRenderer.startWidth = RopeWidth;
        LineRenderer.endWidth = RopeWidth;

        for (int n = 0; n < TotalNodes; n++) {
            LinePositions[n] = new Vector3(RopeNodes[n].transform.position.x, RopeNodes[n].transform.position.y, RopeNodes[n].transform.position.z);
        }

        LineRenderer.positionCount = LinePositions.Length;
        LineRenderer.SetPositions(LinePositions);
    }

}


