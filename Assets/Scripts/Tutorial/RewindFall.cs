using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindFall : MonoBehaviour
{
    public Transform RespawnPoint;
    public float TimeDead;
    PlayerController player;
    float timeStart;
    // Start is called before the first frame update
    void Start()
    {
        timeStart = Mathf.Infinity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time-timeStart > TimeDead)
        {
            player.transform.position = RespawnPoint.position;
            timeStart = Mathf.Infinity;
        }
    }
    
 
    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            timeStart = Time.time;
            player = other.GetComponent<PlayerController>();
         
        }
    }
    public void OnTriggerExit(Collider other)
    {
        timeStart = Mathf.Infinity;
    }
}
