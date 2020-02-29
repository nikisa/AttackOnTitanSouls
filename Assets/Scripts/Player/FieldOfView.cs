using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FieldOfView : MonoBehaviour
{

    //SOSTITUIRE transform.position CON Player.transform.position

    //Inspector
    public float RadiusOffset;
    [HideInInspector]
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    


    //Public
    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    //Private
    float time;
    //For Editor
    float lastViewAngle;
    Vector3 initialPosition;

    private void Start() {
        time = 0.016f;
        initialPosition = transform.position;
        StartCoroutine("FindTargetsWithDelay", time);
        if (viewAngle <= 60 && viewAngle != lastViewAngle) {
                transform.position -= transform.forward * (3 - (viewAngle / 100));
        }
        lastViewAngle = viewAngle;
    }




    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }


    void FindVisibleTargets() {
        
        visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius + RadiusOffset, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward , dirToTarget) < viewAngle / 1.5f) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position , dirToTarget , dstToTarget , obstacleMask)) {
                    visibleTargets.Add(target);
                }
            }
        }
    }
    public GameObject CorrectRayCast()
    {
        RaycastHit hit = new RaycastHit();
        GameObject gameObject = new GameObject();

        foreach (Transform visibleTarget in visibleTargets)
        {

            float distance = Vector3.Distance(transform.position, visibleTarget.position);
            float distanceAngle = Vector3.Angle(transform.position, visibleTarget.position) * Mathf.Rad2Deg;
            float priority = distanceAngle * Mathf.Pow(distance, 2);

            if (priority <= GetCorrectTarget(visibleTargets))
            {
                if (Physics.Raycast(transform.position, visibleTarget.position - transform.position, out hit, Mathf.Infinity, targetMask))
                {
                    return hit.collider.gameObject;
                }


            }
          
        }
        return gameObject;
    }


    public Vector3 DirFromAngle(float angleInDegrees , bool angleIsGlobal) {
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
        float []priority = new float[_visibleTargets.Count]; ;
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
