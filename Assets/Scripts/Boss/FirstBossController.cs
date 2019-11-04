using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : BossControllerBase
{
    //public 
    public PlayerController Player;
    public float MoveSpeed;
         
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
            currentState.Enter(this,Player);
        }
    }

}
