using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FirstBossMask : HookPointBase { 

    public int MaskID;
    public float Mass;
    public FirstBossController boss;
    public Vector3 OldPos;
    public Vector3 Inertia;
    [Tooltip("Insert BreakPointData")]
    public BreakPointData[] BreakPoints;
    public GameObject[] graphics;
    public ParticleSystem[] particles;


    //Public
    [HideInInspector]
    [Range(0, 1)]
    public float KineticEnergyLoss;
    [HideInInspector]
    [Range(0, 1)]
    public float SurfaceFriction;
    [HideInInspector]
    public float AngularAccelerationModule;
    [HideInInspector]
    public float AngularDecelerationModule;
    [HideInInspector]
    public Vector3 AngularAcceleration;
    [HideInInspector]
    public float AngularVelocity;
    [HideInInspector]
    public Vector3 VelocityVector;
    [HideInInspector]
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
    PositionConstraint positionConstraint;
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
    
    #endregion

    GameObject mask;
    GameObject parent;
    BossOrbitManager bossOrbitManager;
    

    private void Awake() {//da spostare quando ci sarà GameManager
        SetUp();
    }

    private void Start() {
        foreach (var item in boss.animator.GetBehaviours<FirstBossState>()) {
            item.SetContext(boss, this, boss.animator, bossOrbitManager);
        }
    }

    void SetUp() {
        positionConstraint = HookPointPivot.GetComponent<PositionConstraint>();
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
        positionConstraint.enabled = true;
        AngularAccelerationModule = _angularMaxSpeed / _angularAccelerationTime;
        Drag = AngularAccelerationModule / _angularMaxSpeed * Time.deltaTime;
        AngularVelocity -= AngularVelocity * Drag;
        transform.eulerAngles += new Vector3(0, AngularVelocity * Time.deltaTime + 0.5f * AngularAccelerationModule * Mathf.Pow(Time.deltaTime, 2), 0);
        transform.position = new Vector3(boss.transform.position.x + currentRadius * Mathf.Sin((transform.eulerAngles.y) * Mathf.Deg2Rad), 1.375f , boss.transform.position.z + currentRadius * Mathf.Cos((transform.eulerAngles.y) * Mathf.Deg2Rad));
        AngularVelocity += AngularAccelerationModule * Time.deltaTime;
        //VelocityVector = new Vector3((AngularVelocity * Mathf.PI / 180) * currentRadius * Mathf.Sin(transform.eulerAngles.x), 0, (AngularVelocity * Mathf.PI / 180) * currentRadius * Mathf.Cos(transform.eulerAngles.z));
        VelocityVector = transform.right * (AngularVelocity * Mathf.Deg2Rad * currentRadius);
    }

    public void DecelerateAround(float _angularDecelerationModule) {
        if (Mathf.Abs(AngularVelocity) > Mathf.Abs(_angularDecelerationModule) * Time.deltaTime) {
            positionConstraint.enabled = true;
            AngularVelocity -= _angularDecelerationModule * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, AngularVelocity * Time.deltaTime, 0);
            transform.position = new Vector3(boss.transform.position.x + currentRadius * Mathf.Sin((transform.eulerAngles.y) * Mathf.Deg2Rad), 1.375f, boss.transform.position.z + currentRadius * Mathf.Cos((transform.eulerAngles.y) * Mathf.Deg2Rad));
            VelocityVector = new Vector3((AngularVelocity * Mathf.PI / 180) * currentRadius * Mathf.Sin(transform.eulerAngles.x), 0, (AngularVelocity * Mathf.PI / 180) * currentRadius * Mathf.Cos(transform.eulerAngles.z));
        }
        else {
            positionConstraint.enabled = false;
            AngularVelocity = 0;
            VelocityVector = Vector3.zero;
        }
    }

    public void RotationReset() {
        positionConstraint.enabled = false;
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

        if (currentLife <= 0) {

            if (BreakPointsCount < graphics.Length) {

                if (BreakPointsCount == graphics.Length-1 && transform.childCount > 0 && currentLife < 0) {
                    
                    transform.GetChild(0).gameObject.transform.position = hook.transform.position;
                }
                else {
                    Destroy(transform.GetChild(1).gameObject);
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

    //Bunce tra Maschera e Player (o oggetto che non sia un muro)
    //public void BounceMovement(Collider collider, float _kineticEnergyLoss, float _surfaceFriction, float _impulseDeltaTime) {

    //    #region Bounce variables
    //    MovementBase collidingObject = collider.GetComponent<MovementBase>();
    //    #endregion

    //    Vector3 fakeCollidingObjectPosition = new Vector3(collidingObject.transform.localPosition.x, transform.localPosition.y, collidingObject.transform.localPosition.z);
    //    normal = (fakeCollidingObjectPosition - transform.localPosition).normalized;

    //    vectorParal = Vector3.Project(VelocityVector, normal);
    //    vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);


    //    collisionVectorParal = Vector3.Project(collidingObject.VelocityVector, -normal);
    //    collisionVectorPerp = Vector3.ProjectOnPlane(collidingObject.VelocityVector, -normal);

    //    //Bounce formula
    //    boss.BounceVector = (vectorParal * (Mass - collidingObject.Mass) + 2 * collidingObject.Mass * collisionVectorParal) / (Mass + collidingObject.Mass);
    //    boss.BounceVector *= 1 - KineticEnergyLoss;

    //    boss.BounceVector += vectorPerp * (1 - KineticEnergyLoss);
    //    normal = (transform.localPosition - fakeCollidingObjectPosition).normalized;

    //    boss.BounceVector = (collisionVectorParal * (collidingObject.Mass - Mass) + 2 * Mass * vectorParal) / (collidingObject.Mass + Mass);
    //    collidingObject.VelocityVector = (boss.BounceVector * (1 - collidingObject.KineticEnergyLoss)) + collisionVectorPerp * (1 - collidingObject.SurfaceFriction);

    //    Vector3 fakeMaskPosition = new Vector3(transform.localPosition.x, boss.transform.localPosition.y, transform.localPosition.z);
    //    normal = fakeMaskPosition - boss.transform.localPosition;

    //    vectorParal = Vector3.Project(boss.BounceVector, normal);
    //    vectorPerp = Vector3.ProjectOnPlane(boss.BounceVector, normal);

    //    boss.VelocityVector += vectorParal;

    //    //AngularVelocity = ((vectorPerp.magnitude * Mathf.Rad2Deg) / currentRadius) * Mathf.Sign(-AngularVelocity /*(Mass * VelocityVector.sqrMagnitude) - (Player.mass * VelocityVector.sqrMagnitude)*/);

    //    //Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, 0.2f);
    //    //Debug.DrawRay(collidingObject.transform.position, collidingObject.VelocityVector, Color.green, 0.2f);
    //    //Debug.DrawRay(transform.position, VelocityVector, Color.blue, 0.2f);

    //}


    //OLD BOUNCE FORMULA
    //public void WallBounce(Collider collider) {

    //    #region Bounce variables
    //    GameObject collidingObject = collider.gameObject;
    //    #endregion

    //    #region OldBounce
    //    normal = -collidingObject.transform.forward;
    //    VelocityVector += boss.VelocityVector;

    //    vectorParal = Vector3.Project(VelocityVector, normal);

    //    //Bounce formula

    //    //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
    //    boss.BounceVector = (-2 * Mass * vectorParal) / (2 * Mass);
    //    boss.BounceVector *= 1 - KineticEnergyLoss;

    //    Vector3 fakeMaskPosition = new Vector3(transform.position.x, boss.transform.position.y, transform.position.z);
    //    normal = fakeMaskPosition - boss.transform.position;

    //    vectorParal = Vector3.Project(boss.BounceVector, normal);
    //    vectorPerp = Vector3.ProjectOnPlane(boss.BounceVector, normal);

    //    //Se c'è possibilità di stun allora:
    //    boss.VelocityVector = vectorParal;

    //    //altrimenti:
    //    //boss.VelocityVector += vectorParal;

    //    AngularVelocity = ((vectorPerp.magnitude * Mathf.Rad2Deg) / currentRadius) * Mathf.Sign(-AngularVelocity);

    //    //Debug.DrawRay(boss.transform.position, vectorParal, Color.blue, .016f);
    //    //Debug.DrawRay(transform.position, vectorPerp, Color.blue, .016f);
    //    //Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.red, .016f);
    //    //Debug.Log("VectorPerp: " + vectorPerp);
    //    #endregion

    //}

    //private void OnTriggerEnter(Collider collider) {

    //    boss.CollidedObjectCollider = collider;

    //    if (collider.GetComponent<MovementBase>() && !collider.GetComponent<BossController>()) {
    //        //BossOrbitManager.BounceMasks(collider);
    //        if (collider.GetComponent<PlayerController>()) {
    //            collider.GetComponent<PlayerController>().animator.SetTrigger("Stunned");
    //            boss.animator.SetInteger("Layer", 11);
    //            bossOrbitManager.ObjHit = 2;
    //        }
            
    //    }


    //    if (collider.tag == "Walls" && (Time.time - boss.timerMaskCollision) > boss.ActiveMaskCollisionTime) {
    //        boss.animator.SetInteger("Layer", 10);
    //        bossOrbitManager.hitMaskIndex = MaskID;
    //        bossOrbitManager.ObjHit = 1;
    //    }

    //}


    //Bounce tra Boss e Wall
    //public void MaskBounceWall(Collider collider , float _kineticEnergyLoss , float _surfaceFriction , float _impulseDeltaTime) {

    //    Debug.Log("MaskBounceWall");

    //    #region NewBounce 

    //        float plusAngle;

    //        normal = -collider.transform.forward;

    //        vectorParal = Vector3.Project(VelocityVector, normal);
    //        vectorPerp = Vector3.ProjectOnPlane(VelocityVector, normal);

    //        //Debug.DrawRay(transform.position, vectorParal, Color.red, 5);
    //        //Debug.DrawRay(transform.position, vectorPerp, Color.cyan, 5);

    //        //Per il muro non serve andare a vedere la sua massa , ma basta dare la stessa massa dell'oggetto che urta
    //        boss.BounceVector = (-2 * Mass * (vectorParal) / (2 * Mass));

    //        plusAngle = AngularVelocity * _impulseDeltaTime;

    //        boss.transform.RotateAround(transform.position, Vector3.up, plusAngle);

    //        Vector3 bossVectorParal = Vector3.Project(boss.VelocityVector, normal);
    //        Vector3 bossVectorPerp = Vector3.ProjectOnPlane(boss.VelocityVector, normal);

    //        Vector3 newBounceVector = (-2 * Mass * (bossVectorParal) / (2 * Mass));

    //        Debug.DrawRay(boss.transform.position, newBounceVector, Color.green, 1f);
    //        Debug.DrawRay(boss.transform.position, boss.BounceVector, Color.black, .03f);
    //        Debug.DrawRay(boss.transform.position, bossVectorPerp, Color.cyan, 1f);

    //        boss.VelocityVector = (newBounceVector + bossVectorPerp) * (1 - _kineticEnergyLoss);
    //        boss.VelocityVector = Quaternion.AngleAxis(plusAngle, Vector3.up) * boss.VelocityVector;
    //        boss.AccelerationVector = boss.VelocityVector.normalized * boss.AccelerationVector.magnitude;
    //        boss.BounceVector *= (1 - _kineticEnergyLoss); /** Mass / boss.Mass;*/
    //        boss.BounceVector = Mathf.Clamp(boss.BounceVector.magnitude, boss.minBounceVector, boss.maxBounceVector) * boss.BounceVector.normalized;

    //        //Debug.DrawRay(boss.transform.position, vectorParal, Color.blue, .16f);

    //        //Debug.DrawRay(boss.transform.position, boss.AccelerationVector, Color.red, .03f);
    //        //Debug.DrawRay(boss.transform.position, boss.VelocityVector, Color.blue, .03f);

    //}

    public void UpdateAngularVelocity(float _surfaceFriction) {
        AngularVelocity *= -(1 - _surfaceFriction);
    }

    //#endregion
}
