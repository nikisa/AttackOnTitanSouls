using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : BossControllerBase
{
    //public 
    public PlayerController Player;
    public float MoveSpeed;
    public bool Collision;
    //public BossBaseState idleState;
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Tick();
    }
    public void ChangeState(BossBaseState _state)
    {
        if (_state != null)
        {
            currentState.Exit();
            currentState = _state;
            currentState.Enter(Rb, this,Player);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Walls")
        {
            Collision = true;
        }
       
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Walls")
        {
            Collision = falses;
        }

    }
}
