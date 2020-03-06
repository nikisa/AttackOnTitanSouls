using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMask : HookPointBase
{

    public int MaskID;
    public float Mass;
    [Range(0, 1)]
    public float KineticEnergyLoss;
    [Range(0, 1)]
    public float SurfaceFriction;
    public BossController boss;
    public Vector3 OldPos;
    public Vector3 Inertia;
    [Tooltip("Insert BreakPointData")]
    public BreakPointData[] BreakPoints;
    public GameObject[] graphics;
    public ParticleSystem[] particles;

    //Public
    //[HideInInspector]
    public float AngularAccelerationModule;
    //[HideInInspector]
    public float AngularDecelerationModule;
    [HideInInspector]
    public Vector3 AngularAcceleration;
    //[HideInInspector]
    public float AngularVelocity;
    //[HideInInspector]
    public Vector3 VelocityVector;
    //[HideInInspector]
    public float Drag;
    [HideInInspector]
    public Vector3 DecelerationVector;
    [HideInInspector]
    public Vector3 targetDir;
    [HideInInspector]
    public float currentRadius;
    //[HideInInspector]
    public float distanceFromBoss;
    [HideInInspector]
    public bool isDetected; //Used for the reassemble statement

    //Private
    BreakPointData actualBreakPointData;
    [SerializeField]
    private int BreakPointsCount = 0;
    private float springVector;
    [SerializeField]
    private float currentLife;
    private float currentElasticK;
    private float distance;
    Vector3 direction = Vector3.zero;
    Vector3 movement = Vector3.zero;
    float maskY;
    #region BounceVariable
    Vector3 normal;
    Vector3 vectorParal;
    Vector3 vectorPerp;
    Vector3 collisionVectorParal;
    Vector3 collisionVectorPerp;
    Vector3 bounceVector;
    #endregion

    GameObject mask;
    GameObject parent;
    BossOrbitManager bossOrbitManager;
    

    private void Awake() {//da spostare quando ci sarà GameManager
        SetUp();
    }

    void SetUp() {
        parent = transform.parent.transform.gameObject;
        bossOrbitManager = FindObjectOfType<BossOrbitManager>();
        currentLife = BreakPoints[BreakPointsCount].lifeMax;
        OldPos = transform.position;
        mask = Instantiate(graphics[0] , transform.position - new Vector3(0, 1.375f,0) , transform.rotation);
        mask.transform.SetParent(transform);
        maskY = 1.375f;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, maskY, transform.position.z);
    }

    public void SetDistanceFromBoss(float _distance) {
        distanceFromBoss = _distance;
    }

    public void RotateAroud(float _angularMaxSpeed , float _angularAccelerationTime) {
        AngularAccelerationModule = _angularMaxSpeed / _angularAccelerationTime;
        Drag = AngularAccelerationModule / _angularMaxSpeed * Time.deltaTime;
        AngularVelocity -= AngularVelocity * Drag;
        transform.eulerAngles += new Vector3(0, AngularVelocity * Time.deltaTime + 0.5f * AngularAccelerationModule * Mathf.Pow(Time.deltaTime, 2), 0);
        transform.position = new Vector3(boss.transform.position.x + currentRadius * Mathf.Sin((transform.eulerAngles.y) * Mathf.Deg2Rad), 1.375f , boss.transform.position.z + currentRadius * Mathf.Cos((transform.eulerAngles.y) * Mathf.Deg2Rad));
        AngularVelocity += AngularAccelerationModule * Time.deltaTime;
        VelocityVector = new Vector3((AngularVelocity * Mathf.PI / 180) * currentRadius * Mathf.Sin(transform.eulerAngles.x), 0, (AngularVelocity * Mathf.PI / 180) * currentRadius * Mathf.Cos(transform.eulerAngles.z));
    }

    public void DecelerateAround(float _angularDecelerationModule) {
        if (Mathf.Abs(AngularVelocity) > Mathf.Abs(_angularDecelerationModule) * Time.deltaTime) {
            AngularVelocity -= _angularDecelerationModule * Time.deltaTime;
            parent.transform.eulerAngles += new Vector3(0, AngularVelocity * Time.deltaTime, 0);
            parent.transform.position = new Vector3(boss.transform.position.x + currentRadius * Mathf.Sin((parent.transform.eulerAngles.y) * Mathf.Deg2Rad), 0, boss.transform.position.z + currentRadius * Mathf.Cos((parent.transform.eulerAngles.y) * Mathf.Deg2Rad));
            VelocityVector = new Vector3((AngularVelocity * Mathf.PI / 180) * currentRadius * Mathf.Sin(parent.transform.eulerAngles.x), 0, (AngularVelocity * Mathf.PI / 180) * currentRadius * Mathf.Cos(parent.transform.eulerAngles.z));
        }
        else {
            AngularVelocity = 0;
            VelocityVector = Vector3.zero;
        }
    }

    public void RotationReset() {

        AngularVelocity = 0;
        VelocityVector = Vector3.zero;
        DecelerationVector = Vector3.zero;
        Drag = 0;
        AngularAccelerationModule = 0;
        AngularDecelerationModule = 0;

    }

    public void SetRadius(float _setupRadius) {
        currentRadius = _setupRadius;
    }

    public void setAngularDecelerationModule(float _angularMaxSpeed , float _angularDecelerationTime) {
        AngularDecelerationModule = _angularMaxSpeed / _angularDecelerationTime;
    }


    //DA REFACTORARE
    public void HookPointSpring() {
        float lifeRatio;

        if (BreakPointsCount < BreakPoints.Length) {
            actualBreakPointData = BreakPoints[BreakPointsCount];
        }
        
        lifeRatio = Mathf.Clamp01(currentLife / actualBreakPointData.lifeMax);

        transform.position = hook.transform.position;
        Inertia = transform.position - OldPos;
        distance = Vector3.Distance(HookPointPivot.position, transform.position);
        springVector = currentElasticK * distance;
        currentElasticK = Mathf.Lerp(actualBreakPointData.FinalElasticK, actualBreakPointData.InitialElasticK, lifeRatio);

        direction = (hook.transform.position - HookPointPivot.position).normalized;
        movement = direction * springVector * Time.fixedDeltaTime;
        transform.position -= movement;
        hook.transform.position = transform.position;
        OldPos = transform.position;
        
        if (springVector >= Inertia.magnitude && distance > .2f) {
            currentLife -= player.DPS * Time.fixedDeltaTime;
        }

        if (currentLife < 0) {
            
            if (BreakPointsCount < graphics.Length) {

                if (BreakPointsCount == graphics.Length-1 && transform.childCount > 0 && currentLife < 0) {
                    
                    transform.GetChild(0).gameObject.transform.position = hook.transform.position;
                }
                else {
                    Destroy(transform.GetChild(0).gameObject);
                }

                BreakPointsCount++;
                currentLife = BreakPoints[BreakPointsCount].lifeMax; //ERRORE DOVUTO ALL'AGGIORNAMENTO DI BREAKPOINTS COUNT DOPO AVER ELIMINATO L'ULTIMA VITA DELLA MASCHERA
                GameObject mask = Instantiate(graphics[BreakPointsCount].gameObject , transform.position - new Vector3(0, 1.375f, 0), transform.rotation);
                mask.transform.SetParent(transform);
                //ParticleSystem particle = Instantiate(particles[BreakPointsCount-1] as ParticleSystem, transform.position, Quaternion.identity);
                if (BreakPointsCount == 2) {
                    //particle = Instantiate(particles[BreakPointsCount] as ParticleSystem, transform.position, Quaternion.identity);
                }
            }

            if (BreakPointsCount == graphics.Length && transform.childCount > 0 && currentLife < 0) {
                Destroy(transform.GetChild(0).gameObject);
                //Instantiate(graphics[0], hook.transform.position /*- new Vector3(0, 1.375f, 0)*/, transform.rotation);
            }


            //UPDATING LIST AFTER DESTROYING A MASK
            if (BreakPointsCount == graphics.Length && currentLife < 0) {
                int index;

                index = bossOrbitManager.MasksList.IndexOf(this);
                bossOrbitManager.removedIndexList.Add(index);

                Debug.Log(index);


                bossOrbitManager.OrbitList.Remove(this.transform.parent.gameObject);
                bossOrbitManager.MasksList.Remove(this);
                Destroy(this.gameObject.transform.parent.gameObject);
                transform.gameObject.SetActive(false);

                Destroy(bossOrbitManager.InitialPoints[index]);
                bossOrbitManager.InitialPoints.Remove(bossOrbitManager.InitialPoints[index]);
                Destroy(bossOrbitManager.EndPoints[index]);
                bossOrbitManager.EndPoints.Remove(bossOrbitManager.EndPoints[index]);
                bossOrbitManager.ReorderMasksID(MaskID);


                isHooked = false;
                hook.isHooked = false;
                hook.hitDistance = 1;

                bossOrbitManager.BossFov.UpdateViewAngle();

            }

        }
    }


    


    public void BounceMovement(Collider collider) {

        #region Bounce variables
        MovementBase collidingObject = collider.GetComponent<MovementBase>();
        #endregion


        

        Vector3 fakeCollidingObjectPosition = new Vector3(collidingObject.transform.localPosition.x, transform.localPosition.y, collidingObject.transform.localPosition.z);
        Debug.Log(fakeCollidingObjectPosition + "position");
        normal = (fakeCollidingObjectPosition - transform.localPosition).normalized;

        vectorParal = Vector3.Project(VelocityVector, normal);
        vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);


        collisionVectorParal = Vector3.Project(collidingObject.VelocityVector, -normal);
        collisionVectorPerp = Vector3.ProjectOnPlane(collidingObject.VelocityVector, -normal);

        //Bounce formula
        bounceVector = (vectorParal * (Mass - collidingObject.Mass) + 2 * collidingObject.Mass * collisionVectorParal) / (Mass + collidingObject.Mass);
        bounceVector *= 1 - KineticEnergyLoss;

        bounceVector += vectorPerp * (1 - KineticEnergyLoss);
        normal = (transform.localPosition - fakeCollidingObjectPosition).normalized;

        bounceVector = (collisionVectorParal * (collidingObject.Mass - Mass) + 2 * Mass * vectorParal) / (collidingObject.Mass + Mass);
        collidingObject.VelocityVector = (bounceVector * (1 - collidingObject.KineticEnergyLoss)) + collisionVectorPerp * (1 - collidingObject.SurfaceFriction);

        Vector3 fakeMaskPosition = new Vector3(transform.localPosition.x, boss.transform.localPosition.y, transform.localPosition.z);
        normal = fakeMaskPosition - boss.transform.localPosition;

        vectorParal = Vector3.Project(bounceVector, normal);
        vectorPerp = Vector3.ProjectOnPlane(bounceVector, normal);

        boss.VelocityVector += vectorParal;

        AngularVelocity = ((vectorPerp.magnitude * Mathf.Rad2Deg) / currentRadius) * Mathf.Sign(-AngularVelocity /*(Mass * VelocityVector.sqrMagnitude) - (Player.mass * VelocityVector.sqrMagnitude)*/);

        //Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, 0.2f);
        //Debug.DrawRay(collidingObject.transform.position, collidingObject.VelocityVector, Color.green, 0.2f);
        //Debug.DrawRay(transform.position, VelocityVector, Color.blue, 0.2f);

    }



    public void WallBounce(Collider collider) {

        #region Bounce variables
        GameObject collidingObject = collider.gameObject;
        #endregion

        normal = -collidingObject.transform.forward;
        VelocityVector += boss.VelocityVector;

        vectorParal = Vector3.Project(VelocityVector, normal);

        //Bounce formula

        //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
        bounceVector = (-2 * Mass * vectorParal) / (2 * Mass);
        bounceVector *= 1 - KineticEnergyLoss;

        Vector3 fakeMaskPosition = new Vector3(transform.position.x, boss.transform.position.y, transform.position.z);
        normal = fakeMaskPosition - boss.transform.position;

        vectorParal = Vector3.Project(bounceVector, normal);
        vectorPerp = Vector3.ProjectOnPlane(bounceVector, normal);

        //Se c'è possibilità di stun allora:
        boss.VelocityVector = vectorParal;

        //altrimenti:
        //boss.VelocityVector += vectorParal;

        AngularVelocity = ((vectorPerp.magnitude * Mathf.Rad2Deg) / currentRadius) * Mathf.Sign(-AngularVelocity);

        //Debug.DrawRay(boss.transform.position, vectorParal, Color.blue, .016f);
        //Debug.DrawRay(transform.position, vectorPerp, Color.blue, .016f);
        //Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, .016f);
        //Debug.Log("VectorPerp: " + vectorPerp);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.GetComponent<MovementBase>() && !collider.GetComponent<BossController>()) {
            BossOrbitManager.BounceMasks(collider);
            if (collider.GetComponent<PlayerController>()) {
                collider.GetComponent<PlayerController>().animator.SetTrigger("Stunned");
            }

        }

        if (collider.tag == "Wall") {

            Debug.Log("Wall Mask Bounce");
            BossOrbitManager.BounceMasksOnWall(collider);
        }
    }

}
