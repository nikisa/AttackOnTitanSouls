using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeState : BossBaseState
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
    public float SpeedStart;
    [Tooltip("the frames to ")]
    public float framesZeroToMax;
    public float AnticipationTime;
    public int Loops;
    public bool Reinitialize; // immensa stronzata come rimediare ?
    public BossBaseState Stop;
    public BossIdleState idle;
    public Material ColorAnticipation;
    public Material BossColor;
    // Start is called before the first frame update
    void Start()
    {
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
            boss.ChangeState(idle);
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
        boss.MoveSpeed = Mathf.Clamp(boss.MoveSpeed, 0, SpeedStart);
    }
    public void Anticipation()
    {
        StartCoroutine(WaitAnticipation());
      
    }
    public IEnumerator WaitAnticipation()
    {
        boss.GetComponent<MeshRenderer>().material = ColorAnticipation;
        yield return new WaitForSeconds(AnticipationTime);
        
        boss.GetComponent<MeshRenderer>().material = BossColor;
    }
  
}
