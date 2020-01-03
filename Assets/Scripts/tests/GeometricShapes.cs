using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GeometricShapes : MonoBehaviour
{
    public List<GameObject> endPoints;
    public float distanceFromCenter;

    void Start() {
        setObjectsPosition();    
    }

    public void setObjectsPosition() {
        float orientation = 360;
        for (int i = 0; i < endPoints.Count; i++) {
            endPoints[i].transform.eulerAngles = new Vector3(endPoints[i].transform.eulerAngles.x , orientation , endPoints[i].transform.eulerAngles.z);
            endPoints[i].transform.DOMove(endPoints[i].transform.forward * distanceFromCenter , .1f);
            orientation -= 360 / endPoints.Count;
        }
    }

}
