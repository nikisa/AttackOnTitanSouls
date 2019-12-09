using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    // Inspector
    public bool debugMode;
    public GrappleGun GrappleGun;
    public Hook hook;
    [HideInInspector]
    public HookPlaque hookPlaque;
    public PlayerController Player;


    

    public int MassPointNumber = 32;
    public float MassPointLength = 0.25f;
    public float hookDynamicFriction = 0f;


    private void Update() {

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
                    UpdatePoints();
                    UpdateHook();
                    UpdateLinks();
            }
        }
        

    }

    // Private
    private ArrayList RopeNodes;

    private void Start() {
        RopeNodes = new ArrayList();
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
        float changeX = 0;
        float changeY = 0;
        Vector3 previousInstance = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        RopeNode firstInstanceNode = RopeNodes[0] as RopeNode;
        hook.OldPos = hook.transform.position;
        hook.transform.position = hook.transform.position + hook.Inertia;
        hook.Inertia = (hook.transform.position - hook.OldPos) * (1-hookDynamicFriction);
        pointsDistance = (firstInstanceNode.transform.position - hook.transform.position).magnitude;//Vector3.Distance(firstInstanceNode.transform.position , hook.transform.position);

        RaycastHit hit;
        Physics.Raycast(hook.transform.position, hook.transform.forward, out hit, Mathf.Infinity);
        Debug.DrawRay(hook.transform.position, hook.transform.forward, Color.blue);

        if (hit.transform.gameObject.GetComponent<HookPlaque>()) {
            hookPlaque.Hooked = true;
            hook.isHooked = true;
            hook.transform.position = hookPlaque.transform.position;
            hook.Inertia = Vector3.zero;
        }

        float directionX = Mathf.Abs(firstInstanceNode.transform.position.x - hook.transform.position.x);
        float directionY = Mathf.Abs(firstInstanceNode.transform.position.z - hook.transform.position.z);

        float angle = 1/Mathf.Tan(directionX/directionY);
        Debug.LogFormat("UpdateHook Angle: {0}", angle);
        Debug.LogFormat("DirectionX: {0}  ---- DirectionY: {1}", directionX , directionY);


        
        if (hook.ropeFinished && pointsDistance > MassPointLength) {
            hookDynamicFriction = 0.4f;
            error = pointsDistance - MassPointLength;
            direction = (hook.transform.position - firstInstanceNode.transform.position).normalized;
            movement = error * direction;
            hook.transform.position -= movement * springValue;
            firstInstanceNode.transform.position += movement * (1 - springValue);
            hook.Inertia = (hook.transform.position - hook.OldPos) * (1 - hookDynamicFriction);
            firstInstanceNode.Inertia = (firstInstanceNode.transform.position - firstInstanceNode.OldPos) * (1 - dynamicFriction);

            //error = pointsDistance - MassPointLength;
            //changeX = error * Mathf.Cos(Vector3.Angle(firstInstanceNode.transform.position, hook.transform.position));
            //changeY = error * Mathf.Sin(Vector3.Angle(firstInstanceNode.transform.position, hook.transform.position));
            //hook.transform.position = new Vector3(hook.transform.position.x - (changeX * springValue), hook.transform.position.y, hook.transform.position.z - (changeY * springValue));
            //hook.Inertia = (hook.transform.position - hook.OldPos) * (1 - DynamicFriction);
            //firstInstanceNode.transform.position = new Vector3(firstInstanceNode.transform.position.x + changeX * (1-springValue) , firstInstanceNode.transform.position.y , firstInstanceNode.transform.position.z + changeY * (1-springValue));
            //firstInstanceNode.Inertia = (firstInstanceNode.transform.position - firstInstanceNode.OldPos) * (1 - springValue);

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
        Vector3 previousInstance = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        RopeNode massPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
        RopeNode nextMassPoint;

        massPoint.OldPos = massPoint.transform.position;
        massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
        massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
        pointsDistance = (GrappleGun.transform.position - massPoint.transform.position).magnitude; //Vector3.Distance(GrappleGun.transform.position, massPoint.transform.position);

        if (pointsDistance > MassPointLength) {

            error = pointsDistance - MassPointLength;
            direction = (GrappleGun.transform.position - massPoint.transform.position).normalized;
            movement = error * direction;
            massPoint.transform.position += movement;
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
            pointsDistance = (GrappleGun.transform.position - massPoint.transform.position).magnitude;

            //error = pointsDistance - MassPointLength;
            //changeX = error * Mathf.Sin(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position));
            //changeY = error * Mathf.Cos(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position));
            //massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX, massPoint.transform.position.y, massPoint.transform.position.z + changeY);
            //massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1-DynamicFriction);
        }


        for (int i = 1; i < RopeNodes.Count; i++) {
            massPoint = RopeNodes[RopeNodes.Count-1-i] as RopeNode;
            nextMassPoint = RopeNodes[RopeNodes.Count - i] as RopeNode;
            massPoint.OldPos = massPoint.transform.position;
            massPoint.transform.position = massPoint.transform.position + massPoint.Inertia;
            massPoint.Inertia = massPoint.transform.position - massPoint.OldPos;
            pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;//Vector3.Distance(previousInstance, massPoint.transform.position);

            if (pointsDistance > MassPointLength) {

                error = pointsDistance - MassPointLength;
                direction = (nextMassPoint.transform.position - massPoint.transform.position).normalized;
                movement = error * direction;
                massPoint.transform.position += movement * springValue;
                nextMassPoint.transform.position -= movement * (1-springValue);
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                nextMassPoint.Inertia = (nextMassPoint.transform.position - nextMassPoint.OldPos) * (1-DynamicFriction);


                //for (int i = 1; i < RopeNodes.Count; i++) {
                //    massPoint = RopeNodes[i] as RopeNode;
                //    nextMassPoint = RopeNodes[i - 1] as RopeNode;
                //    pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude; //Vector3.Distance(previousInstance, massPoint.transform.position);

                //    if (pointsDistance > MassPointLength) {

                //        error = pointsDistance - MassPointLength;
                //        direction = (nextMassPoint.transform.position - massPoint.transform.position).normalized;
                //        movement = error * direction;
                //        massPoint.transform.position -= movement * springValue;
                //        nextMassPoint.transform.position += movement * (1 - springValue);
                //        massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                //        nextMassPoint.Inertia = (nextMassPoint.transform.position - nextMassPoint.OldPos) * (1 - DynamicFriction);
                //        pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;




                        //error = pointsDistance - MassPointLength;
                        //changeX = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                        //changeY = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                        //massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX * springValue, massPoint.transform.position.y, massPoint.transform.position.z + changeY * springValue);
                        //massPoint.Inertia = massPoint.transform.position - massPoint.OldPos;
                    }

                    //if (pointsDistance > MassPointLength) {
                    //    massPoint = RopeNodes[RopeNodes.Count - i] as RopeNode;
                    //    massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX * (1 - springValue), massPoint.transform.position.y, massPoint.transform.position.z + changeY * (1 - springValue));
                    //    massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                    //}
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
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        RopeNode nextMassPoint;
        RopeNode massPoint = RopeNodes[0] as RopeNode;
        
        pointsDistance = (hook.transform.position-massPoint.transform.position).magnitude;//Vector3.Distance(hook.transform.position, massPoint.transform.position);

        if (pointsDistance > MassPointLength) {
            error = pointsDistance - MassPointLength;
            direction = (hook.transform.position - massPoint.transform.position).normalized;
            movement = error * direction;
            massPoint.transform.position +=  movement;
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
            pointsDistance = (hook.transform.position - massPoint.transform.position).magnitude; //Vector3.Distance(hook.transform.position, massPoint.transform.position);

            //changeX = error * Mathf.Sin(Vector3.Angle(hook.transform.position, massPoint.transform.position));
            //changeY = error * Mathf.Cos(Vector3.Angle(hook.transform.position, massPoint.transform.position));
            //massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX, massPoint.transform.position.y, massPoint.transform.position.z + changeY);
            //Debug.LogFormat("Angle: {0}", angle);
            //Debug.LogFormat("ChangeX: {0} --- ChangeY: {1}" , changeX , changeY);
            //massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);

        }
        //previousInstance = massPoint.transform.position;
        SumOfLength += pointsDistance;

        for (int i = 1; i < RopeNodes.Count; i++) {
            massPoint = RopeNodes[i] as RopeNode;
            nextMassPoint = RopeNodes[i - 1] as RopeNode;
            pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude; //Vector3.Distance(previousInstance, massPoint.transform.position);

            if (pointsDistance > MassPointLength) {

                error = pointsDistance - MassPointLength;
                direction = (massPoint.transform.position - nextMassPoint.transform.position).normalized;
                movement = error * direction;
                massPoint.transform.position -= movement * springValue;
                nextMassPoint.transform.position += movement * (1-springValue);
                massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                nextMassPoint.Inertia = (nextMassPoint.transform.position - nextMassPoint.OldPos) * (1 - DynamicFriction);
                pointsDistance = (massPoint.transform.position - nextMassPoint.transform.position).magnitude;


                //error = pointsDistance - MassPointLength;
                //changeX = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                //changeY = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                //massPoint.transform.position = new Vector3(massPoint.transform.position.x + changeX * springValue, massPoint.transform.position.y, massPoint.transform.position.z + changeY * springValue);
                //massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
            }

            SumOfLength += pointsDistance;
        }

        massPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
        pointsDistance = (massPoint.transform.position - GrappleGun.transform.position).magnitude; //Vector3.Distance(previousInstance, GrappleGun.transform.position);

        if (pointsDistance > MassPointLength) {

            error = pointsDistance - MassPointLength;
            direction = (massPoint.transform.position - GrappleGun.transform.position).normalized;
            movement = error * direction;
            massPoint.transform.position -= movement * springValue;
            GrappleGun.transform.position += movement * (1 - springValue);
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
            pointsDistance = (massPoint.transform.position - GrappleGun.transform.position).magnitude;

            //error = pointsDistance - MassPointLength;
            //changeX = error * Mathf.Sin(Vector3.Angle(previousInstance , GrappleGun.transform.position));
            //changeY = error * Mathf.Cos(Vector3.Angle(previousInstance, GrappleGun.transform.position));
            //Player.transform.position = new Vector3(Player.transform.position.x + changeX * springValue, Player.transform.position.y, Player.transform.position.z + changeY * springValue);
            //massPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
            //massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX * (1 - springValue), massPoint.transform.position.y, massPoint.transform.position.z - changeY * (1 - springValue));
            //massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
        }

        massPoint = RopeNodes[RopeNodes.Count - 1] as RopeNode;
        pointsDistance = (massPoint.transform.position - GrappleGun.transform.position).magnitude;
        SumOfLength += pointsDistance;
        Debug.LogFormat("SumOfLength = {0} ", SumOfLength);

        if (SumOfLength*(2-springValue) > RopeNodes.Count * MassPointLength) {
           hook.ropeFinished = true;
       }
       else {
           hook.ropeFinished = false;
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
            GrappleGun.RewindVector += GrappleGun.RewindAcceleration * Time.deltaTime ; 
        }
        else {
            GrappleGun.RewindVector = GrappleGun.RewindTopSpeed * Time.deltaTime; 
        }

        pointsRewinded = (int) Mathf.Ceil(GrappleGun.RewindVector / MassPointLength) - 1;

        if (pointsRewinded >= RopeNodes.Count) {
            hook.shooted = false;
            hook.ropeFinished = false;
            hook.isHooked = false;
            DisableMassPoints(0);
        }
        else {
            for (int i = 0; i < pointsRewinded; i++) {
                massPoint = RopeNodes[RopeNodes.Count - 1 - i] as RopeNode;
                massPoint.transform.position = new Vector3(GrappleGun.transform.position.x /* - MassPointLength  * Mathf.Cos(GrappleGun.transform.rotation)*/ , GrappleGun.transform.position.y, GrappleGun.transform.position.z /* - MassPointLength Mathf.Sin(GrappleGun.transform.rotation)*/);                        
            }
            massPoint = RopeNodes[0] as RopeNode;
            massPoint.transform.position = massPoint.OldPos;
            massPoint.transform.position = new Vector3(massPoint.transform.position.x + massPoint.Inertia.x - GrappleGun.RewindVector * Mathf.Sin(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position)), massPoint.transform.position.y, massPoint.transform.position.z + massPoint.Inertia.z - GrappleGun.RewindVector * Mathf.Cos(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position)));
            massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1-DynamicFriction);
            pointsDisatnce = Vector3.Distance(GrappleGun.transform.position, massPoint.transform.position);

            if (pointsDisatnce > MassPointLength) {
                error = pointsDisatnce - MassPointLength;
                changeX = error * Mathf.Sin(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position));
                changeY = error * Mathf.Cos(Vector3.Angle(GrappleGun.transform.position, massPoint.transform.position));
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
                changeX = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                changeY = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
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
        UpdateHook();
        UpdateRewind(RopeNodes.Count-1-pointsRewinded);
    }

    public void UpdateRewind(int _index) {
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
            changeX = error * Mathf.Sin(Vector3.Angle(hook.transform.position, massPoint.transform.position));
            changeY = error * Mathf.Cos(Vector3.Angle(hook.transform.position, massPoint.transform.position));

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
                changeX = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                changeY = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
            }

            previousInstance = massPoint.transform.position;
            SumOfLength += pointsDisatnce;
        }
        pointsDisatnce = Vector3.Distance(previousInstance, GrappleGun.transform.position);

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
            if (pointsDisatnce > MassPointLength) {
                error = pointsDisatnce - MassPointLength;
                changeX = error * Mathf.Sin(Vector3.Angle(previousInstance, GrappleGun.transform.position));
                changeY = error * Mathf.Cos(Vector3.Angle(previousInstance, GrappleGun.transform.position));
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
                    changeX = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                    changeY = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));

                    massPoint.transform.position = new Vector3(massPoint.transform.position.x - changeX * springValue, massPoint.transform.position.y, massPoint.transform.position.z - changeY * springValue);
                    massPoint.Inertia = (massPoint.transform.position - massPoint.OldPos) * (1 - DynamicFriction);
                }
                previousInstance = massPoint.transform.position;
                SumOfLength += pointsDisatnce;
            }

            if (pointsDisatnce > MassPointLength) {
                error = pointsDistance - MassPointLength;
                changeX = error * Mathf.Sin(Vector3.Angle(previousInstance, massPoint.transform.position));
                changeY = error * Mathf.Cos(Vector3.Angle(previousInstance, massPoint.transform.position));
                springValue = 1;
                Player.transform.position = new Vector3(Player.transform.position.x - changeX * springValue, Player.transform.position.y, Player.transform.position.z - changeY * springValue);
                massPoint.gameObject.SetActive(false);
            }

            if (SumOfLength * (2 - springValue) > RopeNodes.Count * MassPointLength) {
                hook.ropeFinished = true;
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

    
    public void DisableMassPoints(int _fromIndex) {
        RopeNode massPoint;
        for (int i = _fromIndex; i < RopeNodes.Count; i++) {
            massPoint = RopeNodes[i] as RopeNode;
            massPoint.gameObject.SetActive(false);
        }
    }

    public void HookShooting() {
        if (!hook.shooted) {
            hook.ropeFinished = false;
            hook.isHooked = false;
            hook.OldPos = hook.transform.position;
            hook.VectorAngle = hook.transform.eulerAngles.y;
            hook.Inertia = new Vector3(hook.topSpeed * Mathf.Sin(hook.VectorAngle) * Time.deltaTime, 0 ,  hook.topSpeed * Mathf.Cos(hook.VectorAngle) * Time.deltaTime); 
        }
    }

}
