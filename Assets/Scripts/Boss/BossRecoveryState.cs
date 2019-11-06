using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRecoveryState : BossBaseState
{

    FirstBossController boss;
    PlayerController player;
    float TimeStart;
    bool stop;
    //
    [Tooltip(" n frames decelerazione , è ancora in sec")]
    public float decelRatePerSec;
    public float RecoveryTime;
    public BossBaseState ExitState;



    // Start is called before the first frame update
    void Start()
    {
        //decelRatePerSec = decelRatePerSec / 60;
    }

    public override void Enter(FirstBossController _boss, PlayerController _player)
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
        if (boss.MoveSpeed <= 0 && !stop)
        {
            stop = true;
            TimeStart = Time.time;
            Debug.Log(TimeStart);
        }
        if ((Time.time - TimeStart) > RecoveryTime)
        {

            boss.ChangeState(ExitState);
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
