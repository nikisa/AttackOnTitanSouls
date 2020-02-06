using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EndRoom : MonoBehaviour
{
    //public Transform RespawnPoint;
    public float x;
    public float y;
    public float z;
    public Camera NextCamera;
   // public Camera PreviousCamera;
   
    PlayerController player;
    CameraManager cameraManager;
    // Start is called before the first frame update
    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
            if (!player.ImmortalTutorial)
            {
                player.StopPlayer();
                PlayerController.DisableInputEvent();
                cameraManager.SetActiveNormalCamera(NextCamera);
                player.transform.position = new Vector3(x, y, z);
                cameraManager.SetParetCamera(NextCamera);

                // Debug.Log(RespawnPoint.position);   
            }
        }
    }

}
