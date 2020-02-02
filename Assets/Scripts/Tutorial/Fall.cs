using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public Transform RespawnPoint;
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
                player.transform.position = RespawnPoint.position;
            }
        }
    }
    //public IEnumerable Dead()
    //{

    //    yield return new WaitForSeconds(1f);
    //}
}
