using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    public float MassPointNumber = 32;
    public float MassPointLength = 16;


    public void InstantiateRope() {
        for (int i = 0; i < MassPointNumber; i++) {
            //todo

        }
    }

    public void UpdateHook() {
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float error = 0;
        


    }

    public void UpdatePoints() {
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float error = 0;
        float previousInstanceX = 0;
        float previousInstanceY = 0;


    }

    public void UpdateLinks() {
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float error = 0;
        float previousInstanceX = 0;
        float previousInstanceY = 0;
    }

    public void RewindPoint() {
        int pointsRewinded = 0;
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float previousInstanceX = 0;
        float previousInstanceY = 0;
        float error = 0;
        float SumOfLength = 0;
        float pointsDisatnce = 0;
    }

    public void UpdateRewind() {
        float springValue = 1;
        float DynamicFriction = 0.5f;
        float pointsDistance = 0;
        float previousInstanceX = 0;
        float previousInstanceY = 0;
        float error = 0;
        float SumOfLength = 0;
        float pointsDisatnce = 0;
    }



    public void DisableMassPoints() {

    }


}
