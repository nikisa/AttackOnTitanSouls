
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
        player.layerMask = 1 << 10;
        player.AccelerationModule = playerMovementData.maxSpeed / playerMovementData.AccelerationTime;


    }

    public override void Tick()
    {

        if (!player.InputDisable)
        {
           player.PlayerInclination();

            if (player.checkDeadZone()) {
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
           
                player.canDash = false;
                player.targetDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                animator.SetTrigger(DASH);
            }    
        }
    }

}