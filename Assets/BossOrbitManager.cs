using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossOrbitManager : MonoBehaviour
{   
    //Inspector
    public List<GameObject> OrbitList;
    public List<HookPoint> HookPointList;

    //Public
    //[HideInInspector]
    public List<GameObject> EndPoints;
    //[HideInInspector]
    public List<GameObject> InitialPoints;

    //Private
    bool hasFinished;
    float timeAcceleration;


    void Start()
    {
        hasFinished = false;
    }

    public void SetInitial(float _initialRadius, int _index, OrbitData _data) {
        //OrbitList[_index].transform.localPosition = new Vector3(OrbitList[_index].transform.localPosition.x, OrbitList[_index].transform.localPosition.y, 0);
        //Vector3 Temp = OrbitList[_index].transform.localPosition;
        //_data.initialPosition = Temp.z + _initialRadius;
        //OrbitList[_index].transform.localPosition = new Vector3(Temp.x, Temp.y, _data.initialPosition);

        if (_data.isSetup) {
            if (Mathf.Abs(_initialRadius) >= Mathf.Abs(OrbitList[_index].transform.localPosition.z)) {
                OrbitList[_index].transform.Translate(Vector3.forward * 50 * Time.deltaTime);
            }
        }
    }

    public void MoveRadius(float _finalRadius , int _index , float _initialPosition , float _time , Ease _ease)
    {
        Vector3 temp;

        //if (OrbitList[_index].transform.localPosition.x > 0) {
        //    if (OrbitList[_index].transform.localPosition.z > 0) {
        //        temp = new Vector3((_finalRadius - _initialPosition), OrbitList[_index].transform.localPosition.y, (_finalRadius - _initialPosition));
        //    }
        //    else {
        //        temp = new Vector3((_finalRadius - _initialPosition), OrbitList[_index].transform.localPosition.y, (0 - _finalRadius + _initialPosition));
        //    }

        //}
        //else {
        //    if (OrbitList[_index].transform.localPosition.z > 0) {
        //        temp = new Vector3((0 - _finalRadius + _initialPosition), OrbitList[_index].transform.localPosition.y, (_finalRadius - _initialPosition));
        //    }
        //    else {
        //        temp = new Vector3((0 - _finalRadius + _initialPosition), OrbitList[_index].transform.localPosition.y, (0 - _finalRadius + _initialPosition));
        //    }
        //}

        OrbitList[_index].transform.DOMove(OrbitList[_index].transform.forward * _finalRadius, _time).SetRelative(true).SetEase(_ease);

        Debug.Log("temp " + (_finalRadius - _initialPosition).ToString());
        Debug.Log("final " + _finalRadius);
        Debug.Log("initial " + _initialPosition);

    }


    public void RotationMove(float _maxSpeed , float _timeAcceleration , HookPointController _centerPoint)
    {

        //_maxSpeed /= 60;
        //_timeAcceleration /= 60;

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


    public void SetMasks(List <OrbitManagerData> _orbitManagerList) {
        int orbitCount = 0;
        for (int i = 0; i < _orbitManagerList.Count ; i++) {
            for (int y = 0; y < _orbitManagerList[i].orbitData.Count; y++) {
                Debug.Log("OrbitCount: " + orbitCount);
                this.OrbitList[orbitCount].transform.SetParent(_orbitManagerList[i].CenterRotation.transform);
                orbitCount++;
            }
        }
    }

    public void RemoveMask(HookPoint _hookPoint) {
        HookPointList.Remove(_hookPoint);
    }

    public void ResetPointPosition(BossController _boss) {
        for (int i = 0; i < EndPoints.Count; i++) {
            EndPoints[i].transform.position = _boss.transform.position;
            InitialPoints[i].transform.position = _boss.transform.position;
        }
    }

    //PASSARE IL TIME DELLA MASK
    public void SetObjectsPosition(float _initialRadius , float _finalRadius , int _index , float _time , float _orientation) {
            InitialPoints[_index].transform.eulerAngles = new Vector3(InitialPoints[_index].transform.eulerAngles.x, _orientation, InitialPoints[_index].transform.eulerAngles.z);
            InitialPoints[_index].transform.DOLocalMove(InitialPoints[_index].transform.forward * _initialRadius , _time).OnComplete(()=> { 
                                                                                                                          OrbitList[_index].transform.position = InitialPoints[_index].transform.position;
                                                                                                                          OrbitList[_index].transform.eulerAngles = InitialPoints[_index].transform.eulerAngles;
                EndPoints[_index].transform.eulerAngles = new Vector3(EndPoints[_index].transform.eulerAngles.x, _orientation, EndPoints[_index].transform.eulerAngles.z);
                EndPoints[_index].transform.DOLocalMove(EndPoints[_index].transform.forward * _finalRadius, _time).OnComplete(()=> { MoveMasks(_index, 5); });
            });
    }


    public void SetMasks(int _index , float _time) {
        OrbitList[_index].transform.DOMove(InitialPoints[_index].transform.position, _time);
    }

    public void MoveMasks(int _index , float _time) {
            OrbitList[_index].transform.DOMove(EndPoints[_index].transform.position, _time);
    }

}
