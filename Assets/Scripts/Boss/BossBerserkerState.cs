using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBerserkerState : BossBaseState
{
    //private
    int layerMask;
    Rigidbody rb;
    FirstBossController boss;
    PlayerController player;
    Vector3 PlayerDirection;
    RaycastHit hit;
    float startingY;
    float distance;
    //temporary
    float speedTemp;
    Quaternion bossRotation;
    int charges;

    //public
    public int Charges;
    public float ChargeSpeed;
    public BossBaseState Idle;
    [Tooltip (" distance between boss and wall to get collision")]
    public float WallCollisionDistance;
   
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Walls");

    }
    public override void Enter(FirstBossController _boss, PlayerController _player)
    {
        boss = _boss;
        player = _player;
        startingY = boss.transform.position.y;
        speedTemp = ChargeSpeed;
        ChargeSpeed = 0;
        charges = Charges;
        charges--;
        ChargeAttack();
       
        
    }
    public override void Tick()
    {
        if (hit.point != null)
        {
            // wallcollisiondistance da calcolare da codire ?
            distance= Vector3.Distance(boss.transform.position, hit.point);
            Debug.Log(distance);
            if(distance < WallCollisionDistance)
            {
                ChargeAttack();
                charges--;

            }
            if(charges <= 0)
            {
                boss.ChangeState(Idle);
            }
            Move();
        }
      
    
        
    }
    public override void Exit()
    {
     
    }
    public void ChargeAttack()
    {
        Debug.Log("Attack");
        PlayerDirection = player.transform.position - boss.transform.position;
       
        bossRotation = Quaternion.LookRotation(-PlayerDirection);
        boss.transform.rotation = bossRotation;
        if (Physics.Raycast(boss.transform.position, PlayerDirection,out hit, 900,layerMask))
        {
            Debug.DrawRay(boss.transform.position, PlayerDirection, Color.blue, 90);
            //hit.point;
            
            ChargeSpeed = speedTemp;
            // move in hit point at charge speed
            
        }

        
    }
    public void Move()
    {
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, new Vector3 (hit.point.x, startingY, hit.point.z), ChargeSpeed * Time.deltaTime);
    }
}
