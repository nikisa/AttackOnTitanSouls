using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator animator;
    //public GameObject debug;
    public BossView View;
    [HideInInspector]
    public BossData Data;


    //private 

    Vector3 targetDirection;
    Quaternion bossRotation;
    RaycastHit hitWalls;
    float negativeRotationSpeed;
    float distance;
    // Start is called before the first frame update

    void Awake() {

        Data.bossInfo.Player = FindObjectOfType<PlayerController>();
        Data.bossInfo.Graphics = GameObject.FindGameObjectWithTag("Graphics");

        Data = View.GetBossData();  
    }

    protected virtual void Start()
    {

        foreach (var item in animator.GetBehaviours<BaseState>())
        {
            item.SetContext(this, animator);
        }
    }

    public void Move()
    {
        transform.Translate( Vector3.forward * Data.bossInfo.MoveSpeed * Time.deltaTime);
        //Graphics.transform.Translate(transform.forward * MoveSpeed * Time.deltaTime);
    }
    public void Deceleration(float _timeDeceleration ,float _lowSpeed)
    {
        _timeDeceleration = Data.accelerationInfo.MaxSpeed / _timeDeceleration;
        Data.bossInfo.MoveSpeed -= _timeDeceleration * Time.deltaTime;
        Data.bossInfo.MoveSpeed = Mathf.Clamp(Data.bossInfo.MoveSpeed, _lowSpeed, 100);
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
        Data.bossInfo.MoveSpeed += _timeAcceleration * Time.deltaTime;
        Data.bossInfo.MoveSpeed = Mathf.Clamp(Data.bossInfo.MoveSpeed, 0, _maxSpeed);
    }
   
    void OnCollisionEnter(Collision collision)
    {
       
        
    }
    public float CollisionDistance()
    {
     

        Physics.Raycast(transform.position, transform.forward, out hitWalls, 900);
    
        Debug.DrawRay(transform.position, transform.forward, Color.blue, 7); 
        distance = Vector3.Distance(hitWalls.point, transform.position);

        return distance;
    }
}