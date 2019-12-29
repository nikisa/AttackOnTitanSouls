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
    

    void Start()
    {
        hasFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetInitial(float _initialRadius, int _index, OrbitData _data) {
        OrbitList[_index].transform.localPosition = new Vector3(OrbitList[_index].transform.position.x, OrbitList[_index].transform.position.y, 0);
        Vector3 Temp = OrbitList[_index].transform.localPosition;
        _data.initialPosition = Temp.z + _initialRadius;
        OrbitList[_index].transform.localPosition = new Vector3(Temp.x, Temp.y, _data.initialPosition);
        Debug.Log("ci sonio");

    }

    public void MoveRadius(float _finalRadius , int _index , float _initialPosition , bool _hasPingPong , float _speedRadius)
    {
        //Debug.Log(OrbitList[_index].gameObject.transform.parent.name);
        //Debug.Log(_initialPosition + _finalRadius);
        if (Mathf.Abs(_initialPosition) + _finalRadius >= Mathf.Abs(OrbitList[_index].transform.localPosition.z))
        {

            OrbitList[_index].transform.Translate(Vector3.forward* _speedRadius * Time.deltaTime);
        }






        //if((_initialPosition + _finalRadius <= OrbitList[_index].transform.localPosition.z  || _initialPosition + _finalRadius >= OrbitList[_index].transform.localPosition.z) && _hasPingPong)
        //{
        //    OrbitList[_index].transform.Translate(-OrbitList[_index].transform.forward * _speedRadius * Time.deltaTime);
        //    hasFinished = true;
        //}
        //if (OrbitList[_index].transform.localPosition.z <= _initialPosition) {
        //    hasFinished = false;
        //}

    }
    public void RotationMove(float _maxSpeed , float _timeAcceleration , HookPointController _centerPoint)
    {


        if (_maxSpeed >= 0 )
        {

        timeAcceleration = _maxSpeed / _timeAcceleration;
        _centerPoint.MoveSpeed += timeAcceleration * Time.deltaTime;
        _centerPoint.transform.Rotate(Vector3.up * _centerPoint.MoveSpeed);
        _centerPoint.MoveSpeed = Mathf.Clamp(_centerPoint.MoveSpeed, 0, _maxSpeed);
        
        }
        else
        {
            _maxSpeed = Mathf.Abs(_maxSpeed);
            timeAcceleration = _maxSpeed / _timeAcceleration;
            _centerPoint.MoveSpeed += timeAcceleration * Time.deltaTime;
            _centerPoint.transform.Rotate(Vector3.down * _centerPoint.MoveSpeed);
            _centerPoint.MoveSpeed = Mathf.Clamp(_centerPoint.MoveSpeed, 0, _maxSpeed);

        }
        

    }

    public void SetAllInitialPosition(int _index, OrbitData _data)
    {
        _data.initialPosition = OrbitList[_index].transform.localPosition.z;
       
    }
    public void SetHookPoints()
    {
        for (int i = 0; i < OrbitList.Count; i++)
        {
            HookPointList[i]= OrbitList[i].transform.GetChild(1).GetComponent<HookPoint>();
        
        }
    }
    
    public void SetUp() {

        timeAcceleration = 0;
    }

    //public void Controllo()
    //{
    //    for (int i = 0; i < OrbitList.Count; i++)
    //    {
    //        HookPointList[i].

    //    }
    //}
}
