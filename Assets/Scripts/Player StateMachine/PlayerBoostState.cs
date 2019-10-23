using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoostState : PlayerIdleState
{
    //Inspector
    public float BoostSpeed;
    public float DecelerationTime;
    public float BoostTimeFreeze;
    public float AngularSpeed;
    public float ChargingSpeed;
    public float StunDuration;



    public override void Enter(PlayerController _playerController, Rigidbody _rb) {
        
    }
    public override void Tick() {
        
    }

    public override void Exit() {
        
    }


}
