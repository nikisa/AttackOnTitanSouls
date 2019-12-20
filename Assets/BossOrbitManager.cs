using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrbitManager : MonoBehaviour
{
    //Inspector
    public List<GameObject> OrbitList;
    public List<HookPoint> HookPointList;

    //Private
    bool hasFinished;
    float timeAcceleration;
    float moveSpeed;

    void Start()
    {
        hasFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public void SetInitial(float _initialRadius , int _index , OrbitData _data)
    //{
    //    Vector3 Temp = OrbitList[_index].transform.position;
    //    _data.initialPosition = Temp.z + _initialRadius;
    //    OrbitList[_index].transform.position = new Vector3(Temp.x, Temp.y, _data.initialPosition);
       
    //}

    public void MoveRadius(float _finalRadius , int _index , float _initialPosition , bool _hasPingPong , float _speedRadius)
    {
        //Debug.Log(OrbitList[_index].gameObject.transform.parent.name);
        //Debug.Log(_initialPosition + _finalRadius);
        if (_initialPosition + _finalRadius >= OrbitList[_index].transform.localPosition.z && !hasFinished)
        {
             OrbitList[_index].transform.Translate(Vector3.forward* _speedRadius * Time.deltaTime);
        }
        if((_initialPosition + _finalRadius <= OrbitList[_index].transform.localPosition.z  || _initialPosition + _finalRadius >= OrbitList[_index].transform.localPosition.z) && _hasPingPong)
        {
            OrbitList[_index].transform.Translate(-OrbitList[_index].transform.forward * _speedRadius * Time.deltaTime);
            hasFinished = true;
        }
        if (OrbitList[_index].transform.localPosition.z <= _initialPosition) {
            hasFinished = false;
        }

    }
    public void RotationMove(float _maxSpeed , float _timeAcceleration)
    {
        timeAcceleration = _maxSpeed / _timeAcceleration;
        moveSpeed += timeAcceleration * Time.deltaTime;

        transform.Rotate(Vector3.up * moveSpeed);
        moveSpeed = Mathf.Clamp(moveSpeed, 0, _maxSpeed);
    }
    public void SetAllInitialPosition(int _index, OrbitData _data)
    {
        _data.initialPosition = OrbitList[_index].transform.localPosition.z;
        Debug.Log(OrbitList[_index].transform.localPosition.z);
    }
    public void SetHookPoints()
    {
        for (int i = 0; i < OrbitList.Count; i++)
        {
            HookPointList[i]= OrbitList[i].transform.GetChild(1).GetComponent<HookPoint>();
        
        }
    } 
    //public void Controllo()
    //{
    //    for (int i = 0; i < OrbitList.Count; i++)
    //    {
    //        HookPointList[i].

    //    }
    //}
}
