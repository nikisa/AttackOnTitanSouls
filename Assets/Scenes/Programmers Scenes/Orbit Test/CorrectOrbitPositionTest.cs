using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectOrbitPositionTest : MonoBehaviour
{

    public int MasksCount;

    public float raycastDistance;
    public LayerMask layerMask;

    //Private 
    RaycastHit hit;
    Vector3 angle;

    private void Start() {
        
    }

    private void Update() {
        checkFrontalPosition(MasksCount);
    }


    void checkFrontalPosition(int _masksCount) {
        Debug.DrawRay(transform.position , transform.forward + transform.eulerAngles , Color.red);
        for (int i = 0; i < _masksCount; i++) {
            transform.eulerAngles += new Vector3(360 / MasksCount, 0, 0);
            if (Physics.Raycast(transform.position, transform.forward + transform.eulerAngles , out hit, raycastDistance, layerMask)) {
                Debug.Log("STOP " + i);
            }
        }
        
        
        

    }

}
