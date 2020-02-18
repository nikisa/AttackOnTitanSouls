using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashDecelerationState : PlayerBaseState
{

    //Private
    PlayerDashData playerDashData;
    PlayerDecelInTimeData playerDecelInTimeData;
    PlayerMovementData playerMovementData;
    float InitialVelocity;
    float DashTimeFrames;
    float timeDeceleration;
    float HorizontalDash;
    float VerticalDash;
    float Horizontal;
    float Vertical;
    bool IsTimerSet;
    

    public override void Enter() {

        

        IsTimerSet = false;
        Debug.Log("DECEL DASH");

        //HorizontalDash = player.horizontalDash;
        //VerticalDash = player.verticalDash;

        playerDashData = player.playerDashData;
        playerMovementData = player.playerMovementData;
        //playerDecelInTimeData = player.playerDecelInTimeData;


        //___________NEW___________
        player.DecelerationModule = player.dashVelocityModule / playerDashData.DashDecelerationTime;
        //___________NEW___________
        Debug.Log("(DASH DECEL) dashVelocityModule: " + player.dashVelocityModule);
        Debug.Log("(DASH DECEL) DecelerationModule: " + playerDashData.DashDecelerationTime);


    }

    public override void Tick() {

        //player.DashDeceleration(HorizontalDash , VerticalDash , playerDashData.DashDecelerationTime , playerDashData.ActiveDashDistance , playerDashData.ActiveDashTime);

        player.dashVelocityModule = player.VelocityVector.magnitude;
        


        if (player.dashVelocityModule <= (playerDashData.ResumePlayerInput * playerMovementData.maxSpeed)) {

            if (!IsTimerSet) //Come trattarlo senza booleana?
            {
                PlayerController.TimerEvent();
                IsTimerSet = true;
            }


            if (player.newInput) {
                animator.SetTrigger(MOVEMENT);
            }
        }


        if (player.dashVelocityModule < (player.DecelerationModule * Time.deltaTime)) {
            player.VelocityVector = Vector3.zero;
            animator.SetTrigger(IDLE);

        }



        player.newDashDeceleration();

        //if (player.dashMovementSpeed <= (playerDashData.ResumePlayerInput * playerMovementData.maxSpeed)) {

        //    if (!IsTimerSet) // da sitemare
        //    {
        //        PlayerController.TimerEvent();
        //        IsTimerSet = true;
        //    }
        //    Horizontal = player.dataInput.Horizontal; /*player.horizontalDash;*/
        //    Vertical = player.dataInput.Vertical; /*player.verticalDash;*/

        //    if (Vertical != 0 || Horizontal != 0 || player.dashMovementSpeed == 0) {
        //        Debug.Log(player.dashMovementSpeed + " --player.dashMovementSpeed-- ");
        //        animator.SetTrigger(IDLE);
        //    } 
        //}
    }

    public override void Exit() {
        
    }
}
