using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPointManager : MonoBehaviour
{
    public Transform HookPointPivot;
    public Transform HookPointAsset;

    public Hook hook;
    public PlayerController player;


    protected float getPullingVector(Transform _HPPivot , Transform HPAsset) {

        //distance non tiene conto della oldPos
        return Vector3.Distance(_HPPivot.position.normalized , HPAsset.position.normalized);
    }

}
