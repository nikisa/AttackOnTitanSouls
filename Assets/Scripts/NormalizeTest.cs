using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizeTest : MonoBehaviour
{
    public GameObject massPoint1;
    public GameObject massPoint2;


    float springValue = .9f;
    float error = 0;
    Vector3 direction = Vector3.zero;
    Vector3 movement = Vector3.zero;

    void Test() {
        error = (massPoint1.transform.position - massPoint2.transform.position).magnitude - 2;
        direction = ( massPoint2.transform.position - massPoint1.transform.position ).normalized;
        movement = error * direction;
        massPoint1.transform.position -= movement * springValue;
        massPoint2.transform.position += movement * (1 - springValue);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Test();
        }
    }


}
