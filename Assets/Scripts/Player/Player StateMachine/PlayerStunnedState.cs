using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunnedState : PlayerBaseState
{

    //Private
    float timeStart;

    public override void Enter() {
        timeStart = Time.time;
    }

    public override void Tick() {

        float vectorAngle = Vector3.SignedAngle(Vector3.forward, player.VelocityVector.normalized, Vector3.up) * Mathf.Deg2Rad;
        player.DecelerationVector = new Vector3(Mathf.Sin(vectorAngle) * /*player.DecelerationModule*/ 10, 0, Mathf.Cos(vectorAngle) */* player.DecelerationModule*/ 10);

        //Debug.DrawRay(player.transform.position, player.VelocityVector, Color.cyan, 0.2f);

        player.VelocityVector -= player.DecelerationVector * Time.deltaTime;
        player.move = player.VelocityVector * Time.deltaTime;
        player.CharacterController.Move(player.move + Vector3.down * player.gravity);
        animator.SetFloat("Timer" ,Time.time - timeStart);
    }

    public override void Exit() {
        animator.SetFloat("Timer", 0);
    }
}
