using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPoint : MonoBehaviour/*HookPointManager , IGrappable*/
{
    
    public enum HookPointType {
        StaticHookPoint,
        BreakableHookPoint
    };  

    //Inspector
    public bool Hooked;
    public Vector3 OldPos;
    public Vector3 Inertia;



    [Tooltip("Insert BreakPointData")]
    public BreakPointData[] BreakPoints;


    //private
    private int BreakPointsCount;
    private float springVector;
    private float currentLife;
    private float currentElasticK;

    
    void SetUp() {
        currentLife = BreakPoints[BreakPointsCount].lifeMax;
    }



    //public void hookPointSpring() {
    //    float lifeRatio;
    //    BreakPointData breakPointData = BreakPoints[BreakPointsCount];

    //    lifeRatio = Mathf.Clamp01(currentLife / breakPointData.lifeMax);
    //    currentElasticK = breakPointData.InitialElasticK - (1 - (breakPointData.InitialElasticK - breakPointData.FinalElasticK) * lifeRatio);
    //    springVector = currentElasticK * Vector3.Distance(HookPointPivot.position.normalized , HookPointAsset.transform.position.normalized);

    //    if (springVector >= getPullingVector(this.HookPointPivot , this.HookPointAsset)) {

    //        currentLife -= player.DPS * Time.deltaTime;

    //        if (currentLife <= 0) {
    //            if (BreakPointsCount < BreakPoints.Length) {
    //                BreakPointsCount++;
    //                //Set new BreakPointAsset
    //            }
    //            else {
    //                //boss is dead
    //            }

                

    //        }
    //    }
        
    //}

    

   
    
}
