using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraActivation : MonoBehaviour
{
    public CinemachineVirtualCamera camera;
    public PlayerController player;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {

        camera.Follow = player.transform;
    }
}
