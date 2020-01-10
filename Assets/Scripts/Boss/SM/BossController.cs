using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
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

    //Private
    int HookPointLayerMask;
    Vector3 targetDirection;
    Quaternion bossRotation;
    
    protected virtual void Start() {
        HookPointLayerMask = 1 << 10 | 1 << 11;
        foreach (var item in animator.GetBehaviours<BossBaseState>()) {
            item.SetContext(this, animator);
        }
        skin = 4.2f;
    }


    // Boss Movement   
    public void Move()
    {
        transform.Translate( Vector3.forward * MoveSpeed * Time.deltaTime);
    }

    public void NegativeMove() {
        transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
    }

    //Movement Acceleration
    public void Acceleration(float _timeAcceleration, float _maxSpeed) {
        _timeAcceleration = _maxSpeed / _timeAcceleration;
        MoveSpeed += _timeAcceleration * Time.deltaTime;
        MoveSpeed = Mathf.Clamp(MoveSpeed, 0, _maxSpeed);
    }

    //Movement Deceleration
    public void Deceleration(float _timeDeceleration ,float _lowSpeed , float _maxSpeed)
    {
       _timeDeceleration = _maxSpeed / _timeDeceleration;
       MoveSpeed -= _timeDeceleration * Time.deltaTime;
        if (_lowSpeed>=0) {
           MoveSpeed = Mathf.Clamp(MoveSpeed, _lowSpeed, _maxSpeed);
        }
        else {
            MoveSpeed = Mathf.Clamp(MoveSpeed, Mathf.Abs(_lowSpeed), _maxSpeed); //Se vogliono che rimanga fermo --> 0 anziche Mathf.Abs(_lowSpeed)
        }
    }

    // Logic rotation of the boss based on the target direction
    public void RotateTarget(Vector3 _target)
    {
        targetDirection = _target - transform.position;
        bossRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = bossRotation;
    }


    /// <summary>
    /// Check the object category when collides with it using interpolation technique
    /// </summary>
    /// <returns> 
    /// 0 = no collision
    /// 1 = wall
    /// 2 = player 
    /// </returns>
    public int MovingDetectCollision(int _iteration)
    {
        int result = 0;
<<<<<<< HEAD
        float skin = 0;
=======
       
>>>>>>> 0bc5c42925f82abfa9d33b6d40e86304565fbd3c

        if (Mathf.Sqrt(MoveSpeed) < 0.001) result = 0;

        int interpolation = 30;//(int)(MoveSpeed / 1f);
        for (int i = 0; i < interpolation; i++)
        {
            if (Mathf.Sqrt(MoveSpeed) < 0.001) result = 0; ;

            float time = Time.deltaTime / interpolation;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 1.1f, skin, MoveSpeed * Vector3.forward, (MoveSpeed * time) , HookPointLayerMask);
            
            if (hits == null || hits.Length == 0)
            {
                 result=0;
            }
            else
            {
                if (hits[0].collider.tag == "Player")
                {
                    result = 2;
                }
                else
                {
                    result = 1;
                    
                }
            }
        }
        return result;
    }

    //SetTarget with enum could be useful in future
    #region
    //public GameObject SetTarget(BossController.Targets _enumTarget)
    //{
    //    GameObject result = null;
    //    switch (_enumTarget)
    //    {
    //        case BossController.Targets.Player:
    //            result = FindObjectOfType<PlayerController>().gameObject;
    //            break;
    //    }
    //    return result;
    //}
    #endregion

    public MoveToData GetMoveToData()
    {
        return moveToData;
    }

    public enum Targets
    {
        Player,
    };

}