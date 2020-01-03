using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizeTest : MonoBehaviour {
    public GameObject massPoint1;
    public GameObject massPoint2;

    float springValue = .9f;
    float error = 0;
    float angle = 0;
    float testAngle = 0;
    Vector3 direction = Vector3.zero;
    Vector3 movement = Vector3.zero;

    void Test() {

        angle = Mathf.Atan2(massPoint1.transform.position.x - massPoint2.transform.position.x, massPoint1.transform.position.z - massPoint2.transform.position.z) * 180 / Mathf.PI;
        Debug.LogFormat("Angle: {0} -->- Cos = {1}  --> Sin = {2}", angle, Mathf.Cos(angle) , Mathf.Sin(angle));

        testAngle = Mathf.Atan2(massPoint2.transform.position.x - massPoint1.transform.position.x, massPoint2.transform.position.z - massPoint1.transform.position.z) * 180 / Mathf.PI;
        Debug.LogFormat("TestAngle: {0}", testAngle);

        error = (massPoint1.transform.position - massPoint2.transform.position).magnitude - 2;

        if ((massPoint1.transform.position - massPoint2.transform.position).magnitude > 2) {
            direction = (massPoint1.transform.position - massPoint2.transform.position).normalized;
        }
        else if ((massPoint1.transform.position - massPoint2.transform.position).magnitude < 2) {
            direction = (massPoint2.transform.position - massPoint1.transform.position).normalized;
        }

        movement = error * direction;
        massPoint1.transform.position -= movement * springValue;
        massPoint2.transform.position += movement * (1 - springValue);
    }
    

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            Test();

            Debug.LogFormat("distance1 {0} " , Vector3.Distance(massPoint1.transform.position, massPoint2.transform.position));
            Debug.LogFormat("magnitude {0} ", (massPoint2.transform.position - massPoint1.transform.position).magnitude);

        }
    }
}
