using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinalTest : MonoBehaviour
{
    //Public
    public List<GameObject> endPoints;
    public List<GameObject> InitialPoints;
    public List<GameObject> Masks;
    public float InitialRadius;
    public float FinalRadius;
    public GameObject startPosition;

    //Private
    int totalEndPoints;

    private void Start() {
        totalEndPoints = endPoints.Count;
        setObjectsPosition();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.B)) {
            SetMasks();
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            MoveMasks();
        }

            if (totalEndPoints != endPoints.Count) {
            totalEndPoints = endPoints.Count;
            ResetPosition();
            setObjectsPosition();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            //Destroy finalRadius pos
            Destroy(endPoints[totalEndPoints - 1]);
            endPoints.RemoveAt(totalEndPoints-1);
            //Destroy InitialRadius pos
            Destroy(InitialPoints[totalEndPoints - 1]);
            InitialPoints.RemoveAt(totalEndPoints - 1);
            //Destroy Mask
            Destroy(Masks[totalEndPoints - 1]);
            Masks.RemoveAt(totalEndPoints - 1);
            
        }
    }

    public void setObjectsPosition() {
        float orientation = 360;
        for (int i = 0; i < endPoints.Count; i++) {

            InitialPoints[i].transform.eulerAngles = new Vector3(InitialPoints[i].transform.eulerAngles.x, orientation, InitialPoints[i].transform.eulerAngles.z);
            InitialPoints[i].transform.DOMove(InitialPoints[i].transform.forward * InitialRadius, .1f);

            endPoints[i].transform.eulerAngles = new Vector3(endPoints[i].transform.eulerAngles.x, orientation, endPoints[i].transform.eulerAngles.z);
            endPoints[i].transform.DOMove(endPoints[i].transform.forward * FinalRadius, .1f);

            orientation -= 360 / endPoints.Count;
        }
    }

    public void ResetPosition() {
        for (int i = 0; i < endPoints.Count; i++) {
            endPoints[i].transform.position = startPosition.transform.position;
            InitialPoints[i].transform.position = startPosition.transform.position;
        }
    }

    public void SetMasks() {
        for (int i = 0; i < Masks.Count; i++) {
            Masks[i].transform.DOMove(InitialPoints[i].transform.position , 0.5f);
        }
    }

    public void MoveMasks() {
        for (int i = 0; i < Masks.Count; i++) {
            Masks[i].transform.DOMove(endPoints[i].transform.position, 0.5f);
        }
    }

}
