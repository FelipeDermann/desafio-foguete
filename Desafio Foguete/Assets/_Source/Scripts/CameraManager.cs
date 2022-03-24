using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineFreeLook cam;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        cam = GetComponentInChildren<CinemachineFreeLook>();
    }

    public void LaunchCameraChange()
    {
        cam.m_YAxis.m_InputAxisValue = -0.2f;
    }
}
