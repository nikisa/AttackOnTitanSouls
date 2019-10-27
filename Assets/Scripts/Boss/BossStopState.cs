using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStopState : BossBaseState
{
    Rigidbody rb;
    FirstBossController boss;
    PlayerController player;
    float TimeStart;
    //
    public float decelRatePerSec;
    public float TimeStop;
    public BossBaseState Idle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Enter(Rigidbody _rb, FirstBossController _boss, PlayerController _player)
    {
        boss = _boss;
        rb = _rb;
        player = _player;
        TimeStart = Time.time;
        Debug.Log("stop");
        boss.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }
    public override void Tick()
    {
        if ((Time.time - TimeStart) > TimeStop)
        {
            boss.ChangeState(Idle);
        }
        boss.MoveSpeed -= decelRatePerSec * Time.deltaTime;
        boss.MoveSpeed = Mathf.Clamp(boss.MoveSpeed, 0, 100);
        rb.velocity= -boss.transform.forward * boss.MoveSpeed;
    }
    public override void Exit()
    {
        
    }
}
