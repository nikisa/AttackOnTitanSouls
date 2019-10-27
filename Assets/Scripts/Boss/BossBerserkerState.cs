﻿using System.Collections;
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
    //temporary
    float speedTemp;
    Quaternion bossRotation;
    //public
    public int Charges;
    public float ChargeSpeed;
   
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Walls");

    }
    public override void Enter(Rigidbody _rb, FirstBossController _boss, PlayerController _player)
    {
        boss = _boss;
        rb = _rb;
        player = _player;
        startingY = boss.transform.position.y;
        speedTemp = ChargeSpeed;
        ChargeSpeed = 0;
        ChargeAttack();
       
        
    }
    public override void Tick()
    {
        if (boss.Collision == true)
        {

            ChargeAttack();
        }
        Move();
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
            Debug.Log(hit.collider.name);
            ChargeSpeed = speedTemp;
            // move in hit point at charge speed
            
        }

        
    }
    public void Move()
    {
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, new Vector3 (hit.point.x, startingY, hit.point.z), ChargeSpeed * Time.deltaTime);
    }
}
