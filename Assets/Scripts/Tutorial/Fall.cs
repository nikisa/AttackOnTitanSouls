using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    //public Transform RespawnPoint;
    public float x;
    public float y;
    public float z;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
   
        if (other.tag == "Player" )
        {
        player = other.GetComponent<PlayerController>();
            if (!player.ImmortalTutorial)
            {
                player.StopPlayer();
                PlayerController.DisableInputEvent();
                player.transform.position = new Vector3(x,y,z);
               // Debug.Log(RespawnPoint.position);   
            }
        }
    }
 
}
