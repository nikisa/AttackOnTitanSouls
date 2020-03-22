using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunnedState : PlayerBaseState
{
    //Inspector
    public DecelerationData decelerationData;


    //Private
    float timeStart;

    public override void Enter() {
        GetDamage();
        timeStart = Time.time;
    }

    public override void Tick() {


        //To Do: Bounce formula

        //player.CharacterController.Move(-player.VelocityVector);

        float vectorAngle = Vector3.SignedAngle(Vector3.forward, player.VelocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
        player.DecelerationVector = new Vector3(Mathf.Sin(vectorAngle) * decelerationData.Deceleration, 0, Mathf.Cos(vectorAngle) * decelerationData.Deceleration);

        //Debug.DrawRay(player.transform.position, player.VelocityVector, Color.cyan, 0.2f);

        player.VelocityVector -= player.DecelerationVector * Time.deltaTime;
        player.move = player.VelocityVector * Time.deltaTime;
        player.CharacterController.Move(player.move + Vector3.down * player.gravity);

        animator.SetFloat("Timer" ,Time.time - timeStart); // base.Tick() ???
    }

    public override void Exit() {
        animator.SetFloat("Timer", 0);
    }


    void GetDamage() { 
        if (!player.IsImmortal) {
            PlayerController.DmgEvent();
        }
    }

}
