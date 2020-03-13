using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public bool shooted;
    public bool isHooked;
    public bool ropeFinished;
    public float VectorAngle;
    public float topSpeed;
    public float DynamicFriction;
    public float hitDistance;
    public int hookPointID;
    public Vector3 OldPos;
    public Vector3 Inertia;

    //Private
    RaycastHit hit;

    public void Start()
    {
        isHooked = true;
    }
    public RaycastHit RaycastCollsion() {

        Physics.Raycast(transform.position,transform.up, out hit, hitDistance); //hook.transform up sarà forward con il modello nuovo (non girato)
        Debug.DrawRay(transform.position, transform.up, Color.blue);

        return hit;
    }
}
