using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] FirstPersonCamera firstPersonCameraScript;
    CinemachineFreeLook thirdPersonCamera;
    CinemachineVirtualCamera firstPersonCamera;
    bool toggle;
    
    private void Awake()
    {
        thirdPersonCamera = GetComponentInChildren<CinemachineFreeLook>();
        firstPersonCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        // Defaulting cmera perspective.
        thirdPersonCamera.Priority = 0;
        firstPersonCamera.Priority = 1;

        // Removes the cursor from the screen.
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void OnSwitchCameraPressed()
    {
        // Toggles the camera mode.
        toggle = !toggle;

        if(toggle)
        {
            // Switch to third person.
            thirdPersonCamera.Priority = 1;
            firstPersonCamera.Priority = 0;
            
            // Disables first person camera controls.
            firstPersonCameraScript.enabled = false;
        }
        else
        {
            // Switch to first person.
            thirdPersonCamera.Priority = 0;
            firstPersonCamera.Priority = 1;

            // Enables first person camera controls.
            firstPersonCameraScript.enabled = true;
        }
    }
}
