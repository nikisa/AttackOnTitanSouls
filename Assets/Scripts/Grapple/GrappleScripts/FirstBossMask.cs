using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMask : HookPointBase
{

    //public enum HookPointType {
    //    StaticHookPoint,
    //    BreakableHookPoint
    //};  


    public int MaskID;
    public BossController boss;
    public Vector3 OldPos;
    public Vector3 Inertia;
    [Tooltip("Insert BreakPointData")]
    public BreakPointData[] BreakPoints;
    public GameObject[] graphics;
    public ParticleSystem[] particles;

    //Public
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
    public float DecelerationModule;
    [HideInInspector]
    public Vector3 DecelerationVector;
    [HideInInspector]
    public Vector3 targetDir;
    [HideInInspector]
    public float currentRadius;
    //[HideInInspector]
    public float distanceFromBoss;

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
        VelocityVector = new Vector3((AngularVelocity * Mathf.PI - 180) * currentRadius * Mathf.Sin(transform.eulerAngles.x), 0, (AngularVelocity * Mathf.PI - 180) * currentRadius * Mathf.Cos(transform.eulerAngles.z));

    }

    public void DecelerateAround(float _angularDecelerationModule) {
        if (Mathf.Abs(AngularVelocity) > Mathf.Abs(_angularDecelerationModule) * Time.deltaTime) {
            AngularVelocity -= _angularDecelerationModule * Time.deltaTime;
            parent.transform.eulerAngles += new Vector3(0, AngularVelocity * Time.deltaTime, 0);
            parent.transform.position = new Vector3(boss.transform.position.x + currentRadius * Mathf.Sin((parent.transform.eulerAngles.y) * Mathf.Deg2Rad), 0, boss.transform.position.z + currentRadius * Mathf.Cos((parent.transform.eulerAngles.y) * Mathf.Deg2Rad));
            VelocityVector = new Vector3((AngularVelocity * Mathf.PI - 180) * currentRadius * Mathf.Sin(parent.transform.eulerAngles.x), 0, (AngularVelocity * Mathf.PI - 180) * currentRadius * Mathf.Cos(parent.transform.eulerAngles.z));
        }
        else {
            AngularVelocity = 0;
            VelocityVector = Vector3.zero;
        }
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

                isHooked = false;
                hook.isHooked = false;
                hook.hitDistance = 1;

                bossOrbitManager.BossFov.UpdateViewAngle();

            }

        }
    }
}
