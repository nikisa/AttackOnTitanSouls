using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveToState : BossBaseState
{
    //private
    FirstBossController boss;
    PlayerController player;
    Quaternion bossRotation;
    Vector3 targetDirection;
    
    Vector3 target;
    float StartY;
    float distance;
    float loops;
    //public 
    public GameObject Target;
    public float MaxSpeed;
    [Tooltip(" n frames accelerazione , è ancora in sec")]
    public float framesZeroToMax;
    public int Loops;
    public bool Reinitialize; // immensa stronzata come rimediare ?
    public BossBaseState Stop;
    public BossBaseState ExitState;
   
    // Start is called before the first frame update
    void Start()
    {
        //framesZeroToMax = framesZeroToMax * 60;
        loops = Loops;
    }
    public override void Enter( FirstBossController _boss, PlayerController _player)
    {
        boss = _boss;

        if (Reinitialize)
        {
            loops = Loops;
        }
        StartY = boss.transform.position.y;
        if (loops <= 0 )
        {
            boss.ChangeState(ExitState);
        }
        ChargeAttack();
    }
    public override void Tick()
    {
        distance= Vector3.Distance(boss.transform.position, target);
        if (distance <= 1)
        {
            boss.ChangeState(Stop);
        }
        Acceleration();
        Move();
        

    }
    public override void Exit()
    {
        
    }
    public void ChargeAttack()
    {
        //Anticipation();
        loops--;

        Debug.Log("attack");
        target = new Vector3 (Target.transform.position.x,StartY,Target.transform.position.z);
        

        Rotate();
       
    }
    public void Rotate()
    {
        targetDirection = target - boss.transform.position;

        bossRotation = Quaternion.LookRotation(-targetDirection);
        boss.transform.rotation = bossRotation;
    }
    public void Move()
    {
        boss.transform.Translate(Vector3.back * boss.MoveSpeed * Time.deltaTime);
    }
    public void Acceleration()
    {
        boss.MoveSpeed+= framesZeroToMax * Time.deltaTime;
        boss.MoveSpeed = Mathf.Clamp(boss.MoveSpeed, 0, MaxSpeed);
    }
 
}
