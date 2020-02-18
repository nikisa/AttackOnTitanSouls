
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
    //bool canDash = true;    
    float startDash;
    float startTime;
    float boostTime;
    float timer;
    float accelRatePerSec;

    float forwardVelocity;
    Vector3 movementVelocity = Vector3.zero;
    DataInput dataInput;


    public override void Enter()
    {

        playerDashData = player.playerDashData;

        player.layerMask = 1 << 10 /*| 1<<12*/;
        //timer = Time.time;

        accelRatePerSec = playerMovementData.maxSpeed / (playerMovementData.AccelerationTime);
       
        forwardVelocity = 0f;
    }

    public override void Tick()
    {

        if (!player.InputDisable)
        {

           player.PlayerInclination();


            player.newMovement(playerMovementData.maxSpeed , playerMovementData.AccelerationTime);

            if (Mathf.Pow(dataInput.Horizontal, 2) + Mathf.Pow(dataInput.Vertical, 2) < Mathf.Pow(player.DeadZoneValue, 2)) { //Usare newInput anziché riscrivere la DeadZone nella condizione
                animator.SetTrigger(MOVEMENT_DECELERATION);
            }

            if (Time.time - player.timerDash > playerDashData.EnableDashAt)
            {
                player.canDash = true;
            }
            //else {
            //    player.canDash = false;
            //}

            dataInput = player.dataInput;

            if (dataInput.Dash && player.canDash /*&& ((dataInput.Horizontal == 1 || dataInput.Horizontal == -1) || (dataInput.Vertical == 1 || dataInput.Vertical == -1))*/ )
            {
                startDash = Time.time;
                player.canDash = false;
                player.horizontalDash = player.dataInput.Horizontal;
                player.verticalDash = player.dataInput.Vertical;
                animator.SetTrigger(DASH);
            }

            if (Mathf.Pow(dataInput.Horizontal, 2) + Mathf.Pow(dataInput.Vertical, 2) >= Mathf.Pow(player.DeadZoneValue, 2))
            {
                //player.ReadInputGamepad(dataInput, accelRatePerSec , playerIdleData.maxSpeed);
                player.ReadInputKeyboard(dataInput, accelRatePerSec, playerMovementData.maxSpeed);
            }
        }
        //player.CheckInput();

    }


    public override void Exit()
    {

    }
}