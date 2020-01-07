using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossOrbitManager : MonoBehaviour
{
    public static bool prova = false;

    //Inspector
    public List<GameObject> OrbitList;
    public List<HookPoint> HookPointList;

    //Public
    [HideInInspector]
    public List<GameObject> EndPoints;
    [HideInInspector]
    public List<GameObject> InitialPoints;
    //[HideInInspector]
    public List<OrbitData> OrbitDataList;
    [HideInInspector]
    public List<int> removedIndexList;
    
    //Private
    bool hasFinished;
    float timeAcceleration;


    void Start()
    {
        hasFinished = false;
    }

    public void RotationMove(float _maxSpeed , float _timeAcceleration , HookPointController _centerPoint)
    {
        _maxSpeed /= 60;
        _timeAcceleration /= 60;

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

    //Riempie i CenterRotation basandosi sui OrbitDataManager
    public void SetMasksRotation(List <OrbitManagerData> _orbitManagerList) {
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

    public void SetObjectsPosition(float _initialRadius , float _finalRadius , int _index , float _time , float _orientation , float _movementTime , bool _hasDeltaRadius) {
            InitialPoints[_index].transform.eulerAngles = new Vector3(InitialPoints[_index].transform.eulerAngles.x, _orientation, InitialPoints[_index].transform.eulerAngles.z);
            InitialPoints[_index].transform.DOLocalMove(InitialPoints[_index].transform.forward * _initialRadius , _time).OnComplete(()=> {
                SetMasks(_index);
                EndPoints[_index].transform.eulerAngles = new Vector3(EndPoints[_index].transform.eulerAngles.x, _orientation, EndPoints[_index].transform.eulerAngles.z);
                EndPoints[_index].transform.DOLocalMove(EndPoints[_index].transform.forward * _finalRadius, _time).OnComplete(()=> {
                    if (_hasDeltaRadius) {
                        MoveMasks(_index, _movementTime);
                    }
                    
                });
            });
    }


    public void SetMasks(int _index) {
        OrbitList[_index].transform.position = InitialPoints[_index].transform.position;
        OrbitList[_index].transform.eulerAngles = InitialPoints[_index].transform.eulerAngles;
        Debug.Log(OrbitList[_index].name);
    }

    public void MoveMasks(int _index , float _time) {
        OrbitList[_index].transform.DOMove(EndPoints[_index].transform.position, _time).OnComplete(() => BossOrbitManager.prova = true); ;
    }

    public void FillOrbitData(OrbitData _orbitData) {
        OrbitDataList.Add(_orbitData);
    }

    public void RemoveOrbitDataList() {
        for (int i = 0; i < removedIndexList.Count; i++) {
            OrbitDataList.RemoveAt(removedIndexList[i]);
        }
    }

    public void EmptyOrbitDataList() {
        OrbitDataList.Clear();
    }

}
