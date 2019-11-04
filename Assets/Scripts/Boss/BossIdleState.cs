using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState
{
    //private 
    Rigidbody rb;
    FirstBossController boss;
    PlayerController player;
    float TimeStart;
    float startingY;
    //temponary
    float angle;
    Quaternion bossRotation;
    Vector3 direction;
    //public 
    public float accelRatePerSec;
    public float TimeRotation;
    public float MaxSpeed;
    public float TimeChase;
    public BossStopState StopState;
    public BossBerserkerState Berserker;
    public BossChargeState Charge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Enter(FirstBossController _boss,PlayerController _player)
    {
        boss = _boss;
        player = _player;
        TimeStart = Time.time;
        startingY = boss.transform.position.y;
        boss.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        Debug.Log("idle");

    }
    public override void Tick()
    {
        if ((Time.time - TimeStart) > TimeChase)
        {
            boss.ChangeState(Charge);
        }
        //Rotation();
        //Movement();

    }
    public void Rotation()
    {
        direction = player.transform.position - boss.transform.position;
        bossRotation = Quaternion.LookRotation(-direction);
        boss.transform.rotation = bossRotation;
    }
    public void Movement() 
    {
        //boss.transform.position.Set(boss.transform.position.x, startingY,boss.transform.position.z);
        boss.MoveSpeed += accelRatePerSec * Time.deltaTime;
        boss.MoveSpeed = Mathf.Clamp(boss.MoveSpeed, 0, MaxSpeed);
       boss.transform.position= Vector3.MoveTowards(boss.transform.position, new Vector3( player.transform.position.x,startingY,player.transform.position.z), boss.MoveSpeed * Time.deltaTime);
       //rb.MovePosition( new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z) + (boss.direction* boss.MoveSpeed * Time.deltaTime));
    }
    public override void Exit()
    {
       
        
    }
}
