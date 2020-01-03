using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveSpheres : MonoBehaviour
{
    public List<GameObject> spheres;
    public List<GameObject> endPosition;
    public GameObject start;
    public float time;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            JingleBalls();
        }
        else if (Input.GetKeyDown(KeyCode.B)) {
            backToTheKitchen();
        }
        
    }

    void JingleBalls() {
        for (int i = 0; i < spheres.Count; i++) {
            spheres[i].transform.DOMove(endPosition[i].transform.position, time);
        }
    }

    void backToTheKitchen() {
        for (int i = 0; i < spheres.Count; i++) {
            spheres[i].transform.DOMove(start.transform.position, time);
        }
    }
}
