using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPoint : HookPointManager , IGrappable
{
    
    public enum HookPointType {
        StaticHookPoint,
        BreakableHookPoint
    };  

    //Inspector
    public bool isHooked;
    public Vector3 OldPos;
    public Vector3 Inertia;



    [Tooltip("Insert BreakPointData")]
    public BreakPointData[] BreakPoints;
    public GameObject[] graphics;
    public ParticleSystem[] particles;



    //private
    BreakPointData breakPointData;
    [SerializeField]
    private int BreakPointsCount = 0;
    private float springVector;
    [SerializeField]
    private float currentLife;
    private float currentElasticK;
    private float distance;
    Vector3 direction = Vector3.zero;
    Vector3 movement = Vector3.zero;


    private void Awake() {//da spostare quando ci sarà GameManager
        SetUp();
    }

    void SetUp() {
        currentLife = BreakPoints[BreakPointsCount].lifeMax;
        OldPos = transform.position;
        GameObject mask = Instantiate(graphics[0] , transform.position - new Vector3(0, 1.375f,0) , transform.rotation);
        mask.transform.SetParent(transform);
    }


    public void hookPointSpring() {
        float lifeRatio;

        if (BreakPointsCount < BreakPoints.Length) {
            breakPointData = BreakPoints[BreakPointsCount];
        }
        
        lifeRatio = Mathf.Clamp01(currentLife / breakPointData.lifeMax);

        transform.position = hook.transform.position;
        Inertia = transform.position - OldPos;
        distance = Vector3.Distance(HookPointPivot.position, transform.position);
        springVector = currentElasticK * distance;
        currentElasticK = Mathf.Lerp(breakPointData.FinalElasticK, breakPointData.InitialElasticK, lifeRatio);

        direction = (hook.transform.position - HookPointPivot.position).normalized;
        movement = direction * springVector / 60;
        transform.position -= movement;
        hook.transform.position = transform.position;
        OldPos = transform.position;
        
        if (springVector >= Inertia.magnitude && distance > .2f) {
            currentLife -= player.DPS / 60;
            Debug.Log(currentLife);
        }

        if (currentLife < 0) {
            
            if (BreakPointsCount < graphics.Length) {
                Destroy(transform.GetChild(0).gameObject);
                BreakPointsCount++;
                currentLife = BreakPoints[BreakPointsCount].lifeMax;
                GameObject mask = Instantiate(graphics[BreakPointsCount].gameObject , transform.position - new Vector3(0, 1.375f, 0), transform.rotation);
                mask.transform.SetParent(transform);
                ParticleSystem particle = Instantiate(particles[BreakPointsCount-1] as ParticleSystem, transform.position, Quaternion.identity);
                if (BreakPointsCount == 2) {
                    particle = Instantiate(particles[BreakPointsCount] as ParticleSystem, transform.position, Quaternion.identity);
                }
            }
            
            if (BreakPointsCount == graphics.Length && transform.childCount > 0 && currentLife < 0) {
                Destroy(transform.GetChild(0).gameObject);
            }

            isHooked = false;
            hook.isHooked = false;
            hook.hitDistance = 1;

            if (BreakPointsCount == graphics.Length && currentLife < 0) {
                Destroy(this.gameObject);
            }

        }
    }
}
