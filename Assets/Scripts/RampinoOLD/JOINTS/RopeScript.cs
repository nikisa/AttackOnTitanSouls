using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector3 target;
    public float speed = 1;

    public float distance = 2;

    public GameObject nodePrefab;
    public GameObject Player;
    public GameObject lastNode;

    public LineRenderer lr;

    public List<GameObject> Nodes = new List<GameObject>();

    int vertexCount = 2;
    bool finished = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        lr = GetComponent<LineRenderer>();
        lastNode = transform.gameObject;
        Nodes.Add(transform.gameObject);


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed);

        if (transform.position != target) {
            if (Vector3.Distance(Player.transform.position , lastNode.transform.position) > distance) {
                MakeNode();
            }
        }
        else if (!finished) {
            finished = true;

            while (Vector2.Distance(Player.transform.position , lastNode.transform.position) > distance) {
                MakeNode();
            }

            lastNode.GetComponent<HingeJoint>().connectedBody = Player.GetComponent<Rigidbody>();

        }

        RenderLine();
    }

    void RenderLine() {
        lr.SetVertexCount(vertexCount);
        int i;
        for (i = 0; i < Nodes.Count; i++) {

            lr.SetPosition(i, Nodes[i].transform.position);

        }

        lr.SetPosition(i , Player.transform.position);

    }

    void MakeNode() {

        Vector3 posCreation = Player.transform.position - lastNode.transform.position;
        posCreation.Normalize();
        posCreation *= distance;
        posCreation += (Vector3) lastNode.transform.position;

        GameObject piece = Instantiate(nodePrefab, posCreation, Quaternion.identity);

        piece.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint>().connectedBody = piece.GetComponent<Rigidbody>();
        lastNode = piece;

        Nodes.Add(lastNode);
        vertexCount++;
    }
}
