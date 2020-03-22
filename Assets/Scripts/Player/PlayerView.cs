using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    //Inpsector
    public PlayerController Player;
        
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.GetComponent<BossController>()) {
            Debug.Log("Hit in idle");
            Player.BounceMovement(collision.collider);
            Player.animator.SetTrigger("Stunned");
        }
    }


}
