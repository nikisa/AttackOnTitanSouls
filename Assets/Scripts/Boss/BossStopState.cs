using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStopState : BossBaseState
{
  
    FirstBossController boss;
    PlayerController player;
    float TimeStart;
    bool stop;
    //
    public float decelRatePerSec;
    public float TimeStop;
    public BossChargeState Charge;
   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Enter( FirstBossController _boss, PlayerController _player)
    {
        boss = _boss;
        stop = false;
        player = _player;
        TimeStart = 99999;
        Debug.Log("stop");
  
    }
    public override void Tick()
    {

        Deceleration();
        Move();
        if (boss.MoveSpeed <= 0 && !stop )
        {
            stop = true;
            TimeStart = Time.time;
            Debug.Log(TimeStart);
        }
        if((Time.time-TimeStart) > TimeStop)
        {
           
            boss.ChangeState(Charge);
        }
       
    }
    public override void Exit()
    {
        
    }
    public void Deceleration()
    {
        boss.MoveSpeed -= decelRatePerSec * Time.deltaTime;
        boss.MoveSpeed = Mathf.Clamp(boss.MoveSpeed, 0, 100);
    }
    public void Move()
    {
        boss.transform.Translate(Vector3.back * boss.MoveSpeed * Time.deltaTime);
    }
}
