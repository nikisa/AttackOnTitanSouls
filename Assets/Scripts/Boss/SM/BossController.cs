using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MovementBase
{
    //Inspector
    public Animator animator;
    public BossView View;
    public PlayerController Player;
    public GameObject Graphics;
   
    //Public
    [HideInInspector]
    public float MoveSpeed;
    [HideInInspector]
    public float RotationSpeed;
    [HideInInspector]
    public MoveToData moveToData;
    [HideInInspector]
    public float MaxSpeed;
    [HideInInspector]
    public float skin;
    [HideInInspector]
    public RaycastHit hitObject;
    [HideInInspector]
    public GameObject Target;
    [HideInInspector]
    public float CycleTimer;

    //Private
    int HookPointLayerMask;
    float bossY;
    Vector3 targetDirection;
    Quaternion bossRotation;
    
    protected virtual void Start() {

        bossY = transform.position.y;

        HookPointLayerMask = 1 << 10 | 1 << 11;
        foreach (var item in animator.GetBehaviours<BossBaseState>()) {
            item.SetContext(this, animator);
        }
        skin = 4.2f;
    }

    private void Update() {
        //Just keeps the Y axes of the boss constant
        transform.position = new Vector3(transform.position.x , bossY , transform.position.z);
        
    }


    // Logic rotation of the boss based on the target direction
    public void RotateTarget(Vector3 _target)
    {
        targetDirection = _target - transform.position;
        bossRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = bossRotation;
    }

    public int DetectCollision(Vector3 _nextPosition) 
    {
        float softSkin = 0.2f;
        float skin = 5 /*Player.CharacterController.radius*/ + softSkin;

        Vector3 direction = _nextPosition - this.transform.position;

        RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, skin, direction.normalized , direction.magnitude , HookPointLayerMask);

        if (hits == null || hits.Length == 0)
        {
            return 0;
        }
        else
        {
            this.hitObject = hits[0];
            return hits[0].collider.gameObject.layer;
        }
    }

    #region OLD FUNCTIONS

    ///// <summary>
    ///// Check the object category when collides with it using interpolation technique
    ///// </summary>
    ///// <returns> 
    ///// 0 = no collision
    ///// 10 = wall layer
    ///// 11 = player layer
    ///// </returns>
    //public int MovingDetectPlayer(int _iteration) {
    //    int result = 0;
    //    float skin = 5.2f;

    //    int interpolation = _iteration;//(int)(MoveSpeed / 1f);

    //    for (int i = 0; i < interpolation; i++) {
    //        if (Mathf.Sqrt(MoveSpeed) < 0.001) result = 0;

    //        float time = Time.deltaTime / interpolation;
    //        RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 1.1f, skin, MoveSpeed * Vector3.forward, (MoveSpeed * time), HookPointLayerMask);

    //        if (hits == null || hits.Length == 0) {
    //            return result = 0;
    //        }
    //        else {
    //            result = hits[0].collider.gameObject.layer;
    //            hitObject = hits[0];
    //        }
    //    }
    //    return result;
    //}


    //// Boss Movement   
    //public void Move()
    //{
    //    transform.Translate( Vector3.forward * MoveSpeed * Time.deltaTime);
    //}

    //public void NegativeMove() {
    //    transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
    //}

    ////Movement Acceleration
    //public void Acceleration(float _timeAcceleration, float _maxSpeed) {
    //    _timeAcceleration = _maxSpeed / _timeAcceleration;
    //    MoveSpeed += _timeAcceleration * Time.deltaTime;
    //    MoveSpeed = Mathf.Clamp(MoveSpeed, 0, _maxSpeed);
    //}

    ////Movement Deceleration
    //public void Deceleration(float _timeDeceleration ,float _lowSpeed , float _maxSpeed)
    //{
    //   _timeDeceleration = _maxSpeed / _timeDeceleration;
    //   MoveSpeed -= _timeDeceleration * Time.deltaTime;
    //    if (_lowSpeed>=0) {
    //       MoveSpeed = Mathf.Clamp(MoveSpeed, _lowSpeed, _maxSpeed);
    //    }
    //    else {
    //        MoveSpeed = Mathf.Clamp(MoveSpeed, Mathf.Abs(_lowSpeed), _maxSpeed); //Se vogliono che rimanga fermo --> 0 anziche Mathf.Abs(_lowSpeed)
    //    }
    //}

    #endregion

}