using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class test : MonoBehaviour
{
    public List<GameObject> list;
    public float initialRadius;
    public float finalRadius;
    public float time;

    
    void Start()
    {
        for (int i = 0; i < list.Count; i++) {
            list[i].transform.DOMove(list[i].transform.forward * finalRadius , time).SetEase(Ease.Linear);
            //list[i].transform.Translate((list[i].transform.position + transform.forward) * finalRadius * Time.deltaTime);
        }
    }

    
}
