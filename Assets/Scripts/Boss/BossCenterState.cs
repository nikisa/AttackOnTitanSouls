using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCenterState : BossBaseState
{
    
    FirstBossController boss;
    PlayerController player;
    public GameObject Center;
    public float Speed;
    public BossBaseState Idle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Enter( FirstBossController _boss, PlayerController _player)
    {

        boss = _boss;
     
        player = _player;
        Move();
    }
    public override void Tick()
    {
        if (boss.transform.position == Center.transform.position)
        {
            boss.ChangeState(Idle);
        }
    }
    public override void Exit()
    {
      
    }
    public void Move()// spostare in boss
    {
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, Center.transform.position, Speed);
    }
}
