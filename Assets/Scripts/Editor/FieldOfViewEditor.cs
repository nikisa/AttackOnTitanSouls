using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{

    private void OnSceneGUI() {
       FieldOfView fov = (FieldOfView) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position , Vector3.up , Vector3.forward , 360 , fov.viewRadius+ fov.RadiusOffset);

        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position , fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position , fov.transform.position + viewAngleB * fov.viewRadius);

        Handles.color = Color.red;

        foreach (Transform visibleTarget in fov.visibleTargets) {

            float distance = Vector3.Distance(fov.transform.position ,visibleTarget.position);
            float distanceAngle = Vector3.Angle(fov.transform.position , visibleTarget.position) * Mathf.Rad2Deg;
            float priority = distanceAngle * Mathf.Pow(distance, 2);

            if (priority <= fov.GetCorrectTarget(fov.visibleTargets)) {
                Handles.DrawLine(fov.transform.position, visibleTarget.position);
            }
            
            //Distance is the only priority
            //if (distance /*+ offset*/ <= fov.GetMinDistance(fov.visibleTargets)) {
            //    Handles.DrawLine(fov.transform.position, visibleTarget.position);
            //}
        }
    }
}
