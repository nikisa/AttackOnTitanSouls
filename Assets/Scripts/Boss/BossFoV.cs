using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossFoV : MonoBehaviour
{

    //SOSTITUIRE transform.position CON Player.transform.position

    //Inspector
    //[HideInInspector]
    public float ViewRadius;
    //[HideInInspector]
    [Range(0, 360)]
    public float ViewAngle;
    public LayerMask TargetMask;
    public LayerMask ObstacleMask;
    public BossOrbitManager BossOrbitManager;


    //Public
    [HideInInspector]
    public List<Transform> VisibleTargets = new List<Transform>();

    //Private
    float time;
    //For Editor
    float lastViewAngle;
    Vector3 initialPosition;

    private void Start() {

        UpdateViewRadius();
        UpdateViewAngle();

        time = 0.016f;
        initialPosition = transform.position;
        StartCoroutine("FindTargetsWithDelay", time);
        if (ViewAngle <= 60 && ViewAngle != lastViewAngle) {
            transform.position -= transform.forward * (3 - (ViewAngle / 100));
        }
        lastViewAngle = ViewAngle;
    }




    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    public void UpdateViewRadius() {
        ViewRadius = BossOrbitManager.GetMaxMasksDistance();
        
    }

    public void UpdateViewAngle() {
        ViewAngle = 360 / BossOrbitManager.MasksList.Count;
    }




    void FindVisibleTargets() {

        VisibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 1.5f) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, ObstacleMask)) {
                    VisibleTargets.Add(target);
                }
            }
        }
    }



    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

    }


    public float GetMinDistance(List<Transform> _visibleTargets) {

        Transform[] visibleTargets = _visibleTargets.ToArray();
        float[] distances = new float[visibleTargets.Length];
        float distance;


        for (int i = 0; i < visibleTargets.Length; i++) {
            distance = Vector3.Distance(transform.position, visibleTargets[i].position);
            distances[i] = distance;
        }

        return distance = distances.Min();

    }




    public float GetCorrectTarget(List<Transform> _visibleTargets) {
        float[] priority = new float[_visibleTargets.Count]; ;
        Transform[] visibleTargets = _visibleTargets.ToArray();

        float distance;
        float distanceAngle;

        for (int i = 0; i < visibleTargets.Length; i++) {
            distance = Vector3.Distance(transform.position, visibleTargets[i].position);
            distanceAngle = Vector3.Angle(transform.position, visibleTargets[i].position) * Mathf.Rad2Deg;
            priority[i] = distanceAngle * Mathf.Pow(distance, 2);
        }

        return priority.Min();

    }
}
