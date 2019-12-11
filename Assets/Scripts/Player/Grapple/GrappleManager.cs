﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    // Inspector
    public bool debugMode;
    public GrappleGun GrappleGun;
    public Hook hook;
    [HideInInspector]
    public HookPoint hookPoint;
    public PlayerController Player;
    public GameObject CrossHair;

    public int MassPointNumber = 32;
    public float MassPointLength = 0.25f;

    private void LateUpdate() {

        if (debugMode) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                HookShooting();
                if (!hook.shooted)
                    InstantiateRope();
            }
            if (hook.shooted) {
                if (Input.GetKeyDown(KeyCode.Z))
                    UpdatePoints();
                if (Input.GetKeyDown(KeyCode.X))
                    UpdateHook();
                if (Input.GetKeyDown(KeyCode.C))
                    UpdateLinks();
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                HookShooting();
                if (!hook.shooted)
                    InstantiateRope();
            }
            if (hook.shooted) {

                if (!Input.GetKeyDown(KeyCode.Mouse1)) {
                    UpdatePoints();
                    UpdateLinks();
                }
                if (Input.GetKey(KeyCode.Mouse1) && !Input.GetKeyUp(KeyCode.Mouse1)) {
                    RewindPoints();
                }
                
            }

            if (hook.shooted && !hook.isHooked) {
                hit = hook.RaycastCollsion();

                if (hit.transform != null && hit.transform.GetComponent<HookPoint>()) {
                    Debug.Log(hit.transform.gameObject.name + "---");
                    hookPoint = hit.transform.GetComponent<HookPoint>();
                    hookPoint.Hooked = true;
                    hook.isHooked = true;

                }
                else {
                    UpdateHook();
                    Debug.Log("Missing Target");
                }
            }

            if (hook.isHooked) {
                hook.transform.position = hit.transform.position;
                hook.Inertia = Vector3.zero;
                //Call hookPoint function
            }




        }
    }

    // Private
    private ArrayList RopeNodes;
    private Vector3 HookPosition;
    private Vector3 HookScale;
    private Quaternion HookRotation;
    private GameObject HookParent;
    private RaycastHit hit;

    private void Start() {
        RopeNodes = new ArrayList();
        HookParent = hook.transform.parent.gameObject;
        HookPosition = hook.transform.localPosition;
        HookScale = hook.transform.localScale;
        HookRotation = hook.transform.localRotation;
    }

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
        float dynamicFriction = 0.5f;
        float springValue = 1;
        float pointsDistance = 0;
        float error = 0;
        Vector3 shootDirection = Vector3.zero;
        Vector3 previousInstance = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        RopeNode firstInstanceNode = RopeNodes[0] as RopeNode;
        hook.OldPos = hook.transform.position;
        hook.transform.position = hook.transform.position + hook.Inertia;
        hook.Inertia = (hook.transform.position - hook.OldPos) * (1 - hook.DynamicFriction);
        pointsDistance = (firstInstanceNode.transform.position - hook.transform.position).magnitude;


        float directionX = Mathf.Abs(firstInstanceNode.transform.position.x - hook.transform.position.x);
        float directionY = Mathf.Abs(firstInstanceNode.transform.position.z - hook.transform.position.z);
        float angle = 1 / Mathf.Tan(directionX / directionY);

        if (hook.ropeFinished && pointsDistance > MassPointLength) {
            hook.DynamicFriction = 0.4f;
            error = pointsDistance - MassPointLength;
            direction = (firstInstanceNode.transform.position - hook.transform.position).normalized;
            movement = error * direction;
            hook.transform.position -= movement * springValue;
            firstInstanceNode.transform.position += movement * (1 - springValue);
            hook.Inertia = (hook.transform.position - hook.OldPos) * (1 - hook.DynamicFriction);
            firstInstanceNode.Inertia = (firstInstanceNode.transform.position - firstInstanceNode.OldPos) * (1 - dynamicFriction);
            hook.transform.position = new Vector3(hook.transform.position.x, 1.375f, hook.transform.position.z);


        }

        
            
        
        

            //if (hit.transform == null) {
            //    Debug.Log("Missing Target");
            //}
            //else {
            //    Debug.Log(hit.transform.gameObject.name + "---");
            //    hookPoint = hit.transform.gameObject.GetComponent<HookPoint>();
            //    hookPoint.Hooked = true;
            //    hook.isHooked = true;
            //}


            //if (hook.isHooked) {
            //    hook.transform.position = hit.point;
            //    hook.Inertia = Vector3.zero;
            //    //Call hookPoint function

            //}

        
    }

    public void UpdatePoints() {
        float springValue = 1f;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float error = 0;
        Vector3 previousInstance = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        RopeNode massPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
        RopeNode nextMassPoint;
        
        massPoint.OldPos = massPoint.transform.position;
        massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
        massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
        pointsDistance = (GrappleGun.transform.position - massPoint.transform.position).magnitude; 

        if (pointsDistance > MassPointLength) {

            error = pointsDistance - MassPointLength;
            direction = (GrappleGun.transform.position - massPoint.transform.position).normalized;
            movement = error * direction;
            massPoint.transform.position += movement;
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
            pointsDistance = (GrappleGun.transform.position - massPoint.transform.position).magnitude;
        }

        for (int i = 1; i < RopeNodes.Count; i++) {
            massPoint = RopeNodes[RopeNodes.Count-1-i] as RopeNode;
            nextMassPoint = RopeNodes[RopeNodes.Count - i] as RopeNode;
            massPoint.OldPos = massPoint.transform.position;
            massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
            massPoint.Inertia = massPoint.transform.position - massPoint.OldPos;
            pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;

            if (pointsDistance > MassPointLength) {

                error = pointsDistance - MassPointLength;
                direction = (nextMassPoint.transform.position - massPoint.transform.position).normalized;
                movement = error * direction;
                massPoint.transform.position += movement * springValue;
                nextMassPoint.transform.position -= movement * (1-springValue);
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                nextMassPoint.Inertia = (nextMassPoint.transform.position - nextMassPoint.OldPos) * (1-DynamicFriction);
            }
        }
    }

    public void UpdateLinks() {
        float springValue = 0.975f;
        float DynamicFriction = 0.5f;
        float SumOfLength = 0;
        float pointsDistance = 0;
        float error = 0;
        Vector3 previousInstance = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        RopeNode nextMassPoint;
        RopeNode massPoint = RopeNodes[0] as RopeNode;
        RopeNode lastMassPoint;
        
        pointsDistance = (hook.transform.position-massPoint.transform.position).magnitude;

        if (pointsDistance > MassPointLength) {
            error = pointsDistance - MassPointLength;
            direction = (hook.transform.position - massPoint.transform.position).normalized;
            movement = error * direction;
            massPoint.transform.position +=  movement;
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
            pointsDistance = (hook.transform.position - massPoint.transform.position).magnitude; 

        }
        SumOfLength += pointsDistance;

        for (int i = 1; i < RopeNodes.Count; i++) {
            massPoint = RopeNodes[i] as RopeNode;
            nextMassPoint = RopeNodes[i - 1] as RopeNode;
            pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude; 

            if (pointsDistance > MassPointLength) {

                error = pointsDistance - MassPointLength;
                direction = (massPoint.transform.position - nextMassPoint.transform.position).normalized;
                movement = error * direction;
                massPoint.transform.position -= movement * springValue;
                nextMassPoint.transform.position += movement * (1-springValue);
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                nextMassPoint.Inertia = (nextMassPoint.transform.position - nextMassPoint.OldPos) * (1 - DynamicFriction);
                pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;
            }

            SumOfLength += pointsDistance;
            //Debug.LogFormat("SumOfLength(for): {0}", SumOfLength);
        }

        lastMassPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
        pointsDistance = (GrappleGun.transform.position - lastMassPoint.transform.position).magnitude;

        if (pointsDistance > MassPointLength) {

            error = pointsDistance - MassPointLength;
            direction = (GrappleGun.transform.position - lastMassPoint.transform.position).normalized;
            movement = error * direction;
            Player.transform.position -= movement * springValue;
            lastMassPoint.transform.position += movement * (1-springValue);
            lastMassPoint.Inertia = (lastMassPoint.transform.position - lastMassPoint.OldPos) * (1 - DynamicFriction);
            pointsDistance = (lastMassPoint.transform.position - GrappleGun.transform.position).magnitude;
        }

        lastMassPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
        pointsDistance = (lastMassPoint.transform.position - GrappleGun.transform.position).magnitude;
        SumOfLength += pointsDistance;

        if (SumOfLength*(2-springValue) > RopeNodes.Count * MassPointLength) {
           hook.ropeFinished = true;
       }
       else {
           hook.ropeFinished = false;
       }
        
    }

    

    public void RewindPoints() {
        int pointsRewinded = 0;
        float springValue = 1f;
        float DynamicFriction = 0.125f;
        float pointsDistance = 0;
        float error = 0;
        float pointsDisatnce = 0;
        Vector3 previousInstance = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        RopeNode massPoint;
        RopeNode nextMassPoint;

        //funzione 
        if (GrappleGun.RewindVector < GrappleGun.RewindTopSpeed) {
            GrappleGun.RewindVector += GrappleGun.RewindAcceleration * Time.deltaTime; 
        }
        else {
            GrappleGun.RewindVector = GrappleGun.RewindTopSpeed * Time.deltaTime; 
        }
        //funzione

        pointsRewinded = (int) Mathf.Ceil(GrappleGun.RewindVector / MassPointLength) - 1;


        if (pointsRewinded >= RopeNodes.Count-1) {
            Debug.LogFormat("PointsRewinded: {0}", pointsRewinded);
            DestroyMassPoints(0);
            hook.shooted = false;
            hook.ropeFinished = false;
            hook.isHooked = false;
        }
        else {
            for (int i = 0; i < pointsRewinded; i++) {
                massPoint = RopeNodes[RopeNodes.Count - 1 - i] as RopeNode;
                massPoint.transform.position = new Vector3(GrappleGun.transform.position.x - MassPointLength * Mathf.Cos(GrappleGun.transform.rotation.x), GrappleGun.transform.position.y, GrappleGun.transform.position.z - MassPointLength * Mathf.Sin(GrappleGun.transform.rotation.z));
                
            }
            massPoint = RopeNodes[RopeNodes.Count - 1 - pointsRewinded] as RopeNode;
            massPoint.OldPos = massPoint.transform.position;
            massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1-DynamicFriction);
            pointsDisatnce = Vector3.Distance(GrappleGun.transform.position, massPoint.transform.position);
            
            if (pointsDisatnce > MassPointLength) {
                error = pointsDisatnce - MassPointLength;
                direction = (GrappleGun.transform.position - massPoint.transform.position).normalized;
                movement = error * direction;
                massPoint.transform.position += movement * springValue;
                GrappleGun.transform.position -= movement * (1 - springValue);
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);

            }
            previousInstance = massPoint.transform.position;

            for (int i = pointsRewinded + 1; i < RopeNodes.Count; i++) {
                massPoint = RopeNodes[RopeNodes.Count - 1 - i] as RopeNode;
                nextMassPoint = RopeNodes[RopeNodes.Count - i] as RopeNode;
                massPoint.OldPos = massPoint.transform.position;
                massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
                massPoint.Inertia = massPoint.transform.position - massPoint.OldPos;
                pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;

                if (pointsDistance > MassPointLength) {

                    error = pointsDistance - MassPointLength;
                    direction = (nextMassPoint.transform.position - massPoint.transform.position).normalized;
                    movement = error * direction;
                    massPoint.transform.position += movement * springValue;
                    nextMassPoint.transform.position -= movement * (1 - springValue);
                    massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                    nextMassPoint.Inertia = (nextMassPoint.transform.position - nextMassPoint.OldPos) * (1 - DynamicFriction);
                }
            }
        }
        UpdateHook();
        UpdateRewind(RopeNodes.Count-1-pointsRewinded);
    }

    public void UpdateRewind(int _index) {
        float springValue = 1f;
        float DynamicFriction = 0.125f;
        float pointsDistance = 0;
        float error = 0;
        float SumOfLength = 0;
        float pointsDisatnce = 0;
        float pointsUnwinded = 0;
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;
        Vector3 previousInstance = Vector3.zero;

        RopeNode massPoint;
        RopeNode nextMassPoint;

        if (_index <= 0) {
            hook.shooted = false;
            hook.ropeFinished = false;
            hook.isHooked = false;
        }
        else {
            massPoint = RopeNodes[0] as RopeNode;
            pointsDisatnce = Vector3.Distance(hook.transform.position, massPoint.transform.position);

            if (pointsDistance > MassPointLength) {
                error = pointsDistance - MassPointLength;
                direction = (hook.transform.position - massPoint.transform.position).normalized;
                movement = error * direction;
                massPoint.transform.position += movement;
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                pointsDistance = (hook.transform.position - massPoint.transform.position).magnitude;
            }
            SumOfLength += pointsDistance;

            for (int i = 1; i < _index; i++) {
                massPoint = RopeNodes[i] as RopeNode;
                nextMassPoint = RopeNodes[i - 1] as RopeNode;
                pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;

                if (pointsDistance > MassPointLength) {

                    error = pointsDistance - MassPointLength;
                    direction = (massPoint.transform.position - nextMassPoint.transform.position).normalized;
                    movement = error * direction;
                    massPoint.transform.position -= movement * springValue;
                    nextMassPoint.transform.position += movement * (1 - springValue);
                    massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                    nextMassPoint.Inertia = (nextMassPoint.transform.position - nextMassPoint.OldPos) * (1 - DynamicFriction);
                    pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;
                }
                SumOfLength += pointsDistance;
            }

            SumOfLength += pointsDisatnce;
            pointsDisatnce = Vector3.Distance(previousInstance, GrappleGun.transform.position);

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
                if (pointsDisatnce > MassPointLength) {
                    error = pointsDistance - MassPointLength;
                    direction = (massPoint.transform.position - GrappleGun.transform.position).normalized;
                    movement = error * direction;
                    massPoint.transform.position -= movement * springValue;
                    Player.transform.position += movement * (1 - springValue);
                    massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                    pointsDistance = (massPoint.transform.position - GrappleGun.transform.position).magnitude;
                }
            }

            DestroyMassPoints(_index + 1);

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
                pointsUnwinded = Mathf.Ceil(pointsDistance / MassPointLength) - 1;

                for (int i = _index+1; i <= _index + 1 + pointsUnwinded; i++) {
                    massPoint = RopeNodes[i] as RopeNode;
                    nextMassPoint = RopeNodes[i - 1] as RopeNode;

                    pointsDistance = Vector3.Distance(previousInstance, massPoint.transform.position);

                    if (pointsDistance > MassPointLength) {
                        error = pointsDistance - MassPointLength;
                        direction = (massPoint.transform.position - nextMassPoint.transform.position).normalized;
                        movement = error * direction;
                        massPoint.transform.position -= movement * springValue;
                        nextMassPoint.transform.position += movement * (1 - springValue);
                        massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                        nextMassPoint.Inertia = (nextMassPoint.transform.position - nextMassPoint.OldPos) * (1 - DynamicFriction);
                        pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;
                    }
                    SumOfLength += pointsDisatnce;
                    
                }

                if (pointsDisatnce > MassPointLength) {
                    error = pointsDistance - MassPointLength;
                    direction = (massPoint.transform.position - GrappleGun.transform.position).normalized;
                    movement = error * direction;
                    massPoint.transform.position -= movement * springValue;
                    Player.transform.position += movement * (1 - springValue);
                    massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                    pointsDistance = (massPoint.transform.position - GrappleGun.transform.position).magnitude;
                }

                DestroyMassPoints(_index + 1);

                if (SumOfLength * (2 - springValue) > RopeNodes.Count * MassPointLength) {
                    hook.ropeFinished = true;
                }
                
            }
        }
    }

    public void HookFired() {
        UpdatePoints();
        UpdateLinks();
        
    }

    public void Rewind() {
        RewindPoints();
    }

    public void DestroyMassPoints(int _fromIndex) {
        RopeNode massPoint;

        if (_fromIndex == 0) {
            for (int i = _fromIndex; i < RopeNodes.Count; i++) {
                massPoint = RopeNodes[i] as RopeNode;
                Destroy(massPoint.transform.gameObject);
            }
            RopeNodes.Clear();
            HookReset();
            ResetRewindVector();
        }
        else {
            for (int i = _fromIndex; i < RopeNodes.Count; i++) {
                massPoint = RopeNodes[i] as RopeNode;
                Destroy(massPoint.transform.gameObject);
                RopeNodes.RemoveAt(i);
            }
        }
    }

    void HookReset() {
        hook.transform.SetParent(HookParent.transform);
        hook.transform.localPosition = new Vector3(HookPosition.x , HookPosition.y , HookPosition.z);
        hook.transform.localScale = new Vector3(HookScale.x, HookScale.y, HookScale.z);
        hook.transform.localRotation = new Quaternion(HookRotation.x , HookRotation.y , HookRotation.z , HookRotation.w);
        hook.OldPos = Vector3.zero;
        hook.Inertia = Vector3.zero;
        hook.DynamicFriction = 0;
        
    }

    void ResetRewindVector() {
        GrappleGun.RewindVector = 0;
    }

    public void HookShooting() {
        Vector3 shootDirection;
        Vector3 crosshairPosition;
        hook.transform.SetParent(this.transform);
        if (!hook.shooted) {
            hook.ropeFinished = false;
            hook.isHooked = false;
            hook.OldPos = hook.transform.position;
            crosshairPosition = new Vector3(CrossHair.transform.position.x, hook.transform.position.y, CrossHair.transform.position.z);
            shootDirection = (crosshairPosition - hook.transform.position).normalized;
            hook.Inertia = hook.topSpeed * shootDirection * Time.deltaTime;
        }
    }

    

}
