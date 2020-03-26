﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BossFoV))]
public class BossFoVEditor : Editor
{

    private void OnSceneGUI() {
        BossFoV fov = (BossFoV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.ViewRadius);

        Vector3 viewAngleA = fov.DirFromAngle(-fov.ViewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.ViewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewRadius);

        Handles.color = Color.red;

        foreach (Transform visibleTarget in fov.VisibleTargets) {

            float distance = Vector3.Distance(fov.transform.position, visibleTarget.position);
            float distanceAngle = Vector3.Angle(fov.transform.position, visibleTarget.position) * Mathf.Rad2Deg;
            float priority = distanceAngle * Mathf.Pow(distance, 2);

            if (priority <= fov.GetCorrectTarget(fov.VisibleTargets)) {
                Handles.DrawLine(fov.transform.position, visibleTarget.position);
            }

        }
    }
}