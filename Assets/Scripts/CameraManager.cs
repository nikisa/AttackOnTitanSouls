using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    [HideInInspector]
    public List<CinemachineVirtualCamera> cameras;
    public List<Camera> NormalCameras;
    public PlayerController player;

    public void SetActiveCamera(CinemachineVirtualCamera _camera)
    {
        SetAllPriorityAtZero();
        _camera.Priority = 100;
        //_camera.Follow = player.transform;
    }
    public void SetDisableCamera(CinemachineVirtualCamera _camera)
    {
        _camera.Follow = null;
    }
    public void SetAllPriorityAtZero()
    {
        foreach (var camera in cameras)
        {
            camera.Priority = 0;
        }
    }
    public void SetActiveNormalCamera(Camera _camera)
    {
        DisableAllCamera();
        _camera.gameObject.SetActive(true);
        
    }
    public void SetParetCamera(Camera _camera)
    {
        _camera.transform.SetParent(player.transform);
    }
    public void DisableAllCamera()
    {
        foreach (var camera in NormalCameras)
        {
            camera.gameObject.SetActive(false);
        }
    }
}
