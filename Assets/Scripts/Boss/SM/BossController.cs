using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator animator;
    public GameObject debug;
    public BossView View;
    public PlayerController Player;
    public GameObject Graphics;
    //[HideInInspector]
    public float MoveSpeed;
    // [HideInInspector]
    public float RotationSpeed;
    [HideInInspector]
    public MoveToData moveToData;
    [HideInInspector]
    public float MaxSpeed;

    public string CurrentStateBoss;


    //private 

    Vector3 targetDirection;
    Quaternion bossRotation;
    RaycastHit hitWalls;
    RaycastHit hitChase;
    float negativeRotationSpeed;
    float distance;
    float playerDirection;
    int HookPointLayerMask;
    // Start is called before the first frame update

    public void BossSetup()
    {

    }
    protected virtual void Start()
    {
        HookPointLayerMask = 1 << 10 | 1 << 11;
        foreach (var item in animator.GetBehaviours<BaseState>()) {
            item.SetContext(this, animator);
        }

    }

    public void Move()
    {
        transform.Translate( Vector3.forward * MoveSpeed * Time.deltaTime);
        //Graphics.transform.Translate(transform.forward * MoveSpeed * Time.deltaTime);
    }

    public void NegativeMove() {
        transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
        //Graphics.transform.Translate(transform.forward * MoveSpeed * Time.deltaTime);
    }
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
    public void RotateTarget(Vector3 _target)
    {
 
        targetDirection = _target - transform.position;

        bossRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = bossRotation;
       
    }
    public void Acceleration(float _timeAcceleration,float _maxSpeed)
    {
        _timeAcceleration = _maxSpeed / _timeAcceleration;
        MoveSpeed += _timeAcceleration * Time.deltaTime;
        MoveSpeed = Mathf.Clamp(MoveSpeed, 0, _maxSpeed);
    }
   
    void OnCollisionEnter(Collision collision)
    {
       
        
    }
    /// <summary>
    /// 0 = no collision
    /// 1 = wall
    /// 2 = player
    /// </summary>
    /// <returns></returns>
    public int DetectCollision()
    {
        int result = 0;
        float skin = 4.2f;
        float softSkin = 0.01f;

        if (Mathf.Sqrt(MoveSpeed) < 0.001) result = 0; ;

        //int interpolation = (int)(MoveSpeed / 1f) ;
        int interpolation = 30;
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
                    Debug.Log("PLAYER");
                    result = 2;
                }
                else
                {
                    Debug.Log(hits[0].collider.name);
                    result = 1;
                }
               
            }

        }
        return result;
    }
    public RaycastHit RaycastCollision() {
        Physics.Raycast(transform.position, transform.forward, out hitWalls, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        return hitWalls;
    }

    public float CollisionDistance(Vector3 _hitWalls)
    {
        distance = Vector3.Distance(_hitWalls, transform.position);
        return distance;
    }
    public GameObject SetTarget(BossController.Targets _enumTarget)
    {
        GameObject result = null;
        switch (_enumTarget)
        {
            case BossController.Targets.Player:
                result = FindObjectOfType<PlayerController>().gameObject;
                break;

        }

        return result;



    }
    public GameObject SetTargetOrbit(BossController.TargetsOrbit _enumTarget)
    {
        GameObject result = null;
        switch (_enumTarget)
        {
            case BossController.TargetsOrbit.Boss:
                result = FindObjectOfType<BossController>().gameObject;
                break;
            case BossController.TargetsOrbit.CenterPoint:
                result = GameObject.FindGameObjectWithTag("CenterPoint");
                break;
            case BossController.TargetsOrbit.Tentacle:
                result = GameObject.FindGameObjectWithTag("Tentacle");
                break;

        }

        return result;



    }
    public MoveToData GetMoveToData()
    {
        return moveToData;
    }
    public enum Targets
    {
        Player,

    };
    public enum TargetsOrbit
    {
        Boss,
        CenterPoint,
        Tentacle,
    }
}