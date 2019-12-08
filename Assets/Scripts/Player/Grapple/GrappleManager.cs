using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    // Inspector

    public GrappleGun GrappleGun;
    public Hook hook;
    public HookPlaque hookPlaque;
    public PlayerController Player;
    

    public float MassPointNumber = 32;
    public float MassPointLength = 16;


    // Private
    private ArrayList RopeNodes; 


    public void InstantiateRope() {
        for (int i = 0; i < MassPointNumber; i++) {
            RopeNode node = (GameObject.Instantiate(Resources.Load("RopeNode") as GameObject)).GetComponent<RopeNode>();
            node.transform.position = GrappleGun.transform.position;
            RopeNodes.Add(node);
            node.transform.parent = this.transform;
        }

        hook.shooted = true;
    }

    public void UpdateHook() {
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float error = 0;
        float changeX = 0;
        float changeY = 0;

        RopeNode firstInstanceNode = RopeNodes[0] as RopeNode;
        hook.OldPos = hook.transform.position;
        hook.transform.position = hook.transform.position + hook.Inertia;
        hook.Inertia = hook.transform.position - hook.OldPos;
        pointsDistance = Vector3.Distance(firstInstanceNode.transform.position , hook.transform.position);

        if (true) { //Condizione --> Hook collides with HookPlaque

            hookPlaque.Hooked = true;
            hook.isHooked = true;
            hook.transform.position = hookPlaque.transform.position;
            hook.Inertia = Vector3.zero;
            // ?? hook move to top of layer ??
        }

        if (hook.ropeFinished && pointsDistance > MassPointLength) {
            error = pointsDistance - MassPointLength;
            changeX = error * Mathf.Cos(Vector3.Angle(firstInstanceNode.transform.position, hook.transform.position));
            changeY = error * Mathf.Sin(Vector3.Angle(firstInstanceNode.transform.position, hook.transform.position));

            hook.transform.position = new Vector3(hook.transform.position.x - (changeX * springValue), hook.transform.position.y, hook.transform.position.z - (changeY * springValue));
            hook.Inertia = (hook.transform.position - hook.OldPos) * (1 - DynamicFriction);

            firstInstanceNode.transform.position = new Vector3(firstInstanceNode.transform.position.x + changeX * (1-springValue) , firstInstanceNode.transform.position.y , firstInstanceNode.transform.position.z + changeY * (1-springValue));
            firstInstanceNode.Inertia = (firstInstanceNode.transform.position - firstInstanceNode.OldPos) * (1 - springValue);

            if (hook.isHooked) {
                // todo: pick hookPlaque where hookPlaque.isHooked && hookPlaque.life > 0
                    //Pull_HookPoint(HookPlaqueID);
            }

        }


    }

    public void UpdatePoints() {
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float error = 0;
        float changeX = 0;
        float changeY = 0;
        Vector3 previousInstance;
        

        RopeNode massPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
        massPoint.OldPos = massPoint.transform.position;
        massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
        massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
        pointsDistance = Vector3.Distance(GrappleGun.transform.position, massPoint.transform.position);

        if (pointsDistance > MassPointLength) {
            error = pointsDistance - MassPointLength;
            changeX = error * Mathf.Cos(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position));
            changeY = error * Mathf.Sin(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position));
            massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX, massPoint.transform.position.y, massPoint.transform.position.z - changeY);
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1-DynamicFriction);
        }

        previousInstance = massPoint.transform.position;
        for (int i = 1; i < RopeNodes.Count; i++) {
            massPoint = RopeNodes[i] as RopeNode;
            massPoint.OldPos = massPoint.transform.position;
            massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
            massPoint.Inertia = massPoint.transform.position - massPoint.OldPos;
            pointsDistance = Vector3.Distance(previousInstance, massPoint.transform.position);

            if (pointsDistance > MassPointLength) {
                error = pointsDistance - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX * springValue, massPoint.transform.position.y, massPoint.transform.position.z - changeY * springValue);
                massPoint.Inertia = massPoint.transform.position - massPoint.OldPos;
            }

            previousInstance = massPoint.transform.position;

            if (pointsDistance > MassPointLength) {
                massPoint = RopeNodes[(RopeNodes.Count - 1) - i + 1] as RopeNode;
                massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX * (1 - springValue), massPoint.transform.position.y, massPoint.transform.position.z + changeY * (1 - springValue));
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
            }
        }

    }

    public void UpdateLinks() {
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float SumOfLength = 0;
        float pointsDistance = 0;
        float error = 0;
        float changeX = 0;
        float changeY = 0;
        Vector3 previousInstance = Vector3.zero;

        RopeNode massPoint = RopeNodes[0] as RopeNode;
        pointsDistance = Vector3.Distance(hook.transform.position, massPoint.transform.position);

        if (pointsDistance > MassPointLength) {
            error = pointsDistance - MassPointLength;
            changeX = error * Mathf.Cos(Vector3.Angle(hook.transform.position, massPoint.transform.position));
            changeY = error * Mathf.Sin(Vector3.Angle(hook.transform.position, massPoint.transform.position));

            massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX, massPoint.transform.position.y, massPoint.transform.position.z - changeY);
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
            previousInstance = massPoint.transform.position;
            pointsDistance = Vector3.Distance(hook.transform.position , massPoint.transform.position);
            SumOfLength += pointsDistance;
        }

        for (int i = 1; i < RopeNodes.Count; i++) {
            massPoint = RopeNodes[i] as RopeNode;
            pointsDistance = Vector3.Distance(previousInstance, massPoint.transform.position);

            if (pointsDistance > MassPointLength) {
                error = pointsDistance - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX * springValue , massPoint.transform.position.y , massPoint.transform.position.z - changeY * springValue);
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1-DynamicFriction);
                previousInstance = massPoint.transform.position;
                SumOfLength += pointsDistance;
            }

            pointsDistance = Vector3.Distance(previousInstance, GrappleGun.transform.position);

            if (pointsDistance > MassPointLength) {
                error = pointsDistance - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance , GrappleGun.transform.position));
                changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, GrappleGun.transform.position));
                Player.transform.position = new Vector3(Player.transform.position.x - changeX * springValue, Player.transform.position.y, Player.transform.position.z - changeY * springValue);
                massPoint = RopeNodes[i - 1] as RopeNode;
                massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX * (1 - springValue), massPoint.transform.position.y, massPoint.transform.position.z + changeY * (1 - springValue));
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                pointsDistance = Vector3.Distance(previousInstance, GrappleGun.transform.position);
            }

            SumOfLength += pointsDistance;

            if (SumOfLength*(2-springValue) > RopeNodes.Count * MassPointLength) {
                hook.ropeFinished = true;
            }
            else {
                hook.ropeFinished = false;
            }
        }

    }

    public void RewindPoints() {
        int pointsRewinded = 0;
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float error = 0;
        float changeX = 0;
        float changeY = 0;
        float SumOfLength = 0;
        float pointsDisatnce = 0;
        Vector3 previousInstance = Vector3.zero;

        RopeNode massPoint;

        if (GrappleGun.RewindVector < GrappleGun.RewindTopSpeed) {
            GrappleGun.RewindVector += GrappleGun.RewindAcceleration; // *dt
        }
        else {
            GrappleGun.RewindVector = GrappleGun.RewindTopSpeed; // *dt
        }

        pointsRewinded = (int) Mathf.Ceil(GrappleGun.RewindVector / MassPointLength) - 1;

        if (pointsRewinded >= RopeNodes.Count) {
            hook.shooted = false;
            hook.ropeFinished = false;
            hook.isHooked = false;
            //Destroy(massPoint);
        }
        else {
            for (int i = 0; i < pointsRewinded-1; i++) {
                massPoint = RopeNodes[RopeNodes.Count - 1 - i] as RopeNode;
                massPoint.transform.position = new Vector3(GrappleGun.transform.position.x - MassPointLength /* * Mathf.Cos(GrappleGun.transform.rotation)*/ , GrappleGun.transform.position.y, GrappleGun.transform.position.z - MassPointLength /*Mathf.Sin(GrappleGun.transform.rotation)*/);                        
            }
            massPoint = RopeNodes[0] as RopeNode;
            massPoint.transform.position = massPoint.OldPos;
            massPoint.transform.position = new Vector3(massPoint.transform.position.x + massPoint.Inertia.x - GrappleGun.RewindVector * Mathf.Cos(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position)), massPoint.transform.position.y, massPoint.transform.position.z + massPoint.Inertia.z - GrappleGun.RewindVector * Mathf.Sin(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position)));
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1-DynamicFriction);
            pointsDisatnce = Vector3.Distance(GrappleGun.transform.position, massPoint.transform.position);

            if (pointsDisatnce > MassPointLength) {
                error = pointsDisatnce - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position));
                changeY = error * Mathf.Sin(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position));
            }
            previousInstance = massPoint.transform.position;
        }

        for (int i = pointsRewinded+1; i < RopeNodes.Count; i++) {
            massPoint = RopeNodes[i] as RopeNode;
            massPoint.OldPos = massPoint.transform.position;
            massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1-DynamicFriction);
            pointsDisatnce = Vector3.Distance(previousInstance , massPoint.transform.position);

            if (pointsDisatnce > MassPointLength) {
                error = pointsDisatnce - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                massPoint.transform.position = massPoint.transform.position * springValue;
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1-DynamicFriction);
            }

            previousInstance = massPoint.transform.position;

            if (pointsDisatnce > MassPointLength) {
                massPoint = RopeNodes[RopeNodes.Count - 1 - i + 1] as RopeNode;
                massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX * (1-springValue) , massPoint.transform.position.y , massPoint.transform.position.z + changeY * (1 - springValue));
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction); 
            }
        }
    }

    public void UpdateRewind() {
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float error = 0;
        float changeX = 0;
        float changeY = 0;
        float SumOfLength = 0;
        float pointsDisatnce = 0;
        float indexNumber;
        float pointsUnwinded = 0;

        Vector3 previousInstance = Vector3.zero;
        RopeNode massPoint;

        hook.shooted = false;
        hook.ropeFinished = false;
        hook.isHooked = false;

        massPoint = RopeNodes[0] as RopeNode;
        pointsDisatnce = Vector3.Distance(hook.transform.position, massPoint.transform.position);
        indexNumber = RopeNodes.Count;

        if (pointsDisatnce > MassPointLength) {
            error = pointsDisatnce - MassPointLength;
            changeX = error * Mathf.Cos(Vector3.Angle(hook.transform.position, massPoint.transform.position));
            changeY = error * Mathf.Sin(Vector3.Angle(hook.transform.position, massPoint.transform.position));

            massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX, massPoint.transform.position.y, massPoint.transform.position.z - changeY);
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);

            previousInstance = massPoint.transform.position;
            pointsDisatnce = Vector3.Distance(hook.transform.position, massPoint.transform.position);

        }

        SumOfLength += pointsDisatnce;
        previousInstance = massPoint.transform.position;

        for (int i = 1; i < indexNumber; i++) {
            massPoint = RopeNodes[i] as RopeNode;
            pointsDisatnce = Vector3.Distance(previousInstance, massPoint.transform.position);

            if (pointsDisatnce > MassPointLength) {
                error = pointsDisatnce - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
            }

            previousInstance = massPoint.transform.position;
            SumOfLength += pointsDisatnce;
        }
        pointsDisatnce = Vector3.Distance(previousInstance, GrappleGun.transform.position);

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
            if (pointsDisatnce > MassPointLength) {
                error = pointsDisatnce - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, GrappleGun.transform.position));
                changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, GrappleGun.transform.position));
                Player.transform.position = new Vector3(Player.transform.position.x - changeX * springValue, Player.transform.position.y, Player.transform.position.z - changeY * springValue);

                massPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
                massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX * (1 - springValue), massPoint.transform.position.y, massPoint.transform.position.z + changeY * (1 - springValue));
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                pointsDisatnce = Vector3.Distance(previousInstance, GrappleGun.transform.position);
                SumOfLength += pointsDisatnce;
            }
        }
        massPoint.gameObject.SetActive(false);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            pointsUnwinded = Mathf.Ceil(pointsDistance / MassPointLength) - 1;

            for (int i = 1; i <= pointsUnwinded; i++) {
                massPoint = RopeNodes[i] as RopeNode;

                pointsDistance = Vector3.Distance(previousInstance, massPoint.transform.position);

                if (pointsDistance > MassPointLength) {
                    error = pointsDistance - MassPointLength;
                    changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                    changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));

                    massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX * springValue, massPoint.transform.position.y, massPoint.transform.position.z - changeY * springValue);
                    massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                }
                previousInstance = massPoint.transform.position;
                SumOfLength += pointsDisatnce;
            }

            if (pointsDisatnce > MassPointLength) {
                error = pointsDistance - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                springValue = 1;
                Player.transform.position = new Vector3(Player.transform.position.x - changeX * springValue, Player.transform.position.y, Player.transform.position.z - changeY * springValue);
                massPoint.gameObject.SetActive(false);
            }

            if (SumOfLength * (2 - springValue) > RopeNodes.Count * MassPointLength) {
                hook.ropeFinished = true;
            }

        }
    }

    public void UpdateMassPoint() {

        float springVerlet = 1;
        float dynamicFriction = 25;
        float sumOfLength = 0;

        RaycastHit hit;
        Physics.Raycast(hook.transform.position, hook.transform.forward, out hit, Mathf.Infinity);
        Debug.DrawRay(hook.transform.position, hook.transform.forward, Color.blue);

        foreach (RopeNode massPoint in RopeNodes) {
            massPoint.OldPos = massPoint.transform.position;
            massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (100 - dynamicFriction) / 100;
        }

        if (hit.transform.gameObject.GetComponent<HookPlaque>()) {
            hook.Inertia = Vector3.zero;
            hook.transform.position = hit.transform.position;
            hook.isHooked = true;
        }

        if (!hook.isHooked) {
            hook.OldPos = hook.transform.position;
            hook.transform.position = hook.transform.position + hook.Inertia;
            hook.Inertia = hook.transform.position - hook.OldPos;
        }
    }

    public void HookFired() {
        UpdatePoints();
        UpdateLinks();
        UpdateMassPoint();
    }

    public void Rewind() {
        RewindPoints();
    }

    public void HookFailed() {
        UpdateMassPointReverse();
        UpdateLinksReverse();
    }

    void UpdateMassPointReverse() {
        float dynamicFriction = 60;
        foreach (RopeNode massPoint in RopeNodes) {
            massPoint.OldPos = massPoint.transform.position;
            massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
            massPoint.Inertia = massPoint.transform.position - massPoint.OldPos * (100 - dynamicFriction) / 100;
        }
    }

    void UpdateLinksReverse() {
        float springVerlet = 1;
        float dynamicFriction = 60;
        float massPointsDistance = 0;
        float error = 0;
        float changeX = 0;
        float changeY = 0;
        float provisional = 64; //distance between Player and GrappleGun
        Vector3 previousInstance;

        RopeNode massPoint = RopeNodes[(int) MassPointNumber - 1] as RopeNode;

        massPointsDistance = Vector3.Distance(GrappleGun.transform.position, massPoint.transform.position);

        if (massPointsDistance > 0) {
            error = massPointsDistance;
            changeX = error * Mathf.Cos(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position)+360);
            changeY = error * Mathf.Sin(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position)+360);

            massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX, massPoint.transform.position.y, massPoint.transform.position.z - changeY);
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (100 - dynamicFriction) / 100;
        }
        previousInstance = massPoint.transform.position;


        for (int i = 1; i < MassPointNumber; i++) {
            massPoint = RopeNodes[(int) MassPointNumber-1-i] as RopeNode;
            massPointsDistance = Vector3.Distance(previousInstance, massPoint.transform.position);
            if (massPointsDistance > MassPointLength) {
                error = massPointsDistance - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position) + 360);
                changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position) + 360);
                massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX * springVerlet , massPoint.transform.position.y , massPoint.transform.position.z - changeY * springVerlet);
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (100-dynamicFriction)/100;
            }
            previousInstance = massPoint.transform.position;

            if (massPointsDistance > MassPointLength) {
                massPoint = RopeNodes[(int) MassPointNumber-i] as RopeNode;
                massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX * (1 - springVerlet), massPoint.transform.position.y, massPoint.transform.position.z + changeY * (1 - springVerlet));
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (100 - dynamicFriction) / 100;
            }

            massPointsDistance = Vector3.Distance(previousInstance, hook.transform.position);

            if (massPointsDistance > MassPointLength) {
                error = massPointsDistance - MassPointLength;
                changeX = error * Mathf.Cos(Vector3.Angle(previousInstance, hook.transform.position) + 360);
                changeY = error * Mathf.Sin(Vector3.Angle(previousInstance, hook.transform.position) + 360);
                hook.transform.position = new Vector3(hook.transform.position.x - changeX * springVerlet , hook.transform.position.y , hook.transform.position.z - changeY * springVerlet);
                hook.Inertia = hook.transform.position - hook.OldPos * (100 - dynamicFriction) / 100;

                massPoint = RopeNodes[0] as RopeNode;
                massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX * springVerlet, massPoint.transform.position.y, massPoint.transform.position.z - changeY * springVerlet);
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (100 - dynamicFriction) / 100;
            }

        }

    }

    public void DisableMassPoints() {
        foreach (RopeNode massPoint in RopeNodes) {
            //if (masspoint.Id > fromIndex) {
            massPoint.gameObject.SetActive(false);
            //}
        }
    }

}
