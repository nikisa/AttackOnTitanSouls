
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovementState : PlayerBaseState
{

    //Inspector
    public PlayerMovementData playerMovementData;

    //Public 
    [HideInInspector]
    public PlayerDashData playerDashData;


    // Private
    float startDash;
    DataInput dataInput;


    public override void Enter()
    {
        playerDashData = player.playerDashData;
        player.layerMask = 1 << 10 /*| 1<<12*/;
        player.AccelerationModule = playerMovementData.maxSpeed / playerMovementData.AccelerationTime;

        Debug.LogFormat("VelocityVector: {0}  and  Magnitude: {1} " , player.VelocityVector, player.VelocityVector.magnitude);

    }

    public override void Tick()
    {
        

        if (!player.InputDisable)
        {
           player.PlayerInclination();

            if (Mathf.Pow(Input.GetAxis("Horizontal"), 2) + Mathf.Pow(Input.GetAxis("Vertical"), 2) >= Mathf.Pow(player.DeadZoneValue, 2)) { //Usare newInput anziché riscrivere la DeadZone nella condizione
                player.targetDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                player.Movement(player.targetDir , playerMovementData.maxSpeed , player.AccelerationModule);
            }
            else {
                animator.SetTrigger(MOVEMENT_DECELERATION);
            }


            if (Time.time - player.timerDash > playerDashData.EnableDashAt)
            {
                player.canDash = true;
            }


            dataInput = player.dataInput;

            if (Input.GetButtonDown("Dash") && player.canDash)
            {
                startDash = Time.time;
                player.canDash = false;
                animator.SetTrigger(DASH);
            }    
        }
    }


    public override void Exit()
    {

    }
}