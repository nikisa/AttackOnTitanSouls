using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossOrbitManager : MonoBehaviour
{
    //Events
    public delegate void MaskEvent();
    public static MaskEvent ChangedStateEvent;
    
    public delegate void MaskBounceEvent(Collider collider);
    //public static MaskBounceEvent BounceMasks;
    //public static MaskBounceEvent BounceMasksOnWall;


    private void OnEnable() {
        ChangedStateEvent += StopMaskMovement;
        //BounceMasks += AllMasksBounce;
        //BounceMasksOnWall += AllMasksBounceOnWall;
    }

    private void OnDisable() {
        ChangedStateEvent -= StopMaskMovement;
        //BounceMasks -= AllMasksBounce;
        //BounceMasksOnWall -= AllMasksBounceOnWall;
    }

    //Inspector
    public FirstBossController boss;
    public List<GameObject> OrbitList;
    public List<FirstBossMask> MasksList;
    public BossFoV BossFov;
    public LayerMask layerMask;
    //public AnimationCurve Curve;

    //Public
    [HideInInspector]
    public List<GameObject> EndPoints;
    [HideInInspector]
    public List<GameObject> InitialPoints;
    //[HideInInspector]
    //public List<OrbitData> OrbitDataList;
    [HideInInspector]
    public List<MaskBehaviourData> MasksBehaviourList;
    [HideInInspector]
    public List<int> removedIndexList;
    [HideInInspector]
    public int countMasksArrived;
    [HideInInspector]
    public float actualSpeed;
    //[HideInInspector]
    public List<OrbitManagerData> OrbitManagerDataList;
    [HideInInspector]
    public float distanceFromBoss;
    [HideInInspector]
    public int hitMaskIndex;

    // n oggetto hittato 1 = wall , 2 = player 
    [HideInInspector]
    public int ObjHit;

    //Private
    bool hasFinished;
    float timeAcceleration;
    RaycastHit hit;
    


    void Start() {
        hasFinished = false;
    }


    public void SetupMask(List<MaskBehaviourData> _maskBehaviourList) {
        for (int i = 0; i < MasksList.Count; i++) {
            if (_maskBehaviourList[i].isSetup)
            {
            MasksList[i].SetRadius(_maskBehaviourList[i].SetupRadius);
            }
            else {
                MasksList[i].SetRadius(_maskBehaviourList[i].FinalRadius);
            }
            MasksList[i].setAngularDecelerationModule(_maskBehaviourList[i].AngularMaxSpeed , _maskBehaviourList[i].AngularDecelerationTime);
            //if (_maskBehaviourList[i].HasDeltaRadius) {
            //    MasksList[i].SetDistanceFromBoss(_maskBehaviourList[i].FinalRadius);
            //}
            //else {
            //    MasksList[i].SetDistanceFromBoss(_maskBehaviourList[i].SetupRadius);
            //}
        }
    }

    public float GetMaxMasksDistance() {
        float maxDistance = 0;
        for (int i = 0; i < MasksList.Count; i++) {
            if (MasksList[i].distanceFromBoss > maxDistance) {
                maxDistance = MasksList[i].distanceFromBoss;
            }
        }
        distanceFromBoss = maxDistance;
        return maxDistance;
    }

    public void RotationMask(List<MaskBehaviourData> _maskBehaviourList) {
        for (int i = 0; i < MasksList.Count; i++) {
            Debug.Log("RotateAround: " + i);
            MasksList[i].RotateAroud(_maskBehaviourList[i].AngularMaxSpeed , _maskBehaviourList[i].AngularAccelerationTime);
        }
    }

    public void DecelerationMask() {
        for (int i = 0; i < MasksList.Count; i++) {
            MasksList[i].DecelerateAround(MasksList[i].AngularDecelerationModule);
        }
    }


    public void ReorderMasksID(int _removedID) {
        for (int i = 0; i < MasksList.Count; i++) {
            if (MasksList[i].MaskID > _removedID) {
                MasksList[i].MaskID--;
            }
        }
    }

    public void AllMasksBounce(Collider collider) {

        /// Anziché fare il bounce Movement su tutte le maschere
        /// farlo solo su quella colpita.
        /// Poi in un'altra funzione passare il valore dell'angularVelocity ottenuto
        /// e passarlo a tutte le altre maschere


        //for (int i = 0; i < MasksList.Count; i++) {
        // MasksList[i].BounceMovement(collider );
        //}

    }

    public void AllMasksBounceOnWall(Collider collider) {

        //for (int i = 0; i < MasksList.Count; i++) {
        //    MasksList[i].MaskBounceWall(boss.CollidedObjectCollider, bounceData.kinetikEnergyLoss, bounceData.surfaceFriction, bounceData.impulseDeltaTime);
        //}
    }

    public bool checkCorrectPosition(int _index , int _id) {

        Debug.DrawRay(boss.transform.position, EndPoints[_index].transform.position - boss.transform.position, Color.red, 3);

        if (Physics.Raycast(boss.transform.position , EndPoints[_index].transform.position - boss.transform.position , out hit , Mathf.Infinity , layerMask)) {
            //Debug.Log("STOPPED MASK");
            if (hit.collider.gameObject.transform.parent.GetComponent<FirstBossMask>()) {
                
                if (hit.collider.gameObject.transform.parent.GetComponent<FirstBossMask>().MaskID == _id) {
                    
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false; 
            }
            
        }
        else {
            return false;
        }
    }

    public void SetCurrentRadius(List<MaskBehaviourData> _maskBehaviourList) {

        float speed;
        float distance;

        for (int i = 0; i < MasksList.Count; i++) {
            distance = _maskBehaviourList[i].FinalRadius - _maskBehaviourList[i].SetupRadius;
            speed = distance / _maskBehaviourList[i].TravelTime * Time.deltaTime;
            //MasksList[i].SetRadius(MasksList[i].currentRadius += speed);
            //MasksList[i].currentRadius = Mathf.Clamp(MasksList[i].currentRadius, _maskBehaviourList[i].SetupRadius, _maskBehaviourList[i].FinalRadius);
            if (_maskBehaviourList[i].HasDeltaRadius)
            {
            GetMaskById(_maskBehaviourList[i].ID).SetRadius(GetMaskById(_maskBehaviourList[i].ID).currentRadius += speed);
            GetMaskById(_maskBehaviourList[i].ID).currentRadius = Mathf.Clamp(GetMaskById(_maskBehaviourList[i].ID).currentRadius, _maskBehaviourList[i].SetupRadius, _maskBehaviourList[i].FinalRadius);
            }
           //Debug.Log( Curve.Evaluate(Time.deltaTime) +"curvaaaaaaa");
          
        }
    }
    public void ResetVelocity()
    {
        for (int i = 0; i < MasksList.Count; i++)
        {
            MasksList[i].RotationReset();
        }
    }

    //REFACTORARE LA FUNZIONE USANDO UNA STRUTTURA DATI CHE UTILIZZA L'ID COME CHIAVE
    public FirstBossMask GetMaskById(int _id) {

        FirstBossMask currentMask = null;

        for (int i = 0; i < MasksList.Count; i++) {
            if (MasksList[i].MaskID == _id) {
                currentMask = MasksList[i];
            }
        }
        return currentMask;
    }

    public void RemoveMask(FirstBossMask _hookPoint) {
        MasksList.Remove(_hookPoint);
    }


    public void setDetectedMask() {

        for (int i = 0; i < MasksList.Count; i++) {
            if (Physics.Raycast(transform.position, EndPoints[i].transform.position - transform.position, out hit , Mathf.Infinity, layerMask)) {
                hit.collider.GetComponent<FirstBossMask>().isDetected = true;
            }
        }
        
    }

    public void ResetPointPosition(BossController _boss) {
        for (int i = 0; i < EndPoints.Count; i++) {
            EndPoints[i].transform.position = _boss.transform.position;
            InitialPoints[i].transform.position = _boss.transform.position;
        }
    }

    public void SetObjectsPosition(float _initialRadius, float _finalRadius, int _index, float _time, float _orientation, float _movementTime, bool _hasDeltaRadius, bool _isSetUp) {

        _orientation += 180;

       // if (_isSetUp) {
            InitialPoints[_index].transform.eulerAngles = new Vector3(InitialPoints[_index].transform.eulerAngles.x, _orientation, InitialPoints[_index].transform.eulerAngles.z);
            InitialPoints[_index].transform.DOLocalMove(InitialPoints[_index].transform.forward * _initialRadius, _time).OnComplete(() => {
                //if (_isSetUp)
                //{
                //    SetMasks(_index, 0);
                //}
                //else {
                //    NotSetMasks(_index, 0);
                //}
                RotateMask(_index, _movementTime);
               
                EndPoints[_index].transform.eulerAngles = new Vector3(EndPoints[_index].transform.eulerAngles.x, _orientation, EndPoints[_index].transform.eulerAngles.z);
                EndPoints[_index].transform.DOLocalMove(EndPoints[_index].transform.forward * _finalRadius, _time).OnComplete(() => {
                    if (_isSetUp) {
                        NotMoveMasks(_index, _movementTime);
                    }
                    else {
                        MoveMasks(_index, _movementTime);
                    }
                });//EndPoints OnComplete
            });//InitialPoints OnComplete
      //  }
        //else {
        //    EndPoints[_index].transform.eulerAngles = new Vector3(EndPoints[_index].transform.eulerAngles.x, _orientation, EndPoints[_index].transform.eulerAngles.z);
        //    EndPoints[_index].transform.DOLocalMove(EndPoints[_index].transform.forward * _finalRadius, _time).OnComplete(() => {
        //        if (_hasDeltaRadius) {
        //            MoveMasks(_index, _movementTime);
        //        }
        //    });
        //}

    }


    public void SetMasks(int _index, float _time) {
        OrbitList[_index].transform.DOMove(InitialPoints[_index].transform.position, _time);
    }

    public void NotSetMasks(int _index, float _time) {
        OrbitList[_index].transform.DOMove(EndPoints[_index].transform.position, _time);
    }

    public void RotateMask(int _index, float _time)
    {
        OrbitList[_index].transform.DORotate(InitialPoints[_index].transform.eulerAngles, _time);
    }


        public void NotMoveMasks(int _index, float _time) {
                OrbitList[_index].transform.DORotate(InitialPoints[_index].transform.eulerAngles, _time);
                OrbitList[_index].transform.DOMove(InitialPoints[_index].transform.position, _time);
        }

        public void MoveMasks(int _index, float _time) {
            OrbitList[_index].transform.DORotate(EndPoints[_index].transform.eulerAngles, _time);
            OrbitList[_index].transform.DOMove(EndPoints[_index].transform.position, _time);
        }

    //public void FillOrbitData(OrbitData _orbitData) {
    //    OrbitDataList.Add(_orbitData);
    //}

    //public void RemoveOrbitDataList() {
    //    for (int i = 0; i < removedIndexList.Count; i++) {
    //        OrbitDataList.RemoveAt(removedIndexList[i]);
    //    }
    //}

    //public void EmptyOrbitDataList() {
    //    Debug.Log("Clear");
    //    OrbitDataList.Clear();
    //}

    public void StopMaskMovement() {
            for (int i = 0; i < OrbitList.Count; i++) {
                OrbitList[i].transform.DOKill(true);
            }
        }


    #region     FUNCTIONS CEMETERY
    //public void RotationMove(float _maxSpeed, float _timeAcceleration, HookPointController _centerPoint) {
    //    _maxSpeed /= 60;
    //    if (_maxSpeed >= 0) {
    //        timeAcceleration = _maxSpeed / _timeAcceleration;
    //        _centerPoint.MoveSpeed += timeAcceleration * Time.fixedDeltaTime;
    //        _centerPoint.MoveSpeed = Mathf.Clamp(_centerPoint.MoveSpeed, 0, _maxSpeed);
    //        _centerPoint.transform.Rotate(Vector3.up * _centerPoint.MoveSpeed);
    //    }
    //    else {
    //        _maxSpeed = Mathf.Abs(_maxSpeed);
    //        timeAcceleration = _maxSpeed / _timeAcceleration;
    //        _centerPoint.MoveSpeed += timeAcceleration * Time.fixedDeltaTime;
    //        _centerPoint.MoveSpeed = Mathf.Clamp(_centerPoint.MoveSpeed, 0, _maxSpeed);
    //        _centerPoint.transform.Rotate(Vector3.down * _centerPoint.MoveSpeed);
    //    }
    //}

    //public void OrbitDeceleration(float _maxSpeed, float _timeDeceleration, HookPointController _centerPoint) {

    //    _maxSpeed /= 60;

    //    if (_maxSpeed > 0) {
    //        _centerPoint.transform.Rotate(Vector3.up * _centerPoint.MoveSpeed);
    //        _timeDeceleration = _maxSpeed / _timeDeceleration;
    //        _centerPoint.MoveSpeed -= _timeDeceleration * Time.fixedDeltaTime;
    //        _centerPoint.MoveSpeed = Mathf.Clamp(_centerPoint.MoveSpeed, 0, _maxSpeed); //Se vogliono che rimanga fermo --> 0 anziche Mathf.Abs(_lowSpeed)
    //    }
    //    else {
    //        _maxSpeed = Mathf.Abs(_maxSpeed);
    //        _centerPoint.transform.Rotate(Vector3.down * _centerPoint.MoveSpeed);
    //        _timeDeceleration = _maxSpeed / _timeDeceleration;
    //        _centerPoint.MoveSpeed -= _timeDeceleration * Time.fixedDeltaTime;
    //        _centerPoint.MoveSpeed = Mathf.Clamp(_centerPoint.MoveSpeed, 0, _maxSpeed); //Se vogliono che rimanga fermo --> 0 anziche Mathf.Abs(_lowSpeed)
    //    }
    //}


    //public void SetHookPoints() {
    //    for (int i = 0; i < OrbitList.Count; i++) {
    //        HookPointList[i] = OrbitList[i].transform.GetChild(1).GetComponent<FirstBossMask>();
    //    }
    //}

    //public void SetUp() {
    //    timeAcceleration = 0;
    //}

    //Riempie i CenterRotation basandosi sui OrbitDataManager
    //public void SetMasksRotation(List<OrbitManagerData> _orbitManagerList) {
    //    int orbitCount = 0;
    //    for (int i = 0; i < _orbitManagerList.Count; i++) {
    //        for (int y = 0; y < _orbitManagerList[i].orbitData.Count; y++) {
    //            if (OrbitList.Count > orbitCount) {
    //                this.OrbitList[orbitCount].transform.SetParent(_orbitManagerList[i].CenterRotation.transform);
    //                orbitCount++;
    //            }
    //        }
    //    }
    //}
    #endregion
}
